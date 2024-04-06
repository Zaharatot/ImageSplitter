using ImageSplitterLib.Clases.DataClases;
using ImageSplitterLib.Clases.WorkClases;
using ImageSplitterLib.Clases.WorkClases.Targets;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static SplitterDataLib.DataClases.Global.Delegates;

namespace ImageSplitterLib
{
    /// <summary>
    /// Фасадный класс библиотеки сплита изображений
    /// </summary>
    public class ImageSplitterFasade
    {
        /// <summary>
        /// Событие запроса на обновление инфы о сплите картинок
        /// </summary>
        public static event UpdateImageSplitInfoEventHandler UpdateImageSplitInfoRequest;
        /// <summary>
        /// Событие завершения переноса изображения
        /// </summary>
        public static event EmptyEventHandler MoveImageComplete;
        /// <summary>
        /// Событие завершения поиска коллекций
        /// </summary>
        public static event EmptyEventHandler ScanCollectionsComplete;


        /// <summary>
        /// Текущая выбранная картинка
        /// </summary>
        public CollectionInfo CurrentImageInfo => _splitImages.CurrentImageInfo;
        /// <summary>
        /// Класс выполнения сплита
        /// </summary>
        private SplitImages _splitImages;
        /// <summary>
        /// Класс обработки целей
        /// </summary>
        private TargetsProcessor _targetsProcessor;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImageSplitterFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _splitImages = new SplitImages();
            //Получаем экземпляр класса обработки целей
            _targetsProcessor = TargetsProcessor.GetInstance();
        }

        /// <summary>
        /// Метод вызова ивента обновления основной информации на контролле сплита изображений
        /// </summary>
        /// <param name="pagesInfo">Инфомрация о текущих отображаемых страницах</param>
        /// <param name="folders">Список доступных папок</param>
        internal static void InvokeUpdateImageSplitInfoRequest(string pagesInfo,
            List<TargetFolderInfo> folders) =>
            //Вызываем статический ивент
            UpdateImageSplitInfoRequest?.Invoke(pagesInfo, folders);

        /// <summary>
        /// Метод вызова ивента завершения переноса изображения
        /// </summary>
        internal static void InvokeMoveImageComplete() =>
            //Вызываем статический ивент
            MoveImageComplete?.Invoke();

        /// <summary>
        /// Метод вызова события завершения поиска коллекций
        /// </summary>
        internal static void InvokeScanCollectionsComplete() =>
            //Вызываем статический ивент
            ScanCollectionsComplete?.Invoke();







        /// <summary>
        /// Запускаем поиск коллекций
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public void StartScanCollections(SplitPathsInfo info) =>
            //Вызываем внутренний метод
            _splitImages.StartScanCollections(info);

        /// <summary>
        /// Выполняем поиск папок
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public List<TargetFolderInfo> ScanFolders(SplitPathsInfo info) =>
            //Вызываем внутренний метод
            _splitImages.ScanFolders(info);

        /// <summary>
        /// Метод завершения выбора папок
        /// </summary>
        /// <param name="folders">Список папок для простановки</param>
        public void CompleteSelectFolders(List<TargetFolderInfo> folders) =>
            //Вызываем внутренний метод
            _splitImages.CompleteSelectFolders(folders);



        /// <summary>
        /// Проверяем нажатую кнопку на тип кнопки переноса
        /// </summary>
        /// <param name="key">Код нажатой кнопки</param>
        /// <returns>True - нажатие было обработано</returns>
        public bool CheckImageMoveTarget(Key key) =>
            //Вызываем внутренний метод
            _splitImages.CheckImageMoveTarget(key);

        /// <summary>
        /// Откатываем перемещение коллекции
        /// </summary>
        public void UndoMove() =>
            //Вызываем внутренний метод
            _splitImages.UndoMove();


        /// <summary>
        /// Переходим к указанной картинке
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns>Инфомрация о картинке</returns>
        public CollectionInfo MoveToCollection(int direction) =>
            //Вызываем внутренний метод
            _splitImages.MoveToCollection(direction);

        /// <summary>
        /// Перемещаемся к изображению внутри папки
        /// </summary>
        /// <param name="direction">Направление перемещения</param>
        public void MoveToImage(int direction) =>
            //Вызываем внутренний метод
            _splitImages.MoveToImage(direction);


        /// <summary>
        /// Метод добавления новой папки
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        /// <param name="newFolderName">Имя создаваемой папки</param>
        public void AddNewFolder(SplitPathsInfo info, string newFolderName) =>
            //Вызываем внутренний метод
            _splitImages.AddNewFolder(info, newFolderName);

        /// <summary>
        /// Метод удаления папки из списка
        /// </summary>
        /// <param name="key">Клавиша, к которой привязана папка</param>
        public void RemoveFolderFromList(Key key) =>
            //Вызываем внутренний метод
            _splitImages.RemoveFolderFromList(key);


        /// <summary>
        /// Получаем папку для перемещения по нажатой кнопке
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>Папка для перемещеия</returns>
        public TargetFolderInfo GetMoveFolder(Key key) =>
            //Вызываем внутренний метод
            _targetsProcessor.GetMoveFolder(key);

    }
}
