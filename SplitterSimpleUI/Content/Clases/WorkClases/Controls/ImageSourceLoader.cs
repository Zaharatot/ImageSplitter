using SplitterResources;
using SplitterResources.Content.Clases.DataClases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SplitterSimpleUI.Content.Clases.WorkClases.Controls
{
    /// <summary>
    /// Класс загрузки изображения по пути
    /// </summary>
    public class ImageSourceLoader
    {
        /// <summary>
        /// Иконка ошибки
        /// </summary>
        private SvgImage _errorIcon;
        /// <summary>
        /// Цвет заливки иконки
        /// </summary>
        private SolidColorBrush _iconColor;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImageSourceLoader()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Грузим иконку и цвет
            _iconColor = ResourceLoader.LoadBrush("Brush_ActiveColor");
            _errorIcon = ResourceLoader.LoadIcon("Icon_Warning");
        }


        /// <summary>
        /// Метод загрузки иконки ошибки
        /// </summary>
        /// <returns>Отрисованная иконка</returns>
        private ImageSource LoadWarningIcon()
        {
            //Инициализируем новую коллекцию частей
            DrawingCollection colorizedCollection = new DrawingCollection();
            //Проходимся по существующим частям
            foreach (Geometry path in _errorIcon.PathList)
                //Добавляем в раскрашенную коллекцию часть указав новый цвет заливки
                colorizedCollection.Add(new GeometryDrawing(_iconColor, null, path));
            //Инициализируем новое изображение
            return new DrawingImage(new DrawingGroup() {
                Children = colorizedCollection
            });
        }


        /// <summary>
        /// Загружаем картинку по строке пути
        /// </summary>
        /// <param name="path">Путь к файлу картинки на диске</param>
        /// <param name="decodePixelHeight">Размер пикселя для превью</param>
        /// <param name="size">Размер картинки</param>
        /// <returns>Класс картинки</returns>
        private ImageSource LoadImageByPath(string path, int decodePixelHeight, out Size size) 
        {
            //Инициализируем класс изображения
            BitmapImage image = new BitmapImage();
            //Инициализируем инициализацию
            image.BeginInit();
            //Принудительно проставляем высоту для превью
            image.DecodePixelHeight = decodePixelHeight;
            //Передаём поток файла
            image.StreamSource = File.OpenRead(path);
            //Завершаем инициализацию
            image.EndInit();
            //Возвращаем размеры загруженного изобрежния
            size = new Size(image.PixelWidth, image.PixelHeight);
            //Возвращаем загруженное изображение
            return image;
        }


        /// <summary>
        /// Загружаем картинку по строке пути
        /// </summary>
        /// <param name="path">Путь к файлу картинки на диске</param>
        /// <param name="decodePixelHeight">Размер пикселя для превью</param>
        /// <param name="size">Размер картинки</param>
        /// <returns>Класс картинки</returns>
        private ImageSource LoadImage(string path, int decodePixelHeight, out Size size)
        {
            try
            {
                //Если файл изображения существует
                if (File.Exists(path))
                    //Грузим его
                    return LoadImageByPath(path, decodePixelHeight, out size);
            }
            //В случае ошибок будет Null
            catch { }
            //Возвращаем нулевой размер
            size = new Size(0,0);
            //Возвращаем null
            return null;
        }

        /// <summary>
        /// Метод загрузки иконки
        /// </summary>
        /// <param name="element">Контролл, для которого выполняем загрузку</param>
        /// <param name="path">Путь к файлу изображения</param>
        /// <param name="decodePixelHeight">Размер пикселя для превью</param>
        /// <returns>Размер загруженного изображения</returns>
        private Size LoadImageToElement(Image element, string path, int decodePixelHeight)
        {
            //Грузим картинку из файла
            ImageSource source = LoadImage(path, decodePixelHeight, out Size size);
            //Если картинка не была загрузена - ставим иконку ошибки
            element.Source = source ?? LoadWarningIcon();
            //Возвращаем полученный размер
            return size;
        }





        /// <summary>
        /// Закрываем поток в памяти, связанный с изображением
        /// </summary>
        /// <param name="element">Контролл, для которого выполняем загрузку</param>
        public void CloseImageSource(Image element)
        {
            //Если есть исходный поток в памяти, и он является загруженной картинкой
            if ((element.Source != null) && (element.Source is BitmapImage source))
            {
                //Очищаем поток
                source.StreamSource.Dispose();
                //Закрываем поток
                source.StreamSource.Close();
            }
            //Удаляем ссылку на поток
            element.Source = null;
        }

        /// <summary>
        /// Метод загрузки изображения по пути
        /// </summary>
        /// <param name="element">Контролл, для которого выполняем загрузку</param>
        /// <param name="path">Путь к файлу изображения</param>
        /// <param name="decodePixelHeight">Размер пикселя для превью</param>
        /// <returns>Размер загруженного изображения</returns>
        public async Task<Size> LoadImageByPath(Image element, string path, int decodePixelHeight = 0) =>
            //Запускаем в отдельной таске, для текущего контролла
            await element.Dispatcher.InvokeAsync<Size>(() => {
                //Закрываем предыдущий поток изображения, если он существовал
                CloseImageSource(element);
                //Грузим картинку в контролл, и возвращаем размер
                return LoadImageToElement(element, path, decodePixelHeight);
            });

    }
}
