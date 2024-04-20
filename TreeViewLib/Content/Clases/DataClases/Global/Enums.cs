using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeViewWindowLib.Content.Clases.DataClases.Global
{
    /// <summary>
    /// Список глобальных перечислений
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Статусы директории
        /// </summary>
        public enum DirectoryStatuses
        {
            /// <summary>
            /// Полностью расспличенная папка
            /// </summary>
            Splitted = 0,
            /// <summary>
            /// Папка имеет не расспличенные дочерние
            /// </summary>
            NotSplitted = 1,
            /// <summary>
            /// Папка имеет дочерние, которые не расспличены
            /// </summary>
            ChildsNotSplitted = 2
        }
    }
}
