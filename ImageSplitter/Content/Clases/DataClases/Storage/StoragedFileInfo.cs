using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.DataClases.Storage
{
    /// <summary>
    /// Класс информации о хранимом файле
    /// </summary>
    internal class StoragedFileInfo
    {
        /// <summary>
        /// Полный путь к файлу изображения
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Хеш изображения
        /// </summary>
        public ulong Hash { get; set; }

        /// <summary>
        /// Список хешей файлов, которые не являются дубликатами
        /// </summary>
        public List<ulong> NotDuplicateHashes { get; set; }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public StoragedFileInfo()
        {
            //Проставляем деофлтные значения
            Path = "";
            Hash = 0;
            NotDuplicateHashes = new List<ulong>();
        }
    }
}
