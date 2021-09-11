using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageSplitter.Content.Clases.DataClases
{
    /// <summary>
    /// Класс информации об изображении-дубликате
    /// </summary>
    public class DuplicateImageInfo
    {
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// Имя файла изображения
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Имя родительской папки изображения
        /// </summary>
        public string ParentFolderName { get; set; }
        /// <summary>
        /// Путь к файлу изображения
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Разрешение изображения
        /// </summary>
        public string Resolution { get; set; }
        /// <summary>
        /// Хеш изображения
        /// </summary>
        public ulong Hash { get; set; }
        /// <summary>
        /// Список дубликатов для данного изображения
        /// </summary>
        public List<DuplicateImageInfo> Duplicates { get; set; }
        /// <summary>
        /// Флаг необходимосчти удаления
        /// </summary>
        public bool IsNeedRemove { get; set; }
        /// <summary>
        /// Количество пикселей в изображении
        /// </summary>
        public uint PixelsCount { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicateImageInfo()
        {
            //Проставляем дефолтные значения
            PixelsCount = Id = 0;
            Resolution = Name = ParentFolderName = Path = "";
            Hash = 0;
            IsNeedRemove = true;
            Duplicates = new List<DuplicateImageInfo>();
        }
    }
}
