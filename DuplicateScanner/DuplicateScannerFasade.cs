using DuplicateScanner.Clases.DataClases.File;
using DuplicateScanner.Clases.DataClases.Properties;
using DuplicateScanner.Clases.DataClases.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DuplicateScanner.Clases.DataClases.Global.Delegates;

namespace DuplicateScanner
{
    /// <summary>
    /// Фасадный класс библиотеки поиска дубликатов
    /// </summary>
    public class DuplicateScannerFasade : IDisposable
    {
        /// <summary>
        /// Событие обновления информации о прогрессе сканирования
        /// </summary>
        public static event UpdateScanInfoEventHandler UpdateScanInfo;
        /// <summary>
        /// Событие завершения сканирования
        /// </summary>
        public static event CompleteScanEventHandler CompleteScan;

        /// <summary>
        /// Событие обновления информации о прогрессе удаления выбранных файлов
        /// </summary>
        public static event UpdateProgressInfoEventHandler UpdateRemoveInfo;
        /// <summary>
        /// Событие завершения удаления выбранных файлов
        /// </summary>
        public static event CompleteProgressEventHandler CompleteRemove;

        /// <summary>
        /// Класс поиска дубликатов
        /// </summary>
        private Clases.WorkClases.DuplicateScanner _scanner;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicateScannerFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _scanner = new Clases.WorkClases.DuplicateScanner();
        }


        /// <summary>
        /// Метод вызова ивента обновления информации о прогрессе сканирования
        /// </summary>
        /// <param name="info">Информация о прогрессе сканирования</param>
        internal static void InvokeUpdateScanInfo(ScanProgressInfo info) =>
            //Вызываем внешний ивент
            UpdateScanInfo?.Invoke(info);

        /// <summary>
        /// Метод вызова ивента завершения сканирования
        /// </summary>
        /// <param name="result">Список результатов поиска дубликатов</param>
        internal static void InvokeCompleteScan(List<FindResult> result) =>
            //Вызываем внешний ивент
            CompleteScan?.Invoke(result);

        /// <summary>
        /// Метод вызова ивента обновления информации о прогрессе удаления выбранных файлов
        /// </summary>
        /// <param name="info">Информация о прогрессе удаления выбранных файлов</param>
        internal static void InvokeUpdateRemoveInfo(ProgressInfo info) =>
            //Вызываем внешний ивент
            UpdateRemoveInfo?.Invoke(info);

        /// <summary>
        /// Метод вызова ивента завершения удаления выбранных файлов
        /// </summary>
        internal static void InvokeCompleteRemove() =>
            //Вызываем внешний ивент
            CompleteRemove?.Invoke();

        /// <summary>
        /// Запуск сканирования дубликатов
        /// </summary>
        /// <param name="properties">Параметры сканирования</param>
        public void StartDuplicateScan(ScanProperties properties) =>
            //Вызываем дочерний метод в отдельном потоке
            new Thread(() => _scanner.StartDuplicateScan(properties)).Start();


        /// <summary>
        /// Метод запуска удаления дублиткатов
        /// </summary>
        /// <param name="groups">Список запрещённых групп</param>
        /// <param name="toRemove">Группа хешей для удаления</param>
        public void RemoveDuplicates(HashesGroup toRemove, List<HashesGroup> groups) =>
            //Вызываем дочерний метод в отдельном потоке
            new Thread(() => _scanner.RemoveDuplicates(toRemove, groups)).Start();


        /// <summary>
        /// Метод удаления старых дубликатов из списка
        /// </summary>
        public void RemoveOldDuplicates() =>
            //Вызываем дочерний метод в отдельном потоке
            new Thread(() => _scanner.RemoveOldDuplicates()).Start();


        /// <summary>
        /// Метод очистки неуправляемых ресурсов класса
        /// </summary>
        public void Dispose()
        {
            //Завершаем работу с классом сканирования
            _scanner?.Dispose();
        }
    }
}
