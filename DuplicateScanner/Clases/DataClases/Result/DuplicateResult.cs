using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanner.Clases.DataClases.Result
{
    /// <summary>
    /// Класс информации о результатах поиска дубликатов
    /// </summary>
    public class DuplicateResult
    {
        /// <summary>
        /// Хеш пути к изображению
        /// </summary>
        public uint PathHash { get; set; }
        /// <summary>
        /// Строка полного пути к файлу
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Имя файла для отображения
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Путь к родительской папке
        /// </summary>
        public string ParentPath { get; set; }
        /// <summary>
        /// Название родительской папки
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// Ширина файла
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Высота файла
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Разрешение файла
        /// </summary>
        public double Resolution => Width * Height;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicateResult()
        {
            //Проставляем дефолтные значения
            PathHash = 0;
            Width = Height = 0;
            Path = Name = ParentPath = ParentName = "";
        }
    }
}
