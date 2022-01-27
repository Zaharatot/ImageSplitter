using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.DataClases.Tags
{
    /// <summary>
    /// Класс тегированной картинки
    /// </summary>
    internal class TaggedImage
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Полынй путь к файлу
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Список тегов файла
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TaggedImage()
        {
            //Проставляем дефолтные значения
            Name = Path = "";
            Tags = new List<string>();
        }
    }
}
