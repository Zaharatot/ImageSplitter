using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesSplitWindowLib.Content.Clases.DataClases
{
    /// <summary>
    /// Класс информации о перемещаемых файлах
    /// </summary>
    internal class MoveFilesInfo
    {
        /// <summary>
        /// Количество файлов в папке
        /// </summary>
        public int CountFilesInFolder { get; set; }
        /// <summary>
        /// Родительская папка
        /// </summary>
        public DirectoryInfo Parent { get; set; }
        /// <summary>
        /// Идентификатор текущей папки
        /// </summary>
        public int CurrentFolderId { get; private set; }
        /// <summary>
        /// Флаг сплита дочерних
        /// </summary>
        public bool IsChildSplit { get; private set; }

        /// <summary>
        /// Текущий путь к родительской папке
        /// </summary>
        public string CurrentParentPath => $@"{Parent.FullName}\{CurrentFolderId}\";
        /// <summary>
        /// Флаг корректности переданных данных для сплита
        /// </summary>
        public bool IsCorrectData => Parent.Exists && (CountFilesInFolder > 0);


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="countFilesInFolder">Количество файлов в папке</param>
        /// <param name="parent">Родительская папка</param>
        /// <param name="isChildSplit">Флаг сплита дочерних</param>
        public MoveFilesInfo(int countFilesInFolder, DirectoryInfo parent, bool isChildSplit)
        {
            //Проставляем переданные значения
            CountFilesInFolder = countFilesInFolder;
            Parent = parent;
            IsChildSplit = isChildSplit;
            //Проставляем дефолтные значения
            CurrentFolderId = 0;
        }


        /// <summary>
        /// Метод перехода к следующей папке
        /// </summary>
        public void MoveToNextFolder() =>
            CurrentFolderId++;
    }
}
