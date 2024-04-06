using DuplicateScannerLib.Clases.DataClases;
using DuplicateScannerLib.Clases.DataClases.File;
using DuplicateScannerLib.Clases.DataClases.Result;
using DuplicateScannerLib.Clases.WorkClases.Comparers;
using SplitterDataLib.DataClases.Global.DuplicateScan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLeftCalcZ;
using static DuplicateScannerLib.Clases.DataClases.Global.Delegates;
using static DuplicateScannerLib.Clases.DataClases.Global.Enums;

namespace DuplicateScannerLib.Clases.WorkClases.Finder
{
    /// <summary>
    /// Класс поиска дубликатов
    /// </summary>
    internal class DuplicatesFind
    {
        /// <summary>
        /// Метод сравнения хешей
        /// </summary>
        private HashComparer _hashComparer;

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
            //Инициализируем класс сравнения хешей
            _hashComparer = new HashComparer();
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
        /// Метод проверки файла
        /// </summary>
        /// <param name="filesToCheck">Список файлов для проверки</param>
        /// <param name="current">Целевой файл для проверки</param>
        /// <param name="pairs">Пары найденных элементов</param>
        private void ProcessFile(List<DuplicateInfo> filesToCheck, DuplicateInfo current, ref List<DuplicatePair> pairs)
        {
            //Для каждого хеша проходимся по всем остальным
            foreach (var toCheck in filesToCheck)
                //Если это не тот же элемент, и он похож на тестируемый
                if (_hashComparer.IsDuplicate(current, toCheck))
                {
                    //Лочим список пар
                    lock (pairs)
                        //Добавляем в него новую пару
                        pairs.Add(new DuplicatePair(current, toCheck));
                }
        }

        /// <summary>
        /// Метод обновление инфомрации для ивента
        /// </summary>
        /// <param name="info">Класс прогресса для ивента</param>
        private void UpdateEventInfo(ScanProgressInfo info)
        {
            //Обновляем количество обработанных файлов
            info.ProcessedFiles++;
            //Вызываем ивент обновления прогресса
            DuplicateScannerFasade.InvokeUpdateScanInfo(info);
        }




        /// <summary>
        /// Метод поиска дубликатов в файлах
        /// </summary>
        /// <param name="filesToCheck">Список файлов для проверки</param>
        /// <param name="properties">Параметры сканирования</param>
        /// <returns>Словарь найденных дубликатов</returns>
        public List<DuplicatePair> Find(List<DuplicateInfo> filesToCheck, ScanProperties properties)
        {
            //Инициализируем список пар копий для возврата
            List<DuplicatePair> pairs = new List<DuplicatePair>();
            //Инициализируем класс информации о прогрессе
            ScanProgressInfo info = CreateProgressInfo(filesToCheck.Count);
            //Обновляем параметры сравнения файлов
            _hashComparer.ChangeCheckProperties(properties);
            //Вызываем ивент обновления прогресса
            DuplicateScannerFasade.InvokeUpdateScanInfo(info);
            //Проходимся по файлам асинхронно, в несколько потоков
            Parallel.For(0, filesToCheck.Count, (i) => {
                //Если файл можно обрабатывать
                if (filesToCheck[i] != null && filesToCheck[i].IsAllowProcess)
                    //Выполняем обработку файла
                    ProcessFile(filesToCheck, filesToCheck[i], ref pairs);
                //Обновляем инфу в ивенте
                UpdateEventInfo(info);
            });
            //Возвращаем только уникальные пары дублей
            return pairs.Distinct(new FilePairComparer()).ToList();
        }

    }
}
