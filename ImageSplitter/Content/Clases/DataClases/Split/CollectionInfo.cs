using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageSplitter.Content.Clases.DataClases.Global.Enums;

namespace ImageSplitter.Content.Clases.DataClases.Split
{
    /// <summary>
    /// Класс описывающий информацию о коллекции картинок
    /// </summary>
    internal class CollectionInfo
    {
        /// <summary>
        /// Путь к родительскому элементу
        /// </summary>
        public string ParentPath { get; set; }
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
        /// Флаг того, что коллекция является папкой
        /// </summary>
        public bool IsFolder { get; set; }
        /// <summary>
        /// Флаг перемещённой коллекции
        /// </summary>
        public bool IsMoved { get; set; }
        /// <summary>
        /// Размер файла изображения
        /// </summary>
        public long Length { get; set; }

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
            NewParentName = ParentPath = ElementName = null;
            IsMoved = false;
            IsFolder = false;
            FileNames = new List<string>();
            _currentImageId = 0;
            Length = 0;
        }

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
        public void MoveFolderImage(int direction)
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
