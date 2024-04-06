using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SplitterDataLib.DataClases.Files
{
    /// <summary>
    /// Класс поиска нового имени для элемента
    /// </summary>
    public class ElementNameChecker
    {
        /// <summary>
        /// Регулярное выражение описывающее 
        /// итератор в начале названия элемента
        /// </summary>
        private Regex _startIterator;
        /// <summary>
        /// Регулярное выражение описывающее 
        /// итератор в конце названия элемента
        /// </summary>
        private Regex _endIterator;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ElementNameChecker()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем регулярные выражения
            _startIterator = new Regex(@"^[ ]?\(\d+\)[ ]?");
            _endIterator = new Regex(@"[ ]?\(\d+\)[ ]?$");
        }



        /// <summary>
        /// Очищаем оригинальное имя элемента от итераторов
        /// </summary>
        /// <param name="name">Имя элемента</param>
        /// <returns>Строка очищенного имени элемента</returns>
        private string ClearElementName(string name)
        {
            //Удаляем возможный итератор в начале имени
            name = _startIterator.Replace(name, "");
            //Удаляем возможный итератор в конце имени
            name = _endIterator.Replace(name, "");
            //Возвращаем результат
            return name;
        }

        /// <summary>
        /// Проверяем существование элемента с таким именем
        /// </summary>
        /// <param name="path">Полный путь к элементу</param>
        /// <param name="isFolder">Флаг того, что элемент является папкой</param>
        /// <returns>True - элемент существует</returns>
        private bool IsElementExists(string path, bool isFolder) =>
            (isFolder)
                ? Directory.Exists(path)
                : File.Exists(path);



        /// <summary>
        /// Подбираем новое имя для файла, которого нету в новой папке
        /// </summary>
        /// <param name="path">Путь для поиска</param>
        /// <param name="name">Старое имя файла</param>
        /// <param name="isFolder">Флаг того, что элемент является папкой</param>
        /// <returns>Уникальное имя файла</returns>
        public string GetNewElementName(string path, string name, bool isFolder)
        {
            //Ставим дефолтный итератор
            int id = 0;
            //Получаем имя элемента, очищенное от итераторов
            string clearedName = ClearElementName(name);
            //Проставляем дефолтное имя элемента как точку старта проверки
            string ex = clearedName;
            //Пока есть такой элемент в целевой папке
            while (IsElementExists($"{path}\\{ex}", isFolder))
                //Генерим новые имена
                ex = $"({id++}) {clearedName}";
            //Возвращаем результат
            return ex;
        }

    }
}
