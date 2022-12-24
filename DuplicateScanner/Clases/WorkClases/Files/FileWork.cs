using DuplicateScanner.Clases.DataClases;
using DuplicateScanner.Clases.DataClases.File;
using DuplicateScanner.Clases.DataClases.Result;
using DuplicateScanner.Clases.WorkClases.Hash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DuplicateScanner.Clases.DataClases.Global.Enums;

namespace DuplicateScanner.Clases.WorkClases.Files
{
    /// <summary>
    /// Класс работы с файлами
    /// </summary>
    internal class FileWork : IDisposable
    {

        /// <summary>
        /// Текущий список дубликатов хранящихся в системе
        /// </summary>
        private List<DuplicateInfo> _currentDuplicates;


        /// <summary>
        /// Класс поиска файлов
        /// </summary>
        private FileScanner _fileScanner;
        /// <summary>
        /// Класс загрузки информации о сохранённых дубликатах
        /// </summary>
        private DuplicateInfoLoader _duplicateInfoLoader;
        /// <summary>
        /// Класс рассчёта хешей
        /// </summary>
        private HashCalculator _hashCalculator;



        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileWork()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _duplicateInfoLoader = new DuplicateInfoLoader();
            _fileScanner = new FileScanner();
            _hashCalculator = new HashCalculator();
            //Грузим дубликаты из файла
            _currentDuplicates = _duplicateInfoLoader.LoadDuplicates();
        }

        /// <summary>
        /// Получаем список дубликатов из группы хешей
        /// </summary>
        /// <param name="group">Группа хешей для получения дубликатов</param>
        /// <returns>Список дубликатов из группы</returns>
        private List<DuplicateInfo> GetDuplicatesFromGroup(HashesGroup group) =>
            //Из общего списка дубликатов
            _currentDuplicates
                //Выбираем те, которые имеют схожие хеши из списка
                .Where(dup => group.HashList.Contains(dup.PathHash))
                //Возвращаем в виде списка
                .ToList();

        /// <summary>
        /// Получаем список хешей, которые можно добавить в запрещённые
        /// </summary>
        /// <param name="current">Текущий элемент, которому будем добавлять хеши</param>
        /// <param name="duplicates">Список дубликатов для добавления хешей</param>
        /// <returns>Список хешей для добавленяи в список</returns>
        private List<uint> GetHashesToAdd(DuplicateInfo current, List<DuplicateInfo> duplicates) =>
            //Из списка дубликатов
            duplicates
                //Получаем те, которые можно добавить в запрещённые
                .Where(dup => current.IsAllowAddToForbidden(dup.PathHash))
                //От них берём хеши
                .Select(dup => dup.PathHash)
                //Возвращаем в виде списка
                .ToList();

        /// <summary>
        /// Метод инициализации информации о прогрессе
        /// </summary>
        /// <param name="loadedFilesCount">Количество загруженных файлов</param>
        /// <param name="newFilesCount">Количество новых файлов</param>
        /// <returns>Класс информации о прогрессе хеширования</returns>
        private ScanProgressInfo CreateProgressInfo(int loadedFilesCount, int newFilesCount) =>
             new ScanProgressInfo(ScanStages.HashGeneration)
             {
                 FilesFinded = loadedFilesCount,
                 FilesToProcess = newFilesCount,
                 LoadedFiles = loadedFilesCount - newFilesCount
             };


        /// <summary>
        /// Метод удаления файла
        /// </summary>
        /// <param name="hash">Хеш файла</param>
        private void RemoveFile(uint hash)
        {
            //Получаем файл из списка по хешу
            DuplicateInfo info = _currentDuplicates
                .FirstOrDefault(file => file.PathHash == hash);
            //Если файл найден
            if (info != null)
            {
                //Если файл существует
                if (File.Exists(info.Path))
                    //Удаляем его
                    File.Delete(info.Path);
                //Удаляем файл из списка
                _currentDuplicates.Remove(info);
                //Удаляем этот хеш из всех списков запрещённых
                _currentDuplicates.ForEach(file => file.RemoveForbiddenHash(hash));
            }
        }


        /// <summary>
        /// Метод рассчёта хешей для новых файлов
        /// </summary>
        /// <param name="newFiles">Список новых файлов для обработки</param>
        /// <param name="loadedFilesCount">Количество загруженных файлов</param>
        private void UpdateHashes(List<DuplicateInfo> newFiles, int loadedFilesCount)
        {
            //Время запуска итерации
            DateTime start;
            //Инициализируем класс информации о прогрессе
            ScanProgressInfo info = CreateProgressInfo(loadedFilesCount, newFiles.Count);
            //Вызываем ивент обновления прогресса
            DuplicateScannerFasade.InvokeUpdateScanInfo(info);
            //Вычисляем хеши для каждого из новых файлов
            for(int i = 0; i < newFiles.Count; i++) 
            {
                //Получаем время запуска получения хеша
                start = DateTime.Now;
                //Выполняем рассчёт хешей для файла
                _hashCalculator.CalculateHash(newFiles[i]);
                //Обновляем счётчик файлов с ошибкой
                info.ErrorFilesCount = newFiles.Count(file => file.IsErrorFile);
                //Обновляем счётчик обработанных файлов
                info.ProcessedFiles = i;
                //Получаем время итерации для ивента
                info.IterationTime = (DateTime.Now - start).TotalSeconds;
                //Вызываем ивент обновления прогресса
                DuplicateScannerFasade.InvokeUpdateScanInfo(info);
            }
        }


        /// <summary>
        /// Метод простановки запрещённых хешей в группе
        /// </summary>
        /// <param name="group">Группа для простановки</param>
        private void SetForbiddenHashes(HashesGroup group)
        {
            //Если в группе больше одного элемента
            if (group.HashList.Count > 1)
            {
                //Получаем список дубликатов по хешам группы
                List<DuplicateInfo> duplicates = GetDuplicatesFromGroup(group);
                //Проходимся по списку выбранных дубликатов
                foreach (DuplicateInfo current in duplicates)
                    //Получаем список хешей, которые можно добавить текущему, и добавляем их
                    current.ForbiddenHashes.AddRange(GetHashesToAdd(current, duplicates));
            }
        }


        /// <summary>
        /// Выполняем поиск новых файлов
        /// </summary>
        /// <param name="loadedFiles">Список найденных в папке файлов</param>
        /// <returns>Список новых файлов</returns>
        private List<DuplicateInfo> FindNewFiles(ref List<DuplicateInfo> loadedFiles)
        {
            //Инициализируем списки для файлов
            List<DuplicateInfo> newFiles = new List<DuplicateInfo>();
            List<DuplicateInfo> oldFiles = new List<DuplicateInfo>();
            //Буфер для найденного файла
            DuplicateInfo buff;
            //Проходимся по загруженным файлам
            foreach (DuplicateInfo file in loadedFiles)
            {
                //Ищем загруженный файл в сохранённых
                buff = _currentDuplicates.FirstOrDefault(curr => curr.Equals(file));
                //Если файла в сохранённых нет
                if (buff == null)
                    //Добавляем его в список новых
                    newFiles.Add(file);
                //Если файл есть в сохранённых
                else
                    //Добавляем его в список старых
                    oldFiles.Add(buff);
            }
            //Очищаем список загруженных файлов
            loadedFiles.Clear();
            //И добавляем в него новые и старые файлы
            loadedFiles.AddRange(newFiles);
            loadedFiles.AddRange(oldFiles);
            //Возвращаем отдельно список новых файлов
            return newFiles;
        }
        
        
        /// <summary>
        /// Выполняем поиск файлов в переданной папке
        /// </summary>
        /// <param name="path">Путь к папке для сканирования</param>
        /// <returns>Список найденных дубликатов</returns>
        public List<DuplicateInfo> ScanFiles(string path)
        {
            //Выполняем поиск дочерних файлов
            List<DuplicateInfo> loadedFiles = _fileScanner.ScanFiles(path);
            //Выполняем поиск новых файлов
            List<DuplicateInfo> newFiles = FindNewFiles(ref loadedFiles);
            //Выполняем генерацию хешей для новых файлов
            UpdateHashes(newFiles, loadedFiles.Count);
            //Ивент о запуске сохранения
            DuplicateScannerFasade.InvokeUpdateScanInfo(
                new ScanProgressInfo(ScanStages.SavingData));
            //Выполняем сохранение новых файлов в общий список
            SaveFiles(newFiles);
            //Возвращаем список найденных файлов
            return loadedFiles;
        }

        /// <summary>
        /// Метод запуска удаления дублиткатов
        /// </summary>
        /// <param name="toRemove">Группа хешей для удаления</param>
        public void RemoveDuplicates(HashesGroup toRemove)
        {
            //Инициализируем класс информации о прогрессе удаления
            ProgressInfo info = new ProgressInfo() { 
                MaxCount = toRemove.HashList.Count };
            //Проходимся по списку выбранных хешей
            for (int i = 0; i < toRemove.HashList.Count; i++)
            {
                //Удаляем из всех мест соответствующие им файлы
                RemoveFile(toRemove.HashList[i]);
                //Обновляем количество удалённых файлов
                info.Processed = i;
                //Вызываем ивент обновления удаления
                DuplicateScannerFasade.InvokeUpdateRemoveInfo(info);
            }
        }

        /// <summary>
        /// Метод простановки запрещённых в группах хешей
        /// </summary>
        /// <param name="groups">Список групп</param>
        public void SetForbiddenGroups(List<HashesGroup> groups)
        {
            //Проходимся по списку групп
            foreach(HashesGroup group in groups)
                //Проставляем запрещённые хеши группе
                SetForbiddenHashes(group);
        }


        /// <summary>
        /// Выполняем сохранение файлов в общий список
        /// </summary>
        /// <param name="newFiles">Новые файлы, которые ещё не были обработаны</param>
        public void SaveFiles(List<DuplicateInfo> newFiles = null)
        {
            //Если есть новые файлы
            if(newFiles != null)
                //Добавляем униклаьные файлы в общий список 
                _currentDuplicates.AddRange(newFiles);
            //Сохраняем обновлённый список дубликатов
            _duplicateInfoLoader.SaveDuplicates(_currentDuplicates);
        }

        /// <summary>
        /// Метод очистки неуправляемых ресурсов класса
        /// </summary>
        public void Dispose()
        {
            //Выполняем сохранение файлов
            SaveFiles();
            //Завершаем работу с классом вычисления хешей
            _hashCalculator?.Dispose();
        }
    }
}
