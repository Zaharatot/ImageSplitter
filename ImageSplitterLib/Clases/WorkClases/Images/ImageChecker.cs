using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageSplitterLib.Clases.DataClases.Enums;

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
                ".bmp", ".png", ".jpg", ".jpeg"//, ".webm", ".mp4"
            };

        /// <summary>
        /// Массив доступных к работе расширений для видео
        /// </summary>
        private static readonly string[] _videoExtensions = {
                ".dat", ".wmv", ".3g2", ".3gp", ".3gp2", ".3gpp", ".amv", ".asf", 
                ".avi", ".bin", ".cue", ".divx", ".dv", ".flv", ".gxf", ".iso", 
                ".m1v", ".m2v", ".m2t", ".m2ts", ".m4v", ".mkv", ".mov", ".mp2", 
                ".mp2v", ".mp4", ".mp4v", ".mpa", ".mpe", ".mpeg", ".mpeg1", ".mpeg2", 
                ".mpeg4", ".mpg", ".mpv2", ".mts", ".nsv", ".nuv", ".ogg", ".ogm", 
                ".ogv", ".ogx", ".ps", ".rec", ".rm", ".rmvb", ".tod", ".ts", ".tts", 
                ".vob", ".vro", ".webm", ".gif"
            };


        /// <summary>
        /// Проверяем формат файла на допустимость
        /// </summary>
        /// <param name="file">Инфомрация о файле</param>
        /// <returns>True - файл является поддерживаемой картинкой</returns>
        public static bool FileIsImage(FileInfo file) =>
            //Проверяем наличие расширения этого файла в списке допустимых
            _imageExtensions.Contains(file.Extension.ToLower());

        /// <summary>
        /// Проверяем формат файла на допустимость
        /// </summary>
        /// <param name="file">Инфомрация о файле</param>
        /// <returns>True - файл является поддерживаемым видео</returns>
        public static bool FileIsVideo(FileInfo file) =>
            //Проверяем наличие расширения этого файла в списке допустимых
            _videoExtensions.Contains(file.Extension.ToLower());

        /// <summary>
        /// Метод проверки на допустимость данного типа файла
        /// </summary>
        /// <param name="file">Инфомрация о файле</param>
        /// <returns>True - файл является поддерживаемым типом</returns>
        public static bool FileIsAllow(FileInfo file) =>
            FileIsImage(file) || FileIsVideo(file);

        /// <summary>
        /// Метод получения типа коллекции по файлу
        /// </summary>
        /// <param name="file">Инфомрация о файле</param>
        /// <returns>Тип коллекции</returns>
        public static CollectionTypes GetCollectionType(FileInfo file) => 
            //Если файл не картинка, то он видео
            FileIsImage(file) ? CollectionTypes.Image : CollectionTypes.Video;
    }
}
