using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Split;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ImageSplitter.Content.Clases.DataClases.Global.Enums;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit
{
    /// <summary>
    /// Класс переноса файлов
    /// </summary>
    internal class Mover
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
        public Mover()
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
        private string GetNewElementName(string path, string name, bool isFolder)
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


        /// <summary>
        /// Перенос файла в целевую папку
        /// </summary>
        /// <param name="target">Информация о целевой папке</param>
        /// <param name="collection">Информация о переносимой коллекции</param>
        public void MoveCollection(TargetFolderInfo target, CollectionInfo collection)
        {
            //Если мы не пытаемся переместить элемент в ту же папку
            if (target.Path != collection.ParentPath)
            {
                //Получаем текущий путь
                string currentPath = collection.GetCurrentPath();
                //Проставляем новый путь к родительской папке 
                collection.ParentPath = target.Path;
                //Генерируем новое имя для элемента
                collection.ElementName = GetNewElementName(
                    target.Path, collection.ElementName, collection.IsFolder);
                //Проставляем имя новой родительской папки
                collection.NewParentName = target.Name;
                //Получаем новый путь к файлу
                string newPath = collection.GetCurrentPath();
                //Если у нас папка
                if (collection.IsFolder)
                    //Переносим папку
                    Directory.Move(currentPath, newPath);
                //Если у нас файл
                else
                    //Переносим файл
                    File.Move(currentPath, newPath);
                //Указываем, что коллекция была перемещена
                collection.IsMoved = true;
            }
        }

        /// <summary>
        /// Возврат коллекции в оригинальную папку
        /// </summary>
        /// <param name="collection">Информация о переносимой коллекции</param>
        public void UndoMove(CollectionInfo collection)
        {
            //Если мы не пытаемся переместить элемент в ту же папку
            if (collection.OriginalParentPath != collection.ParentPath)
            {
                //Получаем текущий путь к коллекции
                string currentPath = collection.GetCurrentPath();
                //Проставляем оригинальный путь к родительской папке 
                collection.ParentPath = collection.OriginalParentPath;
                //Генерируем новое имя для элемента (нужно, на случай, если в папку уже что-то
                //новое воткнули, ну и итератор будет для файла сбрасывать, что не особо критично)
                collection.ElementName = GetNewElementName(
                    collection.OriginalParentPath, 
                    collection.ElementName, collection.IsFolder);
                //Сбрасываем имя новой родительской папки
                collection.NewParentName = "";
                //Получаем новый путь к файлу
                string newPath = collection.GetCurrentPath();
                //Если у нас папка
                if (collection.IsFolder)
                    //Переносим папку
                    Directory.Move(currentPath, newPath);
                //Если у нас файл
                else
                    //Переносим файл
                    File.Move(currentPath, newPath);
                //Указываем, что перемещания не было
                collection.IsMoved = false;
            }
        }

    }
}
