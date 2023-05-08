using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.DataClases.Split
{
    /// <summary>
    /// Класс информации о древе
    /// </summary>
    internal class TreeInfo
    {
        /// <summary>
        /// Название папки
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Дочерние элементы древа
        /// </summary>
        public List<TreeInfo> Childs { get; set; }



    }
}
