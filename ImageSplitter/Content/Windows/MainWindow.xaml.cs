using DuplicateScanner;
using DuplicateScanner.Clases.DataClases.File;
using DuplicateScanner.Clases.DataClases.Properties;
using DuplicateScanner.Clases.DataClases.Result;
using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Global;
using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.DataClases.Tags;
using ImageSplitter.Content.Clases.WorkClases;
using ImageSplitter.Content.Clases.WorkClases.Addition;
using ImageSplitter.Content.Clases.WorkClases.KeyProcessor;
using ImageSplitter.Content.Clases.WorkClases.KeyProcessor.Processors;
using ImageSplitter.Content.Windows.Tags;
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
            //Добавляем обработчик событяи запуска сканирования
            GlobalEvents.StartDuplicateScan += GlobalEvents_StartDuplicateScan;
            //Добавляем обработчик события запроса на удаление старых элементов
            GlobalEvents.RemoveOldRequest += GlobalEvents_RemoveOldRequest;

            //Добавляем обработчик события обновления статуса сканирования на дубликаты
            DuplicateScannerFasade.UpdateScanInfo += DuplicateScannerFasade_UpdateScanInfo;
            //Добавляем обработчик события завершения сканирования на дубликаты
            DuplicateScannerFasade.CompleteScan += DuplicateScannerFasade_CompleteScan;
            //Добавляем обработчик события обновления статуса удаления выбранных дубликатов
            DuplicateScannerFasade.UpdateRemoveInfo += DuplicateScannerFasade_UpdateRemoveInfo;
            //Добавляем обработчик события завершения удаления выбранных дубликатов
            DuplicateScannerFasade.CompleteRemove += DuplicateScannerFasade_CompleteRemove;
            //Добавляем обработчик события завершения удаления устаревших дубликатов
            DuplicateScannerFasade.CompleteRemoveOldDuplicates += DuplicateScannerFasade_CompleteRemoveOldDuplicates;

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
            //Добавляем обработчик события запроса отображения древа
            SplitImages.ShowTreeRequest += SplitImages_ShowTreeRequest;
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
            //Добавляем обработчик события запуска удаления дубликатов
            ImageDuplicates.DuplicateRemove += ImageDuplicates_DuplicateRemove;
        }

        /// <summary>
        /// Обработчик события запроса отображения древа
        /// </summary>
        /// <param name="path">Путь для отображения древа</param>
        private void SplitImages_ShowTreeRequest(string path)
        {
            //Инициализируем окно отображения
            TreeVisualizerWindow treeVisualizerWindow = new TreeVisualizerWindow();
            //Отображаем окно
            treeVisualizerWindow.Show();
            //Отображаем древо
            treeVisualizerWindow.VisualizeTree(path);
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
        private async void CollectionsSplitTab_MoveToCollectionRequest(int direction) =>
            //ВЫзываем внутренний метод перехода
            await MoveToCollection(direction);

        /// <summary>
        /// Обработчик события запроса на переход к изображению в коллекции
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private async void CollectionsSplitTab_MoveToImageRequest(int direction) =>
            //ВЫполняем переход к картинке в коллекции
            await SplitImages.MoveFolderImage(direction);




        /// <summary>
        /// Обработчик события обновления статуса удаления выбранных дубликатов
        /// </summary>
        /// <param name="info">ИНформация о статусе удаления дубликатов</param>
        private void DuplicateScannerFasade_UpdateRemoveInfo(ProgressInfo info)
        {
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Передаём информацию о прогрессе в контролл
                ImageDuplicates.UpdateRemoveInfo(info);
            });
        }

        /// <summary>
        /// Обработчик события обновления статуса сканирования на дубликаты
        /// </summary>
        /// <param name="info">ИНформация о статусе сканирования</param>
        private void DuplicateScannerFasade_UpdateScanInfo(ScanProgressInfo info)
        {
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Передаём информацию о прогрессе в контролл
                ImageDuplicates.UpdateScanInfo(info);
            });
        }

        /// <summary>
        /// Обработчик события завершения удаления выбранных дубликатов
        /// </summary>
        private void DuplicateScannerFasade_CompleteRemove()
        {
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Выводим сообщение о результате
                MessageBox.Show("Удаление дубликатов было успешно завершено");
                //Скрываем панель прогресса
                ImageDuplicates.SetProgressPanelVisiblity(false);
                //Возвращаем доступность окна
                this.IsEnabled = true;
            });
        }


        /// <summary>
        /// Обработчик события завершения удаления устаревших записей о дубликатах
        /// </summary>
        private void DuplicateScannerFasade_CompleteRemoveOldDuplicates(int count)
        {
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Выводим сообщение о результате
                MessageBox.Show($"Удаление устаревших записей о дубликатах" +
                    $" было успешно завершено! Удалено {count} записей.");
                //Скрываем панель прогресса
                ImageDuplicates.SetProgressPanelVisiblity(false);
                //Возвращаем доступность окна
                this.IsEnabled = true;
            });
        }


        /// <summary>
        /// Обработчик события завершения сканирования на дубликаты
        /// </summary>
        /// <param name="result">Результат сканирования на дубликаты</param>
        private void DuplicateScannerFasade_CompleteScan(List<DuplicatePair> result)
        {
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Скрываем панель прогресса
                ImageDuplicates.SetProgressPanelVisiblity(false);
                //Если дубликаты не были найдены
                if (result.Count == 0)
                {
                    //Удаляем старые панели дубликатов, чтобы не было путанницы
                    ImageDuplicates.ClearOldPanels();
                    //Выводим сообщение о результате
                    MessageBox.Show("Дубликаты не были найдены в указанной папке");
                }
                //Если результаты есть
                else
                    //Втыкаем результаты поиска в контролл
                    ImageDuplicates.SetImages(result);
                //Возвращаем доступность окна
                this.IsEnabled = true;
            });
        }




        /// <summary>
        /// Обработчик события запуска удаления дубликатов
        /// </summary>
        /// <param name="groups">Список запрещённых групп</param>
        /// <param name="toRemove">Группа хешей для удаления</param>
        private void ImageDuplicates_DuplicateRemove(HashesGroup toRemove, List<HashesGroup> groups)
        {
            //Выключаем доступность окна
            this.IsEnabled = false;
            //Отображаем панель прогресса
            ImageDuplicates.SetProgressPanelVisiblity(true);
            //Очищаем панель дубликатов
            ImageDuplicates.ClearOldPanels();
            //Вызываем внутренний метод
            _mainWork.RemoveDuplicates(toRemove, groups);
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
        private async void SplitImages_MoveToCollectionRequest(int direction) =>
            //ВЫзываем внутренний метод перехода
            await MoveToCollection(direction);

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
        /// Обработчик событяи запуска сканирования
        /// </summary>
        /// <param name="properties">Параметры сканирования</param>
        private void GlobalEvents_StartDuplicateScan(ScanProperties properties)
        {
            //Выключаем доступность окна
            this.IsEnabled = false;
            //Отображаем панель прогресса
            ImageDuplicates.SetProgressPanelVisiblity(true);
            //Вызываем внутренний метод
            _mainWork.StartDuplicateScan(properties);
        }

        /// <summary>
        /// Обработчик событяи завершения переноса изображения
        /// </summary>
        private async void GlobalEvents_MoveImageComplete() =>
            //Переходим к следующей картинке
            await MoveToCollection(1);

        /// <summary>
        /// Обработчик событяи завершения сканирвоания
        /// </summary>
        private async void GlobalEvents_ScanComplete() =>
            //Вызываем в UI-потоке
            await this.Dispatcher.InvokeAsync(async () => {
                //Включаем доступность окна
                this.IsEnabled = true;
                //Отображаем первую в списке картинку
                await MoveToCollection(0);
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
        /// Обработчик события запроса на удаление старых элементов
        /// </summary>
        private void GlobalEvents_RemoveOldRequest()
        {
            //Выключаем доступность окна
            this.IsEnabled = false;
            //Отображаем панель прогресса
            ImageDuplicates.SetProgressPanelVisiblity(true);
            //Вызываем внутренний метод
            _mainWork.RemoveOldDuplicates();
        }


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
        private async Task MoveToCollection(int direction)
        {
            //Получаем целевую картинку
            CollectionInfo image = _mainWork.MoveToImage(direction);
            //Грузим её на форму
            await SplitImages.LoadImageInfo(image);
        }



    }
}
