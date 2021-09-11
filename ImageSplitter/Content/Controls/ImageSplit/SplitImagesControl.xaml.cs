using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.WorkClases;
using ImageSplitter.Content.Clases.WorkClases.Addition;
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
using static ImageSplitter.Content.Clases.DataClases.Delegates;
using static ImageSplitter.Content.Clases.DataClases.Enums;

namespace ImageSplitter.Content.Controls.ImageSplit
{
    /// <summary>
    /// Логика взаимодействия для SplitImagesControl.xaml
    /// </summary>
    public partial class SplitImagesControl : UserControl
    {
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
        /// События запуска сплита
        /// </summary>
        public event StartSplitScanEventHandler StartSplitScan;

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
            //Инициализируем класс расчсёта размера
            _sizeCalculator = new SizeCalculator();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку "Влево"
        /// </summary>
        private void LeftPageButton_Click(object sender, RoutedEventArgs e) =>
            //Идём к предыдущей картинке
            MoveToImageRequest?.Invoke(-1);

        /// <summary>
        /// Обработчик события нажатия на кнопку "Вправо"
        /// </summary>
        private void RightPageButton_Click(object sender, RoutedEventArgs e) =>
            //Идём к следующей картинке
            MoveToImageRequest?.Invoke(1);

        /// <summary>
        /// Обработчик события нажатия на кнопку добавления папки
        /// </summary>
        private void AddFolderButton_Click(object sender, RoutedEventArgs e) =>
            //Вызываем внешний ивент
            AddNewFolderRequest?.Invoke();

        /// <summary>
        /// Обработчик события нажатия на кнопку сканирования
        /// </summary>
        private void ScanButton_Click(object sender, RoutedEventArgs e) =>
            //ВЫзываем внешний ивент, передавая в него данные
            StartSplitScan?.Invoke(ScanPathTextBox.Text, MovePathTextBox.Text);

        /// <summary>
        /// Обработчик событяи запроса на удаление папки
        /// </summary>
        /// <param name="key">Клавиша, привязанная к целевой папке</param>
        /// <param name="folderName">Имя папки</param>
        private void FolderInfo_RemoveFolderRequest(Key key, string folderName)
        {
            //Если пользователь подтвердил удаление папки
            if (IsNeedRemoveFolder(folderName))
                //Вызываем внешний ивент
                RemoveFolderRequest?.Invoke(key, folderName);
        }


        /// <summary>
        /// Загружаем картинку по строке пути
        /// </summary>
        /// <param name="path">Путь к файлу картинки на диске</param>
        /// <returns>Класс картинки</returns>
        private BitmapImage LoadImageByPath(string path)
        {
            BitmapImage ex = new BitmapImage();
            ex.BeginInit();
            //Считываем байты файла в поток в памяти
            ex.StreamSource = new MemoryStream(File.ReadAllBytes(path));
            ex.EndInit();
            return ex;
        }

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
        /// Создаём контролл информации о папке
        /// </summary>
        /// <param name="info">Информация о папке</param>
        /// <returns>Сгенерированный контролл</returns>
        private FolderInfoControl CreateFolderInfoControl(TargetFolderInfo info)
        {
            //Инициализируем контролл информации о папке
            FolderInfoControl folderInfo = new FolderInfoControl();
            //Добавляем обработчик событяи запроса на удаление папки
            folderInfo.RemoveFolderRequest += FolderInfo_RemoveFolderRequest;
            //Проставляем в контролл информацию о папке
            folderInfo.SetTargetFolderInfo(info);
            //Возвращаем результат
            return folderInfo;
        }

        /// <summary>
        /// Запрос о необходимости удаления папки
        /// </summary>
        /// <param name="folderName">Имя целевой папки</param>
        /// <returns>True - папку нужно удалить</returns>
        private bool IsNeedRemoveFolder(string folderName)
        {
            //Вызываем сообщение с запросом подтверждения удаления папки
            var result = MessageBox.Show(
                $"Вы действительно хотите удалить папку \"{folderName}\" из списка?",
                "Запрос подтверждения",
                MessageBoxButton.YesNo);
            //Если пользователь нажал "Да" - то всё ок
            return (result == MessageBoxResult.Yes);
        }


        /// <summary>
        /// Обновляем список папок
        /// </summary>
        /// <param name="folders">Список папок</param>
        private void UpdateFoldersList(List<TargetFolderInfo> folders)
        {
            //Очищаем список папок
            FoldersList.Children.Clear();
            //Проходимся по папкам
            foreach (var folder in folders)
                //Генерируем и добавляем на панель контролл информации о папке
                FoldersList.Children.Add(CreateFolderInfoControl(folder));
        }

        /// <summary>
        /// Формируем строку информации об изображении
        /// </summary>
        /// <param name="image">Класс изображения</param>
        /// <param name="path">Путь к файлу изображения</param>
        /// <returns>Строка информации об изображении</returns>
        private string CompileImageInfoString(BitmapImage image, string path)
        {
            //Получаем информацию о файле изображения
            FileInfo imageFile = new FileInfo(path);
            //Формируем строку с инфой о файле, добавляя имя файла
            return $"[{imageFile.Name}] " +
                //Разрешение изображения
                $"[{image.PixelWidth}x{image.PixelHeight}] " +
                //Размер файла
                $"[{_sizeCalculator.GetStringSize(imageFile.Length)}]";
        }

        /// <summary>
        /// Грузим картинку и информацию о ней в контроллы
        /// </summary>
        /// <param name="imagePath">Путь к файлу картинки</param>
        private void LoadImageToControls(string imagePath)
        { 
            //Загружаем картинку
            BitmapImage image = LoadImageByPath(imagePath);
            //Проставляем картинку в контролл
            TargetImage.Source = image;
            //Формируем и проставляем информацию о картинке в контролл
            ImageInfoTextBlock.Text = CompileImageInfoString(image, imagePath);
        }

        /// <summary>
        /// Грузим на панель инфу о картинке
        /// </summary>
        /// <param name="info">Класс инфы о картинке</param>
        internal void LoadImageInfo(ImageInfo info)
        {
            //Закрываем поток в памяти, связанный с изображением
            CloseImageSource();
            //Если картинки нету
            if (info == null)
            {
                //Проставляем скрытие всему
                TargetImage.Source = null;
                MovedInfoTextBox.Visibility = Visibility.Hidden;
            }
            //Если всё ок
            else
            {
                //Грузим картинку и информацию о ней в контроллы
                LoadImageToControls(info.GetCurrentPath());
                //Проставляем видимость контроллу инфы о переносе
                MovedInfoTextBox.Visibility = (info.Status == ImageStatuses.Moved)
                    ? Visibility.Visible : Visibility.Hidden;
                //Проставляем перемещённую папку
                MovedFolderTextBox.Text = $"[{info.NewFolderName}]";
            }
        }

        /// <summary>
        /// Обновляем общую инфорамцию на форме
        /// </summary>
        /// <param name="pagesInfo">Инфомрация о текущих отображаемых страницах</param>
        /// <param name="folders">Список доступных папок</param>
        public void UpdateMainInfo(string pagesInfo, List<TargetFolderInfo> folders)
        {
            //Обновляем инфу о картинках
            CountImagesTextBlock.Text = pagesInfo;
            //Обновляем список папок
            UpdateFoldersList(folders);
        }


    }
}
