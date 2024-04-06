using DuplicateScannerLib.Clases.DataClases.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScannerLib.Clases.DataClases.File
{
    /// <summary>
    /// Класс пары файлов-дубликатов
    /// </summary>
    public class DuplicatePair
    {
        /// <summary>
        /// Оригинальный файл
        /// </summary>
        public DuplicateResult Original { get; set; }
        /// <summary>
        /// Дубликат файла
        /// </summary>
        public DuplicateResult Copy { get; set; }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="original">Оригинальный файл</param>
        /// <param name="copy">Дубликат файла</param>
        public DuplicatePair(DuplicateInfo original, DuplicateInfo copy)
        {
            Original = original.GetResult();
            Copy = copy.GetResult();
        }

        /// <summary>
        /// Метод получения списка униальных разрешений изображений списка
        /// </summary>
        /// <returns>Список уникальных разрешений</returns>
        public List<double> GetResolutions() =>
            //Создаём список разрешений
            new List<double>() { Original.Resolution, Copy.Resolution }
                //Выбираем только уникальные значения
                .Distinct()
                //Сортируем значения по убыванию
                .OrderByDescending(res => res)
                //В виде списка
                .ToList();
    }
}
