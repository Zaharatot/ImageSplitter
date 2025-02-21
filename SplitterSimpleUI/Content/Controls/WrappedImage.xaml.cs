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

namespace SplitterSimpleUI.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для WrappedImage.xaml
    /// </summary>
    public partial class WrappedImage : UserControl
    {
        /// <summary>
        /// Размер изображения
        /// </summary>
        public Size? ImageSize { get; private set; }
        /// <summary>
        /// Имя файла изображения
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// Размер изображения
        /// </summary>
        public long ImageLength { get; private set; }


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public WrappedImage()
        {
            InitializeComponent();
        }



        /// <summary>
        /// Загружаем картинку по строке пути
        /// </summary>
        /// <param name="path">Путь к файлу изображения</param>
        /// <param name="decodePixelHeight">Размер пикселя для превью</param>
        /// <returns>Класс картинки</returns>
        private BitmapImage LoadImageSource(string path, int decodePixelHeight)
        {
            //Инициализируем класс изображения
            BitmapImage image = new BitmapImage();
            //Инициализируем инициализацию
            image.BeginInit();
            //Принудительно проставляем высоту для превью
            image.DecodePixelHeight = decodePixelHeight;
            //Передаём поток файла
            image.StreamSource = new MemoryStream(File.ReadAllBytes(path));
            //Завершаем инициализацию
            image.EndInit();
            //Возвращаем загруженное изображение
            return image;
        }

        /// <summary>
        /// Пытаемся загрузить картинку
        /// </summary>
        /// <param name="path">Путь к файлу изображения</param>
        /// <param name="decodePixelHeight">Размер пикселя для превью</param>
        /// <returns>Класс картинки</returns>
        private ImageSource TryLoadImage(string path, int decodePixelHeight)
        {
            BitmapImage image = null;
            try
            {
                //Получаем информацию о файле
                FileInfo file = new FileInfo(path);
                //Если файл изображения существует
                if (File.Exists(path))
                {
                    //Грузим его
                    image = LoadImageSource(path, decodePixelHeight);
                    //Проставляем имя и размер файла
                    FileName = file.Name;
                    ImageLength = file.Length;
                    //Возвращаем размеры загруженного изобрежния
                    ImageSize = new Size(image.PixelWidth, image.PixelHeight);
                }
            }
            //В случае ошибок будет Null
            catch { image = null; }
            //Возвращаем null
            return image;
        }

        /// <summary>
        /// Метод обработки загрузки картинки
        /// </summary>
        /// <param name="path">Путь к файлу изображения</param>
        /// <param name="decodePixelHeight">Размер пикселя для превью</param>
        private void LoadImageProcessor(string path, int decodePixelHeight)
        {
            //Пытаемся загрузить изображение по пути
            ImageSource source = TryLoadImage(path, decodePixelHeight);
            //Если загрузка не удалась
            if(source == null)
            {
                //Отображаем панель ошибки и скрываем картинку
                ErrorPanel.Visibility = Visibility.Visible;
                MainImage.Visibility = Visibility.Collapsed;
            }
            //Если загрузка прошла успешно
            else
            {
                //Вставляем картинку в изображение
                MainImage.Source = source;
                //Отображаем картинку и скрываем панель ошибки 
                ErrorPanel.Visibility = Visibility.Collapsed;
                MainImage.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Метод очистки информации об изображении
        /// </summary>
        private void ClearImageInfo()
        {
            //Сбрасываем размер и имя файла изображения
            ImageSize = null;
            FileName = "";
            ImageLength = 0;
        }


        /// <summary>
        /// Закрываем поток в памяти, связанный с изображением
        /// </summary>
        public void CloseImageSource()
        {
            //Удаляем информацию об изображении
            ClearImageInfo();
            //Если есть исходный поток в памяти, и он является загруженной картинкой
            if ((MainImage.Source != null) && (MainImage.Source is BitmapImage source))
            {
                //Очищаем поток
                source.StreamSource.Dispose();
                //Закрываем поток
                source.StreamSource.Close();
            }
            //Удаляем ссылку на поток
            MainImage.Source = null;
        }

        /// <summary>
        /// Метод загрузки изображения по пути
        /// </summary>
        /// <param name="path">Путь к файлу изображения</param>
        /// <param name="decodePixelHeight">Размер пикселя для превью</param>
        /// <returns>Размер загруженного изображения</returns>
        public async Task LoadImageAsync(string path, int decodePixelHeight = 0) =>
            //Запускаем в отдельной таске, для текущего контролла
            await this.Dispatcher.InvokeAsync(() => {
                //Отображаем прогрессбар
                MainProgress.Visibility = Visibility.Visible;
                //Закрываем предыдущий поток изображения, если он существовал
                CloseImageSource();
                //Грузим картинку в контролл
                LoadImageProcessor(path, decodePixelHeight);
                //Скрываем прогрессбар
                MainProgress.Visibility = Visibility.Collapsed;
            });
    }
}
