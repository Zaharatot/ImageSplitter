using ImageSplitterLib.Clases.DataClases;
using SplitImagesWindowLib.Content.Clases.WorkClases;
using SplitterDataLib.DataClases.Global.Split;
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
using static SplitterDataLib.DataClases.Global.Delegates;

namespace SplitImagesWindowLib.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для SplitImagesControl.xaml
    /// </summary>
    public partial class SplitImagesControl : UserControl
    {
        /// <summary>
        /// Событие запроса на переход к изображению
        /// </summary>
        public event MoveToImageEventHandler MoveToCollectionRequest;
        /// <summary>
        /// Событие запроса на переход к изображению
        /// </summary>
        public event MoveToImageEventHandler MoveToImageRequest;

        /// <summary>
        /// Событие запроса на добавление новой папки
        /// </summary>
        public event EmptyEventHandler AddNewFolderRequest;
        /// <summary>
        /// Событие запроса на удаление папки
        /// </summary>
        public event RemoveFolderRequestEventHandler RemoveFolderRequest;
        /// <summary>
        /// Событие запроса отмены переноса изображения
        /// </summary>
        public event EmptyEventHandler UndoMove;



        /// <summary>
        /// Класс рассчёта размера
        /// </summary>
        private SizeCalculator _sizeCalculator;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public SplitImagesControl()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
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
            //Инициализируем класс расчсёта размера
            _sizeCalculator = new SizeCalculator();
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void InitEvents()
        {
            //Добавляем обработчик события перехода к изображению
            TopPanel.MoveToImageRequest += TopPanel_MoveToImageRequest;
            //Добавляем обработчик события удаления папки
            FoldersList.RemoveFolderRequest += FoldersList_RemoveFolderRequest;
            //Добавляем обработчик события добавления папки
            FoldersList.AddNewFolderRequest += FoldersList_AddNewFolderRequest;
            //Добавляем обработчик события перехода к коллекции
            BottomPanel.MoveToCollectionRequest += BottomPanel_MoveToCollectionRequest;
            //Добавляем обработчик события запроса отмены переноса
            BottomPanel.UndoMove += BottomPanel_UndoMove;
        }

        /// <summary>
        /// Обработчик события запроса отмены переноса
        /// </summary>
        private void BottomPanel_UndoMove() =>
            //Вызываем внешний ивент
            UndoMove?.Invoke();


        /// <summary>
        /// Обработчик события перехода к коллекции
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private void BottomPanel_MoveToCollectionRequest(int direction) =>
            //Вызываем внешний ивент
            MoveToCollectionRequest?.Invoke(direction);

        /// <summary>
        /// Обработчик события добавления папки
        /// </summary>
        private void FoldersList_AddNewFolderRequest() =>
            //Вызываем внешний ивент
            AddNewFolderRequest?.Invoke();

        /// <summary>
        /// Jбработчик события удаления папки
        /// </summary>
        /// <param name="key">Клавиша, к которой привязана папка</param>
        /// <param name="folderName">Имя папки</param>
        private void FoldersList_RemoveFolderRequest(Key key, string folderName) =>
            //Вызываем внешний ивент
            RemoveFolderRequest?.Invoke(key, folderName);


        /// <summary>
        /// Обработчик события перехода к изображению
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        private void TopPanel_MoveToImageRequest(int direction) =>
            //Вызываем внешний ивент
            MoveToImageRequest?.Invoke(direction);





        /// <summary>
        /// Загружаем картинку по строке пути
        /// </summary>
        /// <param name="path">Путь к файлу картинки на диске</param>
        /// <returns>Класс картинки</returns>
        private async Task<BitmapImage> LoadImageByPath(string path) =>
            //Запускаем в отдельной таске
            await Dispatcher.InvokeAsync<BitmapImage>(() => {
            BitmapImage ex = new BitmapImage();
            ex.BeginInit();
            //Считываем байты файла в поток в памяти
            ex.StreamSource = new MemoryStream(File.ReadAllBytes(path)); 
            ex.EndInit();
            return ex;
        });

        /// <summary>
        /// Закрываем поток в памяти, связанный с изображением
        /// </summary>
        private void CloseImageSource()
        {
            //Если есть исходный поток в памяти
            if (TargetImage.Source != null)
            {
                //Проучаем изображение
                BitmapImage source = (BitmapImage)TargetImage.Source;
                //Очищаем поток
                source.StreamSource.Dispose();
            }
        }


        /// <summary>
        /// Формируем строку информации об изображении
        /// </summary>
        /// <param name="image">Класс изображения</param>
        /// <param name="info">Класс инфы о картинке</param>
        /// <returns>Строка информации об изображении</returns>
        private string CompileImageInfoString(BitmapImage image, CollectionInfo info)
        {
            StringBuilder sb = new StringBuilder();
            //Добавляем в строку имя элемента коллекции
            sb.Append($"[{info.ElementName}] ");
            //Если у нас папка
            if (info.IsFolder)
                //Добавляем в строку номер текущего выбраного элемента коллекции
                sb.Append($"[{info.GetCollectionSelectedElement()}]");
            //Если у нас файл
            else
            {
                //Добавляем в строку размер текущего изображения
                sb.Append($"[{image.PixelWidth}x{image.PixelHeight}] ");
                //Добавляем в строку размер файла изображения
                sb.Append($"[{_sizeCalculator.GetStringSize(info.Length)}]");
            }
            //ВОзвращаем итоговую строку
            return sb.ToString();
        }
        

        /// <summary>
        /// Грузим картинку и информацию о ней в контроллы
        /// </summary>
        /// <param name="info">Класс инфы о картинке</param>
        private async Task LoadImageToControls(CollectionInfo info)
        {
            //Закрываем поток в памяти, связанный с изображением
            CloseImageSource();
            //Получаем путь к картинке и загружаем её
            BitmapImage image = await LoadImageByPath(info.GetImagePath());
            //Проставляем картинку в контролл
            TargetImage.Source = image;
            //Формируем и проставляем информацию о картинке в контролл
            TopPanel.SetCollectionInfo(CompileImageInfoString(image, info));
        }

        /// <summary>
        /// Очищаем все важные дочерние контроллы
        /// </summary>
        private void ClearControls()
        {
            //Закрываем поток в памяти, связанный с изображением
            CloseImageSource();
            //Проставляем скрытие всему
            TargetImage.Source = null;
            BottomPanel.SetMovedFolderInfo("", false);
            //Проставляем пустой текст в контроллы
            TopPanel.SetCollectionInfo("...");
            BottomPanel.SetPagesInfo("...");
        }

        /// <summary>
        /// Грузим контент из класса в контроллы
        /// </summary>
        /// <param name="info">Класс инфы о картинке</param>
        private async Task LoadContentToControls(CollectionInfo info)
        {
            //Грузим картинку и информацию о ней в контроллы
            await LoadImageToControls(info);
            //Проставляем информацию о переносе
            BottomPanel.SetMovedFolderInfo(info.NewParentName, info.IsMoved);
            //Проставляем статус активности для кнопок листания
            TopPanel.SettButtonsEnableState(info.IsFolder);
        }


        /// <summary>
        /// Грузим на панель инфу о картинке
        /// </summary>
        /// <param name="info">Класс инфы о картинке</param>
        internal async Task LoadImageInfo(CollectionInfo info)
        {
            //Если картинки нету
            if (info == null)
                //Очищаем все важные дочерние контроллы
                ClearControls();
            //Если всё ок
            else
                await LoadContentToControls(info);
        }

        /// <summary>
        /// Обновляем общую инфорамцию на форме
        /// </summary>
        /// <param name="pagesInfo">Инфомрация о текущих отображаемых страницах</param>
        /// <param name="folders">Список доступных папок</param>
        public void UpdateMainInfo(string pagesInfo, List<TargetFolderInfo> folders)
        {
            //Обновляем инфу о страницах коллекции
            BottomPanel.SetPagesInfo(pagesInfo);
            //Обновляем список папок
            FoldersList.UpdateFoldersList(folders);
        }

        /// <summary>
        /// Метод простановки информации о путях сплита
        /// </summary>
        /// <param name="info">Информация о пути</param>
        public void SetSplitPathInfo(SplitPathsInfo info) =>
            //Вызываем внутренний метод
            SplitPathsInfoPanel.SetSplitPathInfo(info);
    }
}
