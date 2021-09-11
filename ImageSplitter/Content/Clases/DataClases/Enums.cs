using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.DataClases
{
    /// <summary>
    /// Список глобальных перечислений
    /// </summary>
    internal class Enums
    {
        /// <summary>
        /// Статусы картинок
        /// </summary>
        public enum ImageStatuses
        {
            /// <summary>
            /// Изображение добавлено в список
            /// </summary>
            Added,
            /// <summary>
            /// Изображение перемещено в целевую папку
            /// </summary>
            Moved
        }

    }
}
