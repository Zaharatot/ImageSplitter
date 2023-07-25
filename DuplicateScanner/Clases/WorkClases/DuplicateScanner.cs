using DuplicateScanner.Clases.DataClases;
using DuplicateScanner.Clases.DataClases.File;
using DuplicateScanner.Clases.DataClases.Properties;
using DuplicateScanner.Clases.DataClases.Result;
using DuplicateScanner.Clases.WorkClases.Files;
using DuplicateScanner.Clases.WorkClases.Finder;
using DuplicateScanner.Clases.WorkClases.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DuplicateScanner.Clases.DataClases.Global.Enums;

namespace DuplicateScanner.Clases.WorkClases
{
    /// <summary>
    /// Класс выполнения поиска дубликатов
    /// </summary>
    internal class DuplicateScanner : IDisposable
    {
        /// <summary>
        /// Класс работы с файлами
        /// </summary>
        private FileWork _fileWork;
        /// <summary>
        /// Класс поиска дубликатов
        /// </summary>
        private DuplicatesFind _duplicatesFind;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicateScanner()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _fileWork = new FileWork();
            _duplicatesFind = new DuplicatesFind();
        }




        /// <summary>
        /// Запуск сканирования дубликатов
        /// </summary>
        /// <param name="properties">Параметры сканирования</param>
        public void StartDuplicateScan(ScanProperties properties)
        {
            //Ивент о запуске сканирования
            DuplicateScannerFasade.InvokeUpdateScanInfo(
                new ScanProgressInfo(ScanStages.FindFiles));
            //Выполняем поиск файлов в переданной папке
            List<DuplicateInfo> filesToCheck = _fileWork.ScanFiles(properties.ScanPath);
            //Выполняем поиск дубликатов
            List<DuplicatePair> result = _duplicatesFind.Find(filesToCheck, properties);
            //Вызываем ивент завершения сканирования, с указанием результатов
            DuplicateScannerFasade.InvokeCompleteScan(result);
        }


        /// <summary>
        /// Метод запуска удаления дублиткатов
        /// </summary>
        /// <param name="groups">Список запрещённых групп</param>
        /// <param name="toRemove">Группа хешей для удаления</param>
        public void RemoveDuplicates(HashesGroup toRemove, List<HashesGroup> groups)
        {
            //Выполняем простановку запрещённых
            _fileWork.SetForbiddenGroups(groups);
            //Выполняем удаление дубликатов
            _fileWork.RemoveDuplicates(toRemove);
            //Выполняем сохранение изменений списка
            _fileWork.SaveFiles();
            //Вызываем ивент завершения процесса удаления
            DuplicateScannerFasade.InvokeCompleteRemove();
        }

        /// <summary>
        /// Метод удаления старых дубликатов из списка
        /// </summary>
        public void RemoveOldDuplicates()
        {
            //Удаляем старые дубликаты
            _fileWork.RemoveOldDuplicates();
            //Выполняем сохранение изменений списка
            _fileWork.SaveFiles();
        }

        /// <summary>
        /// Метод очистки неуправляемых ресурсов класса
        /// </summary>
        public void Dispose()
        {
            //Завершаем работу с классом работы с файлами            
            _fileWork.Dispose();
        }
    }
}
