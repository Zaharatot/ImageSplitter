using DCTHashZ.Clases.DataClases.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanner.Clases.DataClases.Image
{
    /// <summary>
    /// Класс, хранящий данные об изображении
    /// </summary>
    internal class ByteImageInfo : IImageInfo
    {
        /// <summary>
        /// Пиксели изображения
        /// </summary>
        public byte[] Pixels { get; set; }
        /// <summary>
        /// Размер изображения
        /// </summary>
        public Size ImageSize { get; set; }
        /// <summary>
        /// Размер оригинального изображения
        /// </summary>
        public Size OriginalSize { get; set; }
        /// <summary>
        /// Флаг изображеняи в градациях серого
        /// </summary>
        public bool IsGrayScale { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="pixels">Массив пикселей изображения</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="height">Высота изображения</param>
        public ByteImageInfo(int height, int width, byte[] pixels)
        {
            //Проставляем дефолтные значения
            Pixels = pixels;
            ImageSize = new Size(height, width);
            IsGrayScale = true;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="pixels">Массив пикселей изображения</param>
        /// <param name="size">Размер изображения</param>
        public ByteImageInfo(Size size, byte[] pixels)
        {
            //Проставляем дефолтные значения
            Pixels = pixels;
            ImageSize = size;
            IsGrayScale = true;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="source">Класс для создания копии</param>
        public ByteImageInfo(ByteImageInfo source)
        {
            //Проставляем дефолтные значения
            Pixels = source.Pixels.Take(source.Pixels.Length).ToArray();
            ImageSize = source.ImageSize;
            OriginalSize = source.OriginalSize;
            IsGrayScale = source.IsGrayScale;
        }

    }
}
