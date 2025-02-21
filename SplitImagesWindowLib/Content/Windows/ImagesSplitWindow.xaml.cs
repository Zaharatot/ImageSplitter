using ImageSplitterLib.Clases.DataClases;
using MessagesWindowLib;
using SplitterDataLib.DataClases.Global.Split;
using SplitterSimpleUI.Content.Clases.DataClases.HotKey;
using SplitterSimpleUI.Content.Clases.WorkClases.HotKey;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SplitImagesWindowLib.Content.Clases.DataClases.Delegates;
using static SplitterDataLib.DataClases.Global.Delegates;
using static SplitterSimpleUI.Content.Clases.DataClases.Global.Delegates;
using static SplitterSimpleUI.Content.Clases.DataClases.Global.Enums;

namespace ImageSplitter.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для ImagesSplitWindow.xaml
    /// </summary>
    public partial class ImagesSplitWindow : Window
    {
        #region Events

        /// <summary>
        /// Событие запроса на создание новой папки
        /// </summary>
        public event EmptyEventHandler AddNewFolderRequest;
        /// <summary>
        /// Событие запроса на обновление пути сплита
        /// </summary>
        public event EmptyEventHandler UpdateSplitPathRequest;
        /// <summary>
        /// Событие запроса на отображение древа
        /// </summary>
        public event EmptyEventHandler ShowTreeRequest;
        /// <summary>
        /// Событие запроса на запуск сканирования
        /// </summary>
        public event EmptyEventHandler StartScanRequest;
        /// <summary>
        /// Событие запроса на отмену переноса коллекции
        /// </summary>
        public event EmptyEventHandler UndoMoveRequest;
        /// <summary>
        /// Событие запроса сплита файлов
        /// </summary>
        public event EmptyEventHandler StartFileSplitRequest;
        /// <summary>
        /// Событие запроса переименования файлов
        /// </summary>
        public event EmptyEventHandler StartFileRenameRequest;
        /// <summary>
        /// Событие запроса поиска дубликатов
        /// </summary>
        public event EmptyEventHandler ScanDuplicatesRequest;
        /// <summary>
        /// Событие запроса завершения рабоыт приложения
        /// </summary>
        public event EmptyEventHandler CloseApplicationRequest;


        /// <summary>
        /// Событие запроса на удаление папки
        /// </summary>
        public event RemoveFolderRequestEventHandler RemoveFolderRequest;

        /// <summary>
        /// Событие запроса на перенос изображения по клавише
        /// </summary>
        public event KeyPressEventHandler MoveImageRequest;

        /// <summary>
        /// Событие запроса на переход к изображению
        /// </summary>
        public event MoveToImageEventHandler MoveToCollectionRequest;
        /// <summary>
        /// Событие запроса на переход к изображению
        /// </summary>
        public event MoveToImageEventHandler MoveToImageRequest;

        #endregion


        /// <summary>
        /// Класс обработки хоткеев
        /// </summary>
        private HotKeyProcessor _hotKeyProcessor;
        /// <summary>
        /// Класс работы со всплывающими сообщениями
        /// </summary>
        private PopupMessagesFasade _popupMessagesFasade;

        /// <summary>
        /// Словарь событий основного меню
        /// </summary>
        private Dictionary<MainMenuElements, EmptyEventHandler> _mainMenuEventsDict;


        /// <summary>
        /// Конструктор окна
        /// </summary>
        public ImagesSplitWindow()
        {
            InitializeComponent();
            Init();
        }

        #region Initialization

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            InitVariables();
            InitHotkeys();
            InitMessages();
            InitEvents();
        }


        /// <summary>
        /// Инициализируем значения переменных
        /// </summary>
        private void InitVariables()
        {
            //Инициализируем словарь событий
            _mainMenuEventsDict = CreateEventsDict();
        }

        /// <summary>
        /// Инициализируем хоткеи
        /// </summary>
        private void InitHotkeys()
        {
            //Получаем экземпляр класса обработки хоткеев
            _hotKeyProcessor = new HotKeyProcessor();
            //Добавляем хоткеи для текущего окна
            AddHotKeys();
        }

        /// <summary>
        /// Метод добавления хоткеев для окна
        /// </summary>
        private void AddHotKeys() =>
            _hotKeyProcessor.AddWindow(this, new WindowHotKeys(false,
                //В случае нажатия другой клавиши - будет вызвано
                //событие обработки переноса изображеняи по кнопке
                (e) => { MoveImageRequest?.Invoke(e); }) {
                //Добавляем список хоткеев
                HotKeys = new List<HotKeyInfo>() { 
                    //Хоткеи для вызова функций приложения
                    //При нажатии на "Ctrl + N" - выполняем вызов окна создания новой папки
                    new HotKeyInfo(Key.N, () => { AddNewFolderRequest?.Invoke(); }, true),
                    //При нажатии на "Ctrl + O" - выполняем вызов окна обновления путей сплита
                    new HotKeyInfo(Key.O, () => { UpdateSplitPathRequest?.Invoke(); }, true),
                    //При нажатии на "Ctrl + T" - выполняем вызов окна древа
                    new HotKeyInfo(Key.T, () => { ShowTreeRequest?.Invoke(); }, true),
                    //При нажатии на "Ctrl + F" - выполняем запуск сканирования
                    new HotKeyInfo(Key.F, () => { StartScanRequest?.Invoke(); }, true),
                    //При нажатии на "Ctrl + Z" - выполняем отмену переноса
                    new HotKeyInfo(Key.Z, () => { UndoMoveRequest?.Invoke(); }, true),
                    //При нажатии на "Ctrl + S" - выполняем запуск сплита файлов
                    new HotKeyInfo(Key.S, () => { StartFileSplitRequest?.Invoke(); }, true),
                    //При нажатии на "Ctrl + R" - выполняем запуск переименования файлов
                    new HotKeyInfo(Key.R, () => { StartFileRenameRequest?.Invoke(); }, true),
                    //При нажатии на "Ctrl + D" - выполняем запуск поиска дубликатов
                    new HotKeyInfo(Key.D, () => { ScanDuplicatesRequest?.Invoke(); }, true),


                    //Хоткеи для работы с коллекциями
                    //При нажатии на "Left" - идём к предыдущей картинке
                    new HotKeyInfo(Key.Left, () => { MoveToCollectionRequest?.Invoke(-1); }),
                    //При нажатии на "Right" - идём к следующей картинке
                    new HotKeyInfo(Key.Right, () => { MoveToCollectionRequest?.Invoke(1); }),
                    //При нажатии на "Up" - идём к предыдущей картинке в коллекции
                    new HotKeyInfo(Key.Up, () => { MoveToImageRequest?.Invoke(-1); }),
                    //При нажатии на "Down" - идём к следующей картинке в коллекции
                    new HotKeyInfo(Key.Down, () => { MoveToImageRequest?.Invoke(1); }),
                }
            });

        /// <summary>
        /// Инициализируем сообщения
        /// </summary>
        private void InitMessages()
        {
            //Инициализируем класс работы с сообщениями
            _popupMessagesFasade = new PopupMessagesFasade();
            //Добавляем контролл сообщения на панель
            _popupMessagesFasade.AddPopupControl(MainPanel);
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void InitEvents()
        {
            //Добавляем обработчик события выбора элемента в основном меню
            MainMenuPanel.MainMenuSelectItem += MainMenuPanel_MainMenuSelectItem;

            //Добавляем обработчик события запроса на добавление новой папки
            SplitImages.AddNewFolderRequest += SplitImages_AddNewFolderRequest;
            //Добавляем обработчик события запроса на удаление папки из списка
            SplitImages.RemoveFolderRequest += SplitImages_RemoveFolderRequest;
            //Добавляем обработчик события запроса отмены переноса
            SplitImages.UndoMove += SplitImages_UndoMove;

            //Добавляем обработчик события запроса на переход к коллекции
            SplitImages.MoveToCollectionRequest += SplitImages_MoveToCollectionRequest;
            //Добавляем обработчик события запроса на переход к изображению в коллекции
            SplitImages.MoveToImageRequest += SplitImages_MoveToImageRequest;
        }

        /// <summary>
        /// Создаём словарь событий главного меню
        /// </summary>
        /// <returns>Созданный словарь</returns>
        private Dictionary<MainMenuElements, EmptyEventHandler> CreateEventsDict() =>
            new Dictionary<MainMenuElements, EmptyEventHandler>() {
                { MainMenuElements.DuplicateScan, () => { ScanDuplicatesRequest?.Invoke(); } },
                { MainMenuElements.FilesSplit, () => { StartFileSplitRequest?.Invoke(); } },
                { MainMenuElements.RenameFiles, () => {  StartFileRenameRequest?.Invoke(); } },
                { MainMenuElements.SelectPath, () => {  UpdateSplitPathRequest?.Invoke(); } },
                { MainMenuElements.SplitScanImages, () => {  StartScanRequest?.Invoke(); } },
                { MainMenuElements.TreeView, () => {  ShowTreeRequest?.Invoke(); } },
            };





        #endregion



        #region MainActions



        /// <summary>
        /// Обработчик события выбора элемента в основном меню
        /// </summary>
        /// <param name="element">Выбранный элемент</param>
        private void MainMenuPanel_MainMenuSelectItem(MainMenuElements element)
        {
            //Если такое событие есть в словаре
            if (_mainMenuEventsDict.ContainsKey(element))
                //Вызываем его
                _mainMenuEventsDict[element]?.Invoke();
        }

        #endregion


        #region SplitActions


        /// <summary>
        /// Обработчик события запроса отмены переноса
        /// </summary>
        private void SplitImages_UndoMove() =>
            //Вызываем внешний ивент
            UndoMoveRequest?.Invoke();

        /// <summary>
        /// Обработчик события запроса на удаление папки из списка
        /// </summary>
        /// <param name="key">Клавиша, к которой привязана папка</param>
        /// <param name="folderName">Имя папки</param>
        private void SplitImages_RemoveFolderRequest(Key key, string folderName) =>
            //Вызываем внешний ивент
            RemoveFolderRequest?.Invoke(key, folderName);

        /// <summary>
        /// Обработчик события запроса на добавление новой папки
        /// </summary>
        private void SplitImages_AddNewFolderRequest() =>
            //Вызываем внешний ивент
            AddNewFolderRequest?.Invoke();

        /// <summary>
        /// Обработчик события запроса на переход к изображению в коллекции
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private void SplitImages_MoveToImageRequest(int direction) =>
            //Вызываем внешний ивент
            MoveToImageRequest?.Invoke(direction);

        /// <summary>
        /// Обработчик события запроса на переход к коллекции
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private void SplitImages_MoveToCollectionRequest(int direction) =>
            //Вызываем внешний ивент
            MoveToCollectionRequest?.Invoke(direction);

        #endregion










        /// <summary>
        /// Обработчик осбытия обработки закрытия окна
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e) =>
            //Вызываем запрос закрытия приложения
            CloseApplicationRequest?.Invoke();

        /// <summary>
        /// Обработчик событяи закрытия окна
        /// </summary>
        private void Window_Closed(object sender, EventArgs e) =>
            //Удаляем обюработку хоткеев для данного окна
            _hotKeyProcessor.RemoveWindow(this);







        /// <summary>
        /// Обновляем общую инфорамцию на форме
        /// </summary>
        /// <param name="pagesInfo">Инфомрация о текущих отображаемых страницах</param>
        /// <param name="folders">Список доступных папок</param>
        public void UpdateMainInfo(string pagesInfo, List<TargetFolderInfo> folders) =>
            //Вызываем в UI потоке
            this.Dispatcher.Invoke(() => { 
                //Передаём вызов в целевой контролл
                SplitImages.UpdateMainInfo(pagesInfo, folders);
            });

        /// <summary>
        /// Метод обновления пути сплита
        /// </summary>
        /// <param name="info">Информация о новом пути для сплита</param>
        public void UpdateSplitPath(SplitPathsInfo info)
        {
            //Передаём выбранные пути в контролл сплита
            SplitImages.SetSplitPathInfo(info);
            //Если передан флаг старта сплита и пути для него есть
            if (info.IsStartSplit && info.IsContainPaths)
                //Вызываем запуск сканирования
                StartScanRequest?.Invoke();
        }


        /// <summary>
        /// Метод безопасного обновления статуса окна
        /// </summary>
        /// <param name="isEnabled">Флаг активности окна</param>
        public void SetEnabledStatus(bool isEnabled) =>
            //Вызываем в UI-потоке
            this.Dispatcher.InvokeAsync(() => {
                //Включаем доступность окна
                this.IsEnabled = isEnabled;
            });

        /// <summary>
        /// Грузим на панель инфу о картинке
        /// </summary>
        /// <param name="info">Класс инфы о картинке</param>
        internal async Task LoadImageInfo(CollectionInfo info) =>
            //Вызываем в UI-потоке
            await this.Dispatcher.InvokeAsync(async () => {
                //Передаём вызов в целевой контролл
                await SplitImages.LoadImageInfo(info);
            });
    }
}
