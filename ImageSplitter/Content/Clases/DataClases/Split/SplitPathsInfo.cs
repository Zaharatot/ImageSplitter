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
        /// Флаг запуска сплита.
        /// Используеются для передачи информации из окна.
        /// </summary>
        public bool IsStartSplit { get; set; }
        /// <summary>
        /// Флаг наличия путей для сканирования
        /// </summary>
        public bool IsContainPaths => 
            //Проверяем наличие обоих путей
            !(string.IsNullOrEmpty(ScanPath) || string.IsNullOrEmpty(MovePath));

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SplitPathsInfo()
        {
            //Проставляем дефолтные параметры
            ScanPath = MovePath = "";
            IsStartSplit = IsFolder = false;
        }
    }
}
