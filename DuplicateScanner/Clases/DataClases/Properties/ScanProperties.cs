using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DuplicateScanner.Clases.DataClases.Global.Enums;

namespace DuplicateScanner.Clases.DataClases.Properties
{
    /// <summary>
    /// Класс параметров сканирования
    /// </summary>
    public class ScanProperties
    {
        /// <summary>
        /// Путь сканирования
        /// </summary>
        public string ScanPath { get; set; }
        /// <summary>
        /// Точность поиска
        /// </summary>
        public int ScanAccuracy { get; set; }
        /// <summary>
        /// Тип сканирования
        /// </summary>
        public ScanTypes ScanType { get; set; }




        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ScanProperties() 
        {
            //Проставляем дефолтные значения
            ScanPath = "";
            ScanAccuracy = 9;
            ScanType = ScanTypes.Both;
        }
    }
}
