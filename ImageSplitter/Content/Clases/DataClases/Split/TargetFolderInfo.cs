using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.DataClases.Split
{
    /// <summary>
    /// Информация о целевой папке
    /// </summary>
    public class TargetFolderInfo
    {
        /// <summary>
        /// Путь к этой папке
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Имя папки
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Целевая клавиша
        /// </summary>
        public Key TargetKey { get; set; }


    }
}
