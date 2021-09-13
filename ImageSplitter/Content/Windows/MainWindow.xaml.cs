using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Duplicates;
using ImageSplitter.Content.Clases.DataClases.Global;
using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases;
using ImageSplitter.Content.Clases.WorkClases.Addition;
using ImageSplitter.Content.Clases.WorkClases.KeyProcessor;
using ImageSplitter.Content.Clases.WorkClases.KeyProcessor.Processors;
using System;
using System.Collections.Generic;
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

namespace ImageSplitter.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Основной рабочий класс
        /// </summary>
        private MainWork _mainWork;
        /// <summary>
        /// Класс обработки нажатий клавишь
        /// </summary>
        private KeyActionProcessor _keyActionProcessor;

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            InitVariables();
            InitEvents();
        }

        /// <summary>
        /// Инициализируем значения переменных
        /// </summary>
        private void InitVariables()
        {
            //Инициализируем основной рабочий класс
            _mainWork = new MainWork();
            //Инициализируем класс обработки нажатий клавишь
            _keyActionProcessor = new KeyActionProcessor(_mainWork);
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void InitEvents()
        {
            InitCurrentEvents();
            InitGlobalEvents();
            InitControlsEvents();
        }

        /// <summary>
        /// Инициализируем ивенты для текущего окна
        /// </summary>
        private void InitCurrentEvents()
        {
            //Добавляем обработчик событяи закрытия окна
            this.Closed += MainWindow_Closed;
            //Добавляем обработчик глобального события нажатия кнопки клавиатуры, с фокусом на любом элементе окна
            this.AddHandler(UIElement.PreviewKeyDownEvent, new RoutedEventHandler(MainWindow_KeyDown));
        }

        /// <summary>
        /// Инициализируем глобальные ивенты
        /// </summary>
        private void InitGlobalEvents()
        {
            //Добавляем обработчик событяи обновления основной информации на контролле сплита изображений
            GlobalEvents.UpdateImageSplitInfoRequest += GlobalEvents_UpdateImageSplitInfoRequest;
            //Добавляем обработчик событяи завершения сканирвоания
            GlobalEvents.ScanComplete += GlobalEvents_ScanComplete;
            //Добавляем обработчик событяи завершения переноса изображения
            GlobalEvents.MoveImageComplete += GlobalEvents_MoveImageComplete;
            //Добавляем обработчик событяи завершения сканирвоания дубликатов
            GlobalEvents.DuplicateScanComplete += GlobalEvents_DuplicateScanComplete;
            //Добавляем обработчик событяи обновления статуса сканирования дубликатов
            GlobalEvents.DuplicateScanProgress += GlobalEvents_DuplicateScanProgress;
            //Добавляем обработчик событяи завершения удаления дубликатов
            GlobalEvents.RemoveDuplicatesComplete += GlobalEvents_RemoveDuplicatesComplete;
            //Добавляем обработчик событяи завершения сканирования дубликатов, при котором не было найдено дублей
            GlobalEvents.DuplicateScanNotFound += GlobalEvents_DuplicateScanNotFound;
            //Добавляем обработчик события запроса на переход к коллекции
            CollectionsSplitTab.MoveToCollectionRequest += CollectionsSplitTab_MoveToCollectionRequest;
            //Добавляем обработчик события запроса на переход к изображению в коллекции
            CollectionsSplitTab.MoveToImageRequest += CollectionsSplitTab_MoveToImageRequest;
            //Добавляем обработчик события запроса на переход к указанной вкладке
            MainKeys.SendToTabRequest += MainKeys_SendToTabRequest;
        }

        /// <summary>
        /// Инициализируем ивенты от контроллов
        /// </summary>
        private void InitControlsEvents()
        {
            //Добавляем обработчик событяи запуска сплита
            SplitImages.StartSplitScan += SplitImages_StartSplitScan;
            //Добавляем обработчик события запроса на переход к коллекции
            SplitImages.MoveToCollectionRequest += SplitImages_MoveToCollectionRequest;
            //Добавляем обработчик события запроса на добавление новой папки
            SplitImages.AddNewFolderRequest += SplitImages_AddNewFolderRequest;
            //Добавляем обработчик события запроса на удаление папки из списка
            SplitImages.RemoveFolderRequest += SplitImages_RemoveFolderRequest;
            //Добавляем обработчик события запуска отмены сплита
            FileSplitParams.StartBack += FileSplitParams_StartBack;
            //Добавляем обработчик события запуска сплита файлов
            FileSplitParams.StartFileSplit += FileSplitParams_StartFileSplit;
            //Добавляем обработчик события переименования файлов
            RenameFiles.RenameFiles += RenameFiles_RenameFiles;
            //Добавляем обработчик события запуска сканирования на дубликаты
            ImageDuplicates.StartDuplicateScan += ImageDuplicates_StartDuplicateScan;
            //Добавляем обработчик события запуска удаления дубликатов
            ImageDuplicates.DuplicateRemove += ImageDuplicates_DuplicateRemove;
        }


        /// <summary>
        /// Обработчик события запроса на переход к указанной вкладке
        /// </summary>
        /// <param name="tabId">Id вкладки для перехода</param>
        private void MainKeys_SendToTabRequest(int tabId) =>
            //Выбираем нужную вкладку
            MainTabControl.SelectedIndex = tabId;

        /// <summary>
        /// Обработчик события запроса на переход к коллекции
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private void CollectionsSplitTab_MoveToCollectionRequest(int direction) =>
            //ВЫзываем внутренний метод перехода
            MoveToCollection(direction);

        /// <summary>
        /// Обработчик события запроса на переход к изображению в коллекции
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private void CollectionsSplitTab_MoveToImageRequest(int direction) =>
            //ВЫполняем переход к картинке в коллекции
            SplitImages.MoveFolderImage(direction);

        /// <summary>
        /// Jбработчик событяи завершения сканирования 
        /// дубликатов, при котором не было найдено дублей
        /// </summary>
        private void GlobalEvents_DuplicateScanNotFound() =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Возвращаем доступность окна
                this.IsEnabled = true;
                //ВЫводим сообщение о том что не было найдено дубликатов
                MessageBox.Show("Duplicates not found!");
            });

        /// <summary>
        /// Обработчик событяи завершения удаления дубликатов
        /// </summary>
        private void GlobalEvents_RemoveDuplicatesComplete() =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Очищаем панель дубликатов
                ImageDuplicates.ClearOldPanels();
                //ВЫводим сообщение о завершении удаления дубликатов
                MessageBox.Show("Duplicate remove complete!");
            });

        /// <summary>
        /// Обработчик события запуска удаления дубликатов
        /// </summary>
        /// <param name="duplicates">Список дубликатов для удаления</param>
        private void ImageDuplicates_DuplicateRemove(List<DuplicateImageInfo> duplicates) =>
            //Вызываем внутренний метод
            _mainWork.RemoveDuplicates(duplicates);

        /// <summary>
        /// Обработчик события запуска сканирования на дубликаты
        /// </summary>
        /// <param name="path">Путь для сканирования</param>
        private void ImageDuplicates_StartDuplicateScan(string path)
        {
            //Выключаем доступность окна
            this.IsEnabled = false;
            //Вызываем внутренний метод
            _mainWork.StartDuplicateScan(path);
        }

        /// <summary>
        /// Обработчик события запроса на удаление папки из списка
        /// </summary>
        /// <param name="key">Клавиша, к которой привязана папка</param>
        /// <param name="folderName">Имя папки</param>
        private void SplitImages_RemoveFolderRequest(Key key, string folderName) =>
            //Вызываем внутренний метод
            _mainWork.RemoveFolderFromList(key);

        /// <summary>
        /// Обработчик события запроса на добавление новой папки
        /// </summary>
        private void SplitImages_AddNewFolderRequest() =>
            //Вызываем внутренний метод
            _mainWork.AddNewFolder();

        /// <summary>
        /// Обработчик события запроса на переход к коллекции
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private void SplitImages_MoveToCollectionRequest(int direction) =>
            //ВЫзываем внутренний метод перехода
            MoveToCollection(direction);

        /// <summary>
        /// Обработчик событяи запуска сплита
        /// </summary>
        /// <param name="scanPath">Путь сканирования</param>
        /// <param name="splitPath">Путь сплита</param>
        /// <param name="isFolder">Флаг сканирования папок</param>
        private void SplitImages_StartSplitScan(string scanPath, string splitPath, bool isFolder)
        {
            //Выключаем доступность окна
            this.IsEnabled = false;
            //Запускаем сканирование
            _mainWork.StartScan(scanPath, splitPath, isFolder);
        }


        /// <summary>
        /// Обработчик событяи обновления статуса сканирования дубликатов
        /// </summary>
        /// <param name="current">Текущее значение</param>
        /// <param name="max">Максимальное значение</param>
        private void GlobalEvents_DuplicateScanProgress(int current, int max) =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Проставляем прогресс сканирования
                ImageDuplicates.SetScanProgress(current, max);
            });

        /// <summary>
        /// Обработчик событяи завершения сканирвоания дубликатов
        /// </summary>
        /// <param name="duplicates">Список дубликатов для отображения</param>
        private void GlobalEvents_DuplicateScanComplete(List<DuplicateImageInfo> duplicates) =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Проставляем дубликаты в контролл
                ImageDuplicates.SetImages(duplicates);
                //Включаем доступность окна
                this.IsEnabled = true;
                //ВЫводим сообщение о завершении работы
                MessageBox.Show("Duplicate scan complete!");
            });

        /// <summary>
        /// Обработчик событяи завершения переноса изображения
        /// </summary>
        private void GlobalEvents_MoveImageComplete() =>
            //Переходим к следующей картинке
            MoveToCollection(1);

        /// <summary>
        /// Обработчик событяи завершения сканирвоания
        /// </summary>
        private void GlobalEvents_ScanComplete() =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Включаем доступность окна
                this.IsEnabled = true;
                //Отображаем первую в списке картинку
                MoveToCollection(0);
                //Вызываем месседжбокс
                MessageBox.Show("Scan complete!");
            });

        /// <summary>
        /// Обработчик событяи обновления основной информации на контролле сплита изображений
        /// </summary>
        /// <param name="pagesInfo">Инфомрация о текущих отображаемых страницах</param>
        /// <param name="folders">Список доступных папок</param>
        private void GlobalEvents_UpdateImageSplitInfoRequest(string pagesInfo, List<TargetFolderInfo> folders) =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Обновляем общую инфорамцию в контролле
                SplitImages.UpdateMainInfo(pagesInfo, folders);
            });


        /// <summary>
        /// Обработчик события переименования файлов
        /// </summary>
        /// <param name="mask">Маска имени для переименования</param>
        /// <param name="path">Путь для переименования</param>
        private void RenameFiles_RenameFiles(string path, string mask) =>
            //Запускаем переименование
            _mainWork.RenameFiles(path, mask);

        /// <summary>
        /// Обработчик события запуска сплита файлов
        /// </summary>
        /// <param name="countFiles">Количество файлов для сплита</param>
        /// <param name="path">Путь для сплита</param>
        /// <param name="isChildSplit">Флаг сплита в дочерних папках</param>
        private void FileSplitParams_StartFileSplit(string path, int countFiles, bool isChildSplit) =>  
            //Запускаем сплит
            _mainWork.StartSplit(path, countFiles, isChildSplit);

        /// <summary>
        /// Обработчик события запуска отмены сплита
        /// </summary>
        /// <param name="path">Путь для отмены сплита</param>
        private void FileSplitParams_StartBack(string path) =>
            //Выполняем возврат файлов
            _mainWork.BackToParent(path);


        /// <summary>
        /// Обработчик событяи закрытия окна
        /// </summary>
        private void MainWindow_Closed(object sender, EventArgs e) =>
            //Завершаем работу основного рабочего класса
            _mainWork?.Dispose();

        /// <summary>
        /// Обработчик событяи нажатия на кнопку
        /// </summary>
        private void MainWindow_KeyDown(object sender, RoutedEventArgs e) =>
            //Вызываем обработку нажатия на клавишу
            _keyActionProcessor.ProcessKeyPress((KeyEventArgs)e, MainTabControl.SelectedIndex);


        /// <summary>
        /// Переходим к картинке
        /// </summary>
        /// <param name="direction">Направление движения</param>
        private void MoveToCollection(int direction)
        {
            //Получаем целевую картинку
            CollectionInfo image = _mainWork.MoveToImage(direction);
            //Грузим её на форму
            SplitImages.LoadImageInfo(image);
        }



    }
}
