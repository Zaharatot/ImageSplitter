using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.DataClases.Split
{
    /// <summary>
    /// Класс информации о путях для сплита
    /// </summary>
    public class SplitPathsInfo
    {
        /// <summary>
        /// Путь для поиска изображений
        /// </summary>
        public string ScanPath { get; set; }
        /// <summary>
        /// Путь для перемещения изображений
        /// </summary>
        public string MovePath { get; set; }
        /// <summary>
        /// Флаг поиска папок
        /// </summary>
        public bool IsFolder { get; set; }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SplitPathsInfo()
        {
            //Проставляем дефолтные параметры
            ScanPath = MovePath = "";
            IsFolder = false;
        }
    }
}
