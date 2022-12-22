using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanner.Clases.DataClases.Result
{
    /// <summary>
    /// Класс информации о прогрессе работы
    /// </summary>
    public class ProgressInfo
    {
        /// <summary>
        /// Количество файлов для обработки
        /// </summary>
        public int MaxCount { get; set; }
        /// <summary>
        /// Количество обработанных файлов
        /// </summary>
        public int Processed { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ProgressInfo()
        {
            //Проставляем дефолтные значения
            MaxCount = Processed = 0;
        }
    }
}
