using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanner.Clases.DataClases.Result
{
    /// <summary>
    /// Класс найденной пары элементов
    /// </summary>
    public class FindPair
    {
        /// <summary>
        /// Оригинальный файл
        /// </summary>
        public DuplicateResult Original { get; set; }
        /// <summary>
        /// Дубликат файла
        /// </summary>
        public DuplicateResult Copy { get; set; }


        

    }
}
