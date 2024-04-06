using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitterLib.Clases.WorkClases.Images
{
    /// <summary>
    /// Класс проверки на изображение
    /// </summary>
    internal static class ImageChecker
    {
        /// <summary>
        /// Массив доступных к работе расширений для изображений
        /// </summary>
        private static readonly string[] _imageExtensions = { 
                ".bmp", ".png", ".jpg", ".jpeg", ".gif"
            };



        /// <summary>
        /// Проверяем формат файла на допустимость
        /// </summary>
        /// <param name="file">Инфомрация о файле</param>
        /// <returns>True - файл является поддерживаемой картинкой</returns>
        public static bool FileIsImage(FileInfo file) =>
            //Проверяем наличие расширения этого файла в списке допустимых
            _imageExtensions.Contains(file.Extension.ToLower());
    }
}
