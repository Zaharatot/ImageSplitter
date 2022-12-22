using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanner.Clases.DataClases.Result
{
    /// <summary>
    /// Класс результатов поиска дубликатов
    /// </summary>
    public class FindResult
    {
        /// <summary>
        /// Результаты поиска дубликатов
        /// </summary>
        public List<DuplicateResult> Results { get; set; }

        /// <summary>
        /// Флаг наличия дубликатов
        /// </summary>
        public bool IsContainDuplicates => Results.Count > 1;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="current">Текущий элемент для которого ищутся дубликаты</param>
        public FindResult(DuplicateResult current)
        {
            //Инициализируем переданные значения
            Results = new List<DuplicateResult>() { current };
        }
    }
}
