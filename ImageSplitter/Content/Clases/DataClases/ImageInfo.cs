using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageSplitter.Content.Clases.DataClases.Enums;

namespace ImageSplitter.Content.Clases.DataClases
{
    /// <summary>
    /// Класс описывающий информацию о картинке
    /// </summary>
    internal class ImageInfo
    {
        /// <summary>
        /// Оригинальный путь к изображению
        /// </summary>
        public string OriginalPath { get; set; }
        /// <summary>
        /// Новый путь к изображению
        /// </summary>
        public string NewPath { get; set; }
        /// <summary>
        /// Оришинальное имя файла
        /// </summary>
        public string OriginalFileName { get; set; }
        /// <summary>
        /// Новое имя файла
        /// </summary>
        public string NewFileName { get; set; }
        /// <summary>
        /// Имя новой папки
        /// </summary>
        public string NewFolderName { get; set; }
        /// <summary>
        /// Статус работы с картинкой
        /// </summary>
        public ImageStatuses Status { get; set; }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImageInfo()
        {
            //Проставляем дефолтные значения
            NewFolderName = OriginalPath = NewPath = 
                NewFileName = OriginalFileName = null;
            Status = ImageStatuses.Added;
        }

        /// <summary>
        /// Получаем текущий путь к картинке
        /// </summary>
        /// <returns>Строка пути к файлу</returns>
        public string GetCurrentPath() =>
            (Status == ImageStatuses.Added) ? 
                $"{OriginalPath}\\{OriginalFileName}" : 
                $"{NewPath}\\{NewFileName}";

    }
}
