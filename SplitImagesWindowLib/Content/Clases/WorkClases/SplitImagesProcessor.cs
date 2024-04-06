using ImageSplitter.Content.Windows;
using ImageSplitterLib;
using ImageSplitterLib.Clases.DataClases;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SplitImagesWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс обработки сплита изображений
    /// </summary>
    internal class SplitImagesProcessor : IDisposable
    {

        /// <summary>
        /// Окно сплита изображений
        /// </summary>
        private ImagesSplitWindow _imagesSplitWindow;
        /// <summary>
        /// Фаскдный класс библиотеки сплита изображений
        /// </summary>
        private ImageSplitterFasade _imageSplitterFasade;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SplitImagesProcessor()
        {
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
            //Инициализируем фасадный класс библиотеки сплита изображений
            _imageSplitterFasade = new ImageSplitterFasade();
            //Инициализируем основное окно
            _imagesSplitWindow = new ImagesSplitWindow();
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void InitEvents()
        {
            InitMainWindowEvents();
            InitClasesEvents();
        }


        /// <summary>
        /// Инициализируем обработчики событий от основного окна
        /// </summary>
        private void InitMainWindowEvents()
        {
            //События работы с окнами
            //Добавляем обработчик события запроса отображения окна древа
            _imagesSplitWindow.ShowTreeRequest += _imagesSplitWindow_ShowTreeRequest;
            //Добавляем обработчик события запроса отображения окна обновления путей сплита
            _imagesSplitWindow.UpdateSplitPathRequest += _imagesSplitWindow_UpdateSplitPathRequest;
            //Добавляем обработчик события запроса отображения окна создания новой папки
            _imagesSplitWindow.AddNewFolderRequest += _imagesSplitWindow_AddNewFolderRequest;
            //Добавляем обработчик события запроса запуска сканирования
            _imagesSplitWindow.StartScanRequest += _imagesSplitWindow_StartScanRequest;
            //Добавляем обработчик события запроса сплита файлов
            _imagesSplitWindow.StartFileSplitRequest += _imagesSplitWindow_StartFileSplitRequest;
            //Добавляем обработчик события запроса переименования файлов
            _imagesSplitWindow.StartFileRenameRequest += _imagesSplitWindow_StartFileRenameRequest;
            //Добавляем обработчик события запроса поиска дубликатов
            _imagesSplitWindow.ScanDuplicatesRequest += _imagesSplitWindow_ScanDuplicatesRequest;

            //События сплита
            //Добавляем обработчик события запроса перехода к изображению в коллекции
            _imagesSplitWindow.MoveToImageRequest += _imagesSplitWindow_MoveToImageRequest;
            //Добавляем обработчик события запроса перехода к коллекции
            _imagesSplitWindow.MoveToCollectionRequest += _imagesSplitWindow_MoveToCollectionRequest;
            //Добавляем обработчик события перемещения картинки по пути
            _imagesSplitWindow.MoveImageRequest += _imagesSplitWindow_MoveImageRequest;
            //Добавляем обработчик события запроса отмены переноса
            _imagesSplitWindow.UndoMoveRequest += _imagesSplitWindow_UndoMoveRequest;
            //Добавляем обработчик события запроса удаления папки
            _imagesSplitWindow.RemoveFolderRequest += _imagesSplitWindow_RemoveFolderRequest;
        }


        /// <summary>
        /// Инициализируем обработчики событий от классов
        /// </summary>
        private void InitClasesEvents()
        {
            //События из библиотеки сплиттера
            //Добавляем обработчик события завершения перемещения изображения
            ImageSplitterFasade.MoveImageComplete += ImageSplitterFasade_MoveImageComplete;
            //Добавляем обработчик события запроса на обновление инфы о сплите картинок
            ImageSplitterFasade.UpdateImageSplitInfoRequest += ImageSplitterFasade_UpdateImageSplitInfoRequest;
            //Добавляем обработчик события завершения поиска коллекций
            ImageSplitterFasade.ScanCollectionsComplete += ImageSplitterFasade_ScanCollectionsComplete;
        }

        #region GlobalEvents

        /// <summary>
        /// Обработчик события запроса отображения окна древа
        /// </summary>
        private void _imagesSplitWindow_ShowTreeRequest() =>
            //Вызываем глобальный ивент
            SplitImagesFasade.InvokeShowTreeRequest();

        /// <summary>
        /// Обработчик события запроса отображения окна обновления путей сплита
        /// </summary>
        private void _imagesSplitWindow_UpdateSplitPathRequest() =>
            //Вызываем глобальный ивент
            SplitImagesFasade.InvokeUpdateSplitPathRequest();

        /// <summary>
        /// Обработчик события запроса отображения окна создания новой папки
        /// </summary>
        private void _imagesSplitWindow_AddNewFolderRequest() =>
            //Вызываем глобальный ивент
            SplitImagesFasade.InvokeAddNewFolderRequest();

        /// <summary>
        /// Обработчик события запроса запуска сканирования
        /// </summary>
        private void _imagesSplitWindow_StartScanRequest() =>
            //Вызываем глобальный ивент
            SplitImagesFasade.InvokeStartScanRequest();

        /// <summary>
        /// Обработчик события запроса сплита файлов
        /// </summary>
        private void _imagesSplitWindow_StartFileSplitRequest() =>
            //Вызываем глобальный ивент
            SplitImagesFasade.InvokeStartFileSplitRequest();

        /// <summary>
        /// Обработчик события запроса переименования файлов
        /// </summary>
        private void _imagesSplitWindow_StartFileRenameRequest() =>
            //Вызываем глобальный ивент
            SplitImagesFasade.InvokeStartFileRenameRequest();


        /// <summary>
        /// Обработчик события запроса поиска дубликатов
        /// </summary>
        private void _imagesSplitWindow_ScanDuplicatesRequest() =>
            //Вызываем глобальный ивент
            SplitImagesFasade.InvokeScanDuplicatesRequest();

        /// <summary>
        /// Обработчик события завершения поиска коллекций
        /// </summary>
        private async void ImageSplitterFasade_ScanCollectionsComplete()
        {
            //Отображаем текущую выбранную картинку
            await ShowCurrentCollection();
            //Вызываем глобальный ивент
            SplitImagesFasade.InvokeScanCollectionsComplete();
        }

        #endregion


        #region SplitImages

        /// <summary>
        /// Обработчик события запроса отмены переноса
        /// </summary>
        private async void _imagesSplitWindow_UndoMoveRequest()
        {
            //Откатываем перенос изображения
            _imageSplitterFasade.UndoMove();
            //Отображаем текущую выбранную картинку
            await ShowCurrentCollection();
        }

        /// <summary>
        /// Обработчик события запроса удаления папки
        /// </summary>
        /// <param name="key">Клавиша, к которой привязана папка</param>
        /// <param name="folderName">Имя папки</param>
        private void _imagesSplitWindow_RemoveFolderRequest(Key key, string folderName) =>
            //Вызываем удаление папки
            _imageSplitterFasade.RemoveFolderFromList(key);

        /// <summary>
        /// Обработчик события запроса перехода к коллекции
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private async void _imagesSplitWindow_MoveToCollectionRequest(int direction) =>
            //Выполняем переход к картинке
            await MoveToCollection(direction);

        /// <summary>
        /// Обработчик события запроса перехода к изображению в коллекции
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private async void _imagesSplitWindow_MoveToImageRequest(int direction)
        {
            //Получаем целевую картинку
            _imageSplitterFasade.MoveToImage(direction);
            //Отображаем текущую выбранную картинку
            await ShowCurrentCollection();
        }

        /// <summary>
        /// Обработчик события перемещения картинки по пути
        /// </summary>
        /// <param name="key">Клавиша для обработки</param>
        private void _imagesSplitWindow_MoveImageRequest(Key key) =>
            //Вызываем внутренний метод
            _imageSplitterFasade.CheckImageMoveTarget(key);

        /// <summary>
        /// Обработчик события завершения перемещения изображения
        /// </summary>
        private async void ImageSplitterFasade_MoveImageComplete() =>
            //Выполняем переход к следующей картинке
            await MoveToCollection(1);


        /// <summary>
        /// Обработчик события запроса на обновление инфы о сплите картинок
        /// </summary>
        /// <param name="pagesInfo">Инфомрация о текущих отображаемых страницах</param>
        /// <param name="folders">Список доступных папок</param>
        private void ImageSplitterFasade_UpdateImageSplitInfoRequest(string pagesInfo, List<TargetFolderInfo> folders) =>
            //Вызываем обновление информации в основном окне
            _imagesSplitWindow.UpdateMainInfo(pagesInfo, folders);


        #endregion


        /// <summary>
        /// Метод отображения текущей коллекции
        /// </summary>
        private async Task ShowCurrentCollection() =>
            //Обновляем картинку в контролле по текущей коллекции
            await _imagesSplitWindow.LoadImageInfo(_imageSplitterFasade.CurrentImageInfo);

        /// <summary>
        /// Метод отображения текущей коллекции
        /// </summary>
        /// <param name="direction">Направление перемщения</param>
        private async Task MoveToCollection(int direction)
        {
            //Получаем целевую картинку
            CollectionInfo image = _imageSplitterFasade.MoveToCollection(direction);
            //Грузим её на форму
            await _imagesSplitWindow.LoadImageInfo(image);
        }





        /// <summary>
        /// Метод завершения выбора папок
        /// </summary>
        /// <param name="folders">Список папок для простановки</param>
        public void CompleteSelectFolders(List<TargetFolderInfo> folders)
        {
            //Вызываем передаём выбранные папки в класс обработки
            _imageSplitterFasade.CompleteSelectFolders(folders);
            //Включаем окно по завершению поиска
            _imagesSplitWindow.SetEnabledStatus(true);
            //TODO: ВЫЗЫВАТЬ НОРМАЛЬНОЕ ОКНО!
            //Вызываем месседжбокс
            MessageBox.Show("Scan complete!");
        }

        /// <summary>
        /// Запускаем поиск коллекций
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public void StartScanCollections(SplitPathsInfo info)
        {
            //Блокируем окно на время выполнения действия
            _imagesSplitWindow.SetEnabledStatus(false);
            //Вызываем метод запуска поиска коллекций
            _imageSplitterFasade.StartScanCollections(info);
        }


        /// <summary>
        /// Выполняем поиск папок
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public List<TargetFolderInfo> ScanFolders(SplitPathsInfo info) =>
            //ВЫполняем запуск поиска папок
            _imageSplitterFasade.ScanFolders(info);


        /// <summary>
        /// Метод обновления пути сплита
        /// </summary>
        /// <param name="info">Информация о новом пути для сплита</param>
        public void UpdateSplitPath(SplitPathsInfo info) =>
            //Передаём вызов в окно
            _imagesSplitWindow.UpdateSplitPath(info);


        /// <summary>
        /// Метод добавления новой папки
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        /// <param name="newFolderName">Имя создаваемой папки</param>
        public void AddNewFolder(SplitPathsInfo info, string newFolderName) =>
            //Вызываем внутренний метод
            _imageSplitterFasade.AddNewFolder(info, newFolderName);

        /// <summary>
        /// Метод отображения окна дубликатов
        /// </summary>
        public void ShowSplitWindow() =>
            //Вызываем отображение окна
            _imagesSplitWindow.Show();

        /// <summary>
        /// Обработчик события завершения работы с окном
        /// </summary>
        public void Dispose()
        {
            //Если окно дубликатов существует
            if (_imagesSplitWindow != null)
            {
                //Разрешаем закрыть окно дубликатов
                _imagesSplitWindow.IsAllowClose = true;
                //Закрываем его
                _imagesSplitWindow.Close();
            }
        }
    }
}
