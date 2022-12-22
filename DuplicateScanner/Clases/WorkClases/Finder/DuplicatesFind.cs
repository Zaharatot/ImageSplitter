using DuplicateScanner.Clases.DataClases;
using DuplicateScanner.Clases.DataClases.File;
using DuplicateScanner.Clases.DataClases.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DuplicateScanner.Clases.DataClases.Global.Enums;

namespace DuplicateScanner.Clases.WorkClases.Finder
{
    /// <summary>
    /// Класс поиска дубликатов
    /// </summary>
    internal class DuplicatesFind
    {
        /// <summary>
        /// Класс сравнения хешей
        /// </summary>
        private EqualDctHash _equalHash;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicatesFind()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _equalHash = new EqualDctHash();
        }

        /// <summary>
        /// Выполняем проверку на схожесть файлов
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы являются схожими</returns>
        private bool IsFileEquals(DuplicateInfo current, DuplicateInfo toCheck) =>
            //Сравнивать просто лайновые хеши смысла нет
            //Сравниваем ДКП хеши изображений
            _equalHash.EqalImageHash(current.DcpHash, toCheck.DcpHash) ||
            //Сравниваем ДКП-хеш оригинала с лайновым ДКП-хешем сравниваемого
            _equalHash.EqalLinedImageHash(current.DcpHash, toCheck.LinedDcpHash) ||
            //Сравниваем лайновый ДКП-хеш оригинала с ДКП-хешем сравниваемого
            _equalHash.EqalLinedImageHash(current.LinedDcpHash, toCheck.DcpHash);

        /// <summary>
        /// Проверка на запрет сравнения хешей
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы не записаны как точные не дубликаты</returns>
        private bool IsDenyCheck(DuplicateInfo current, DuplicateInfo toCheck) =>
            //Сравниваемый находится в списке запрещённых у целевого
            current.ForbiddenHashes.Contains(toCheck.PathHash) ||
            //Или целевой находится в запрещённом списке у сравниваемого
            toCheck.ForbiddenHashes.Contains(current.PathHash);

        /// <summary>
        /// Выполняем проверку на дубликат двух файлов
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы являются дубликатами</returns>
        private bool IsDuplicate(DuplicateInfo current, DuplicateInfo toCheck) =>
            //Если сравниваемую картинку можно обрабатывать
            toCheck.IsAllowProcess &&
            //Если нет запрета на сравнение 
            !IsDenyCheck(current, toCheck) &&
            //И изображения похожи
            IsFileEquals(current, toCheck);


        /// <summary>
        /// Метод поиска дубликатов для файла
        /// </summary>
        /// <param name="filesToCheck">Список файлов для проверки</param>
        /// <param name="id">Идентификатор проверяемого файла</param>
        /// <returns>Список дубликатов</returns>
        private FindResult CheckDuplicates(List<DuplicateInfo> filesToCheck, int id)
        {
            //Инициализируем класс результатов поиска
            FindResult result = new FindResult(filesToCheck[id].GetResult());
            //Проходимся по всем последующим файлам, т.к.
            //все предыдущие уже были обработаны, и если
            //был дубль с текущим, то он уже в списке
            for (int i = id + 1; i < filesToCheck.Count; i++)
                //Если файлы являются дубликатами
                if (IsDuplicate(filesToCheck[id], filesToCheck[i]))
                    //Добавляем хеш в список дубликатов
                    result.Results.Add(filesToCheck[i].GetResult());
            //Возвращаем класс результатов
            return result;
        }

        /// <summary>
        /// Метод инициализации информации о прогрессе
        /// </summary>
        /// <param name="filesToCheckCount">Количество файлов для поиска дубликатов</param>
        /// <returns>Класс информации о прогрессе хеширования</returns>
        private ScanProgressInfo CreateProgressInfo(int filesToCheckCount) =>
             new ScanProgressInfo(ScanStages.DuplicateFind) {
                 FilesToProcess = filesToCheckCount
             };



        /// <summary>
        /// Метод поиска дубликатов в файлах
        /// </summary>
        /// <param name="filesToCheck">Список файлов для проверки</param>
        /// <returns>Словарь найденных дубликатов</returns>
        public List<FindResult> Find(List<DuplicateInfo> filesToCheck)
        {
            //Класс результатов поиска
            FindResult result;
            //Инициализируем список результатов поиска
            List<FindResult> duplicates = new List<FindResult>();
            //Инициализируем класс информации о прогрессе
            ScanProgressInfo info = CreateProgressInfo(filesToCheck.Count);
            //Вызываем ивент обновления прогресса
            DuplicateScannerFasade.InvokeUpdateScanInfo(info);
            //Проходимся по списку файлов
            for (int i = 0; i < filesToCheck.Count; i++)
            {
                //Если данное изображение вообще можно обрабатывать
                if (filesToCheck[i].IsAllowProcess)
                {
                    //Получаем список дубликатов для текущего файла
                    result = CheckDuplicates(filesToCheck, i);
                    //Если у файла есть дубликаты
                    if (result.IsContainDuplicates)
                        //Добавляем этот файл с его дубликатами в список
                        duplicates.Add(result);
                }
                //Обновляем количество обработанных файлов
                info.ProcessedFiles = i;
                //Вызываем ивент обновления прогресса
                DuplicateScannerFasade.InvokeUpdateScanInfo(info);
            }
            //Возвращаем список результаттов
            return duplicates;
        }
    }
}
