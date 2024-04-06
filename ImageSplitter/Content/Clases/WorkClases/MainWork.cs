using DuplicateScanWindowLib;
using FilesRenameWindowLib;
using FilesSplitWindowLib;
using FolderCreateWindowLib;
using ImageSplitter.Content.Windows;
using ImageSplitterLib;
using ImageSplitterLib.Clases.DataClases;
using SelectFoldersWindowLib;
using SplitImagesWindowLib;
using SplitPathWindowLib;
using SplitterDataLib.DataClases.Global.DuplicateScan;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TreeViewWindowLib;

namespace ImageSplitter.Content.Clases.WorkClases
{
    /// <summary>
    /// Основной рабочий класс
    /// </summary>
    internal class MainWork : IDisposable
    {
        /// <summary>
        /// Класс выбора путей
        /// </summary>
        private SplitPathFasade _splitPathFasade;
        /// <summary>
        /// Класс отображения древа
        /// </summary>
        private TreeViewFasade _treeViewFasade;
        /// <summary>
        /// Класс отображения окна создания папки
        /// </summary>
        private FolderCreateFasade _folderCreateFasade;
        /// <summary>
        /// Фасадный класс библиотеки выбора папок для сплита
        /// </summary>
        private SelectFoldersFasade _selectFoldersFasade;
        /// <summary>
        /// Фасадный класс библиотеки сплита файлов
        /// </summary>
        private FilesSplitFasade _filesSplitFasade;
        /// <summary>
        /// Класс переименованяи файлов
        /// </summary>
        private FilesRenameFasade _filesRenameFasade;
        /// <summary>
        /// Фасадный класс библиотеки сканирвоания на дубликаты
        /// </summary>
        private DuplicateScanFasade _duplicateScanFasade;
        /// <summary>
        /// Фасадный класс библиотеки сплита изображений
        /// </summary>
        private SplitImagesFasade _splitImagesFasade;



        /// <summary>
        /// Путь для сплита
        /// </summary>
        private SplitPathsInfo _path;



        /// <summary>
        /// Конструктор класса
        /// </summary>
        public MainWork()
        {
            Init();
        }


        #region Initialization

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            InitVariables();
            InitClases();
            InitEvents();
        }

        /// <summary>
        /// Инициализируем значения переменных
        /// </summary>
        private void InitVariables()
        {
            //Инициализируем дефолтные значения
            _path = new SplitPathsInfo();
        }

        /// <summary>
        /// Инициализируем используемые классы
        /// </summary>
        private void InitClases()
        {
            //Инициализируем класс сплита изображений
            _splitImagesFasade = new SplitImagesFasade();
            //Инициализируем класс выбора путей
            _splitPathFasade = new SplitPathFasade();
            //Инициализируем класс отображения древа
            _treeViewFasade = new TreeViewFasade();
            //Инициализируем класс сплита файлов
            _filesSplitFasade = new FilesSplitFasade();
            //Инициализируем класс переименования
            _filesRenameFasade = new FilesRenameFasade();
            //Инициализируем класс сканирования дубликатов
            _duplicateScanFasade = new DuplicateScanFasade();
            //Инициализируем класс выбора папок для сплита
            _selectFoldersFasade = new SelectFoldersFasade();
            //Инициализируем класс выбора имени папки
            _folderCreateFasade = new FolderCreateFasade();
        }


        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void InitEvents()
        {
            //Добавляем обработчик события запроса отображения окна создания новой папки
            SplitImagesFasade.AddNewFolderRequest += SplitImagesFasade_AddNewFolderRequest;
            //Добавляем обработчик события запроса отображения окна обновления путей сплита
            SplitImagesFasade.UpdateSplitPathRequest += SplitImagesFasade_UpdateSplitPathRequest;
            //Добавляем обработчик события запроса отображения окна древа
            SplitImagesFasade.ShowTreeRequest += SplitImagesFasade_ShowTreeRequest;
            //Добавляем обработчик события запроса запуска сканирования
            SplitImagesFasade.StartScanRequest += SplitImagesFasade_StartScanRequest;
            //Добавляем обработчик события запроса сплита файлов
            SplitImagesFasade.StartFileSplitRequest += SplitImagesFasade_StartFileSplitRequest;
            //Добавляем обработчик события запроса переименования файлов
            SplitImagesFasade.StartFileRenameRequest += SplitImagesFasade_StartFileRenameRequest;
            //Добавляем обработчик события запроса поиска дубликатов
            SplitImagesFasade.ScanDuplicatesRequest += SplitImagesFasade_ScanDuplicatesRequest;
            //Добавляем обработчик события запроса завершения поиска коллекций
            SplitImagesFasade.ScanCollectionsComplete += SplitImagesFasade_ScanCollectionsComplete;
        }


        #endregion



        #region SplitEvents


        /// <summary>
        /// Обработчик события запроса завершения поиска коллекций
        /// </summary>
        private void SplitImagesFasade_ScanCollectionsComplete()
        {
            //Выполняем поиск папок
            List<TargetFolderInfo> folders = _splitImagesFasade.ScanFolders(_path);
            //Получаем список выбранных папок
            folders = _selectFoldersFasade.SelectFolders(folders);
            //Вызываем ментод передачи выбранных папок
            _splitImagesFasade.CompleteSelectFolders(folders);
        }


        /// <summary>
        /// Обработчик события запроса поиска дубликатов
        /// </summary>
        private void SplitImagesFasade_ScanDuplicatesRequest() =>
            //Выполняем отображение поиска дубликатов
            _duplicateScanFasade.ShowDuplicatesWindow();

        /// <summary>
        /// Обработчик события запроса переименования файлов
        /// </summary>
        private void SplitImagesFasade_StartFileRenameRequest() =>
            //Выполняем запуск переименования файлов
            _filesRenameFasade.RenameFiles(_path);

        /// <summary>
        /// Обработчик события запроса сплита файлов
        /// </summary>
        private void SplitImagesFasade_StartFileSplitRequest() =>
            //Выполняем запуск сплита файлов
            _filesSplitFasade.SplitFiles(_path);

        /// <summary>
        /// Обработчик события запроса запуска сканирования
        /// </summary>
        private void SplitImagesFasade_StartScanRequest() =>
            //Выполняем запуск старта сканирования файлов
            _splitImagesFasade.StartScanCollections(_path);

        /// <summary>
        /// Обработчик события запроса отображения окна древа
        /// </summary>
        private void SplitImagesFasade_ShowTreeRequest() =>
            //Отображаем древо по текущему выбранному пути
            _treeViewFasade.ShowTree(_path.MovePath);

        /// <summary>
        /// Обработчик события запроса отображения окна обновления путей сплита
        /// </summary>
        private void SplitImagesFasade_UpdateSplitPathRequest()
        {
            //Получаем путь сплита из окна выбора пути
            _path = _splitPathFasade.UpdateSplitPath();
            //Обновляем путь сплита в основном окне
            _splitImagesFasade.UpdateSplitPath(_path);


            //TODO: Тут ещё в поиске дубликатов нужно будет путь показывать!
        }

        /// <summary>
        /// Обработчик события запроса отображения окна создания новой папки
        /// </summary>
        private void SplitImagesFasade_AddNewFolderRequest()
        {
            //Получаем имя новой папки
            string name = _folderCreateFasade.GetFolderName();
            //Если имя папки корректно
            if (!string.IsNullOrEmpty(name))
                //Вызываем метод добавления папки
                _splitImagesFasade.AddNewFolder(_path, name);
        }



        #endregion

        /// <summary>
        /// Метод отображения основного окна приложения
        /// </summary>
        public void ShowMainWindow() =>
            //Отображаем окно сплита
            _splitImagesFasade.ShowSplitWindow();

        /// <summary>
        /// Метод очистки неуправляемых ресурсов класса
        /// </summary>
        public void Dispose()
        {
            //Завершаем работу с классом сплита изображений
            _splitImagesFasade?.Dispose();
            //Завершаем работу с классом поиска дубликатов
            _duplicateScanFasade?.Dispose();
        }
    }
}
