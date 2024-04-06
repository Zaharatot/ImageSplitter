using ImageSplitterLib.Clases.DataClases;
using SplitterDataLib.DataClases.Files;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageSplitterLib.Clases.WorkClases.Collection
{
    /// <summary>
    /// Класс переноса файлов
    /// </summary>
    internal class CollectionMover
    {

        /// <summary>
        /// Класс поиска имени файла
        /// </summary>
        private ElementNameChecker _elementNameChecker;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public CollectionMover()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем класс поиска имени файла
            _elementNameChecker = new ElementNameChecker();
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
                collection.ElementName = _elementNameChecker.GetNewElementName(
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
                collection.ElementName = _elementNameChecker.GetNewElementName(
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
