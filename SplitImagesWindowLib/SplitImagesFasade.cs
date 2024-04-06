using SplitImagesWindowLib.Content.Clases.WorkClases;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SplitterDataLib.DataClases.Global.Delegates;

namespace SplitImagesWindowLib
{
    /// <summary>
    /// Фасадный класс сплита изображений
    /// </summary>
    public class SplitImagesFasade : IDisposable
    {
        /// <summary>
        /// Событие запроса на создание новой папки
        /// </summary>
        public static event EmptyEventHandler AddNewFolderRequest;
        /// <summary>
        /// Событие запроса на обновление пути сплита
        /// </summary>
        public static event EmptyEventHandler UpdateSplitPathRequest;
        /// <summary>
        /// Событие запроса на отображение древа
        /// </summary>
        public static event EmptyEventHandler ShowTreeRequest;
        /// <summary>
        /// Событие запроса на запуск сканирования
        /// </summary>
        public static event EmptyEventHandler StartScanRequest;
        /// <summary>
        /// Событие запроса сплита файлов
        /// </summary>
        public static event EmptyEventHandler StartFileSplitRequest;
        /// <summary>
        /// Событие запроса переименования файлов
        /// </summary>
        public static event EmptyEventHandler StartFileRenameRequest;
        /// <summary>
        /// Событие запроса поиска дубликатов
        /// </summary>
        public static event EmptyEventHandler ScanDuplicatesRequest;
        /// <summary>
        /// Событие завершения поиска коллекций
        /// </summary>
        public static event EmptyEventHandler ScanCollectionsComplete;

        /// <summary>
        /// Событие запроса завершения рабоыт приложения
        /// </summary>
        public static event EmptyEventHandler CloseApplicationRequest;

        /// <summary>
        /// Класс обработки сплита изображений
        /// </summary>
        private SplitImagesProcessor _splitImagesProcessor;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SplitImagesFasade()
        {
            Init();
        }

        private void Init()
        {
            //Инициализируем класс сплита изображений
            _splitImagesProcessor = new SplitImagesProcessor();
        }

        /// <summary>
        /// Метод вызова ивента запроса завершения рабоыт приложения
        /// </summary>
        internal static void InvokeCloseApplicationRequest() =>
            //Вызываем статический ивент
            CloseApplicationRequest?.Invoke();

        /// <summary>
        /// Метод вызова ивента запроса на поиск дубликатов
        /// </summary>
        internal static void InvokeScanDuplicatesRequest() =>
            //Вызываем статический ивент
            ScanDuplicatesRequest?.Invoke();

        /// <summary>
        /// Метод вызова ивента запроса на переименование файлов
        /// </summary>
        internal static void InvokeStartFileRenameRequest() =>
            //Вызываем статический ивент
            StartFileRenameRequest?.Invoke();

        /// <summary>
        /// Метод вызова ивента запроса на сплит файлов
        /// </summary>
        internal static void InvokeStartFileSplitRequest() =>
            //Вызываем статический ивент
            StartFileSplitRequest?.Invoke();

        /// <summary>
        /// Метод вызова ивента запроса на запуск сканирования
        /// </summary>
        internal static void InvokeStartScanRequest() =>
            //Вызываем статический ивент
            StartScanRequest?.Invoke();

        /// <summary>
        /// Метод вызова ивента запроса на отображение древа
        /// </summary>
        internal static void InvokeShowTreeRequest() =>
            //Вызываем статический ивент
            ShowTreeRequest?.Invoke();

        /// <summary>
        /// Метод вызова ивента запроса на обновление пути сплита
        /// </summary>
        internal static void InvokeUpdateSplitPathRequest() =>
            //Вызываем статический ивент
            UpdateSplitPathRequest?.Invoke();

        /// <summary>
        /// Метод вызова ивента запроса на создание новой папки
        /// </summary>
        internal static void InvokeAddNewFolderRequest() =>
            //Вызываем статический ивент
            AddNewFolderRequest?.Invoke();

        /// <summary>
        /// Метод вызова события завершения поиска коллекций
        /// </summary>
        internal static void InvokeScanCollectionsComplete() =>
            //Вызываем статический ивент
            ScanCollectionsComplete?.Invoke();




        /// <summary>
        /// Метод завершения выбора папок
        /// </summary>
        /// <param name="folders">Список папок для простановки</param>
        public void CompleteSelectFolders(List<TargetFolderInfo> folders) =>
            //Вызываем внутренний метод
            _splitImagesProcessor.CompleteSelectFolders(folders);

        /// <summary>
        /// Запускаем сканирование
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public void StartScanCollections(SplitPathsInfo info) =>
            //Вызываем внутренний метод
            _splitImagesProcessor.StartScanCollections(info);

        /// <summary>
        /// Выполняем поиск папок
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public List<TargetFolderInfo> ScanFolders(SplitPathsInfo info) =>
            //Вызываем внутренний метод
            _splitImagesProcessor.ScanFolders(info);





        /// <summary>
        /// Метод обновления пути сплита
        /// </summary>
        /// <param name="info">Информация о новом пути для сплита</param>
        public void UpdateSplitPath(SplitPathsInfo info) =>
            //Вызываем внутренний метод
            _splitImagesProcessor.UpdateSplitPath(info);

        /// <summary>
        /// Метод добавления новой папки
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        /// <param name="newFolderName">Имя создаваемой папки</param>
        public void AddNewFolder(SplitPathsInfo info, string newFolderName) =>
            //Вызываем внутренний метод
            _splitImagesProcessor.AddNewFolder(info, newFolderName);


        /// <summary>
        /// Метод отображения окна сплита изображений
        /// </summary>
        public void ShowSplitWindow() =>
            //Вызываем отображение окна
            _splitImagesProcessor.ShowSplitWindow();


        /// <summary>
        /// Обработчик события завершения работы с окном
        /// </summary>
        public void Dispose() =>
            //Вызываем завершение работы основного класса
            _splitImagesProcessor?.Dispose();

    }
}
