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

        /// <summary>
        /// Метод получения списка униальных разрешений изображений списка
        /// </summary>
        /// <returns>Список уникальных разрешений</returns>
        public List<double> GetResolutions() =>
            //Из списка результатов
            Results
                //ВЫбираем только разрешения
                .Select(dup => dup.Resolution)
                //Получаем только униальные значения
                .Distinct()
                //Сортируем значения по убыванию
                .OrderByDescending(res => res)
                //В виде списка
                .ToList();
    }
}
