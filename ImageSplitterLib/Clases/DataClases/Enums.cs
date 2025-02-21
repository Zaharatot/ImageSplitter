using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitterLib.Clases.DataClases
{
    /// <summary>
    /// Класс глобальных перечислений
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Перечисление типов коллекций
        /// </summary>
        public enum CollectionTypes
        {
            /// <summary>
            /// Коллекция является изображением
            /// </summary>
            Image = 0,
            /// <summary>
            /// Коллекция является папкой
            /// </summary>
            Folder = 1,
            /// <summary>
            /// Коллекция является видео
            /// </summary>
            Video = 2,
        }
    }
}
