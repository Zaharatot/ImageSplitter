using ImageSplitterLib.Clases.WorkClases.Images;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageSplitterLib.Clases.DataClases.Enums;

namespace ImageSplitterLib.Clases.DataClases
{
    /// <summary>
    /// Класс описывающий информацию о коллекции картинок
    /// </summary>
    public class CollectionInfo
    {
        /// <summary>
        /// Путь к родительскому элементу
        /// </summary>
        public string ParentPath { get; set; }
        /// <summary>
        /// Оригинальный путь к родительскому элементу
        /// </summary>
        public string OriginalParentPath { get; private set; }
        /// <summary>
        /// Имя элемента
        /// </summary>
        public string ElementName { get; set; }
        /// <summary>
        /// Имя новой родительской папки
        /// </summary>
        public string NewParentName { get; set; }
        /// <summary>
        /// Список имён файлов
        /// Используется только для папок
        /// </summary>
        public List<string> FileNames { get; set; }
        /// <summary>
        /// Тип коллекции
        /// </summary>
        public CollectionTypes CollectionType { get; set; }
        /// <summary>
        /// Флаг перемещённой коллекции
        /// </summary>
        public bool IsMoved { get; set; }

        /// <summary>
        /// Флаг папки
        /// </summary>
        public bool IsFolder => CollectionType == CollectionTypes.Folder;

        /// <summary>
        /// Идентификатор текущего выбранного файла
        /// </summary>
        private int _currentImageId;




        /// <summary>
        /// Конструктор класса
        /// </summary>
        public CollectionInfo()
        {
            //Проставляем дефолтные значения
            OriginalParentPath = NewParentName = 
                ParentPath = ElementName = null;
            IsMoved = false;
            CollectionType = CollectionTypes.Image;
            FileNames = new List<string>();
            _currentImageId = 0;
        }

        /// <summary>
        /// Конструктор класса, для файла
        /// </summary>
        /// <param name="file">Информация о файле</param>
        /// <param name="parentPath">Путь к родительской папке</param>
        public CollectionInfo(FileInfo file, string parentPath)
        {
            //Проставляем переданные значения
            ElementName = file.Name;
            OriginalParentPath = ParentPath = parentPath;
            CollectionType = ImageChecker.GetCollectionType(file);
            //Проставляем дефолтные значения
            FileNames = new List<string>();
            NewParentName = null;
            IsMoved = false;
            _currentImageId = 0;
        }


        /// <summary>
        /// Конструктор класса, для папки
        /// </summary>
        /// <param name="folder">Информация о папке</param>
        /// <param name="parentPath">Путь к родительской папке</param>
        public CollectionInfo(DirectoryInfo folder, string parentPath)
        {
            //Проставляем переданные значения
            FileNames = GetFolderChildImageNames(folder);
            ElementName = folder.Name;
            OriginalParentPath = ParentPath = parentPath;
            CollectionType = CollectionTypes.Folder;
            //Проставляем дефолтные значения
            NewParentName = null;
            IsMoved = false;
            _currentImageId = 0;
        }

        /// <summary>
        /// Получаем список дочерних изображений из папки
        /// </summary>
        /// <param name="parent">Родительская папка</param>
        /// <returns>Список имён дочерних изображений</returns>
        private List<string> GetFolderChildImageNames(DirectoryInfo parent) =>
            //Из папки
            parent
                //Получаем файлы
                .GetFiles()
                //Выбираем из них только изображения
                .Where(file => ImageChecker.FileIsImage(file))
                //От изображений берём только имена файлов
                .Select(image => image.Name)
                //Возвращаем в виде списка
                .ToList();



        /// <summary>
        /// Получаем текущий путь к коллекции
        /// </summary>
        /// <returns>Строка пути к файлу</returns>
        public string GetCurrentPath() =>
            //Если у нас папка
            (IsFolder)
                //Возвращаем путь к текущему выбранному изображению
                ? $"{ParentPath}{ElementName}"
                //Если файл - просто текущий путь к элементу
                : $"{ParentPath}{ElementName}";


        /// <summary>
        /// Получаем текущий путь к картинке
        /// </summary>
        /// <returns>Строка пути к файлу</returns>
        public string GetImagePath() =>
            //Если у нас папка
            (IsFolder)
                //Возвращаем путь к текущему выбранному изображению
                ? $"{ParentPath}{ElementName}\\{FileNames[_currentImageId]}"
                //Если файл - просто текущий путь к элементу
                : $"{ParentPath}{ElementName}";

        /// <summary>
        /// Перемещаемся к изображению внутри папки
        /// </summary>
        /// <param name="direction">Направление перемещения</param>
        public void MoveToImage(int direction)
        {
            //Если картинки вообще есть
            if (FileNames.Count > 0)
            {
                //Переходим по направлению
                _currentImageId += direction;
                //Засовываем значение идентификатора
                //обратно в рамки если оно зха них вышло
                if (_currentImageId >= FileNames.Count)
                    _currentImageId = FileNames.Count - 1;
                if (_currentImageId < 0)
                    _currentImageId = 0;
            }
        }

        /// <summary>
        /// Возвращаем информацию о выбранном изображении
        /// </summary>
        /// <returns>Строка номера выбранного изображения</returns>
        public string GetCollectionSelectedElement() =>
            $"{_currentImageId + 1} / {FileNames.Count}";

    }
}
