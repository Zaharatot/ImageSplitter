using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Input;
using ImageSplitterLib.Clases.DataClases;
using SplitterDataLib.DataClases.Global.Split;
using ImageSplitterLib.Clases.WorkClases.Images;

namespace ImageSplitterLib.Clases.WorkClases.Collection
{
    /// <summary>
    /// Класс сканирования папки на наличие коллекций
    /// </summary>
    internal class CollectionsScanner
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public CollectionsScanner()
        {

        }


        /// <summary>
        /// Загружаем изображения в коллекцию
        /// </summary>
        /// <param name="parent">Родительская папка</param>
        /// <returns>Список коллекций</returns>
        private List<CollectionInfo> LoadImagesToCollections(DirectoryInfo parent) =>
            //Из папки
            parent
                //Получаем файлы
                .GetFiles()
                //Выбираем из них только изображения
                .Where(file => ImageChecker.FileIsImage(file))
                //Сортируем картинки по имени
                .OrderBy(image => image.Name)
                //Возвращаем в виде списка
                .ToList()
                //Конвертируем элементы списка в коллекции
                .ConvertAll(image => new CollectionInfo(image, parent.FullName + "\\"));

        /// <summary>
        /// Загружаем папки в коллекцию
        /// </summary>
        /// <param name="parent">Родительская папка</param>
        /// <returns>Список коллекций</returns>
        private List<CollectionInfo> LoadFoldersToCollections(DirectoryInfo parent) =>
            //Из папки
            parent
                //Получаем дочерние директории
                .GetDirectories()
                //Возвращаем в виде списка
                .ToList()
                //Конвертируем элементы списка в коллекции
                .ConvertAll(folder => new CollectionInfo(folder, parent.FullName + "\\"));
       


        /// <summary>
        /// Сканируем изображения
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        /// <returns>Список найденных изображений</returns>
        public List<CollectionInfo> ScanCollections(SplitPathsInfo info)
        {
            List<CollectionInfo> collectionList = new List<CollectionInfo>();
            //Проверка наличия папки для сканирования
            if (Directory.Exists(info.ScanPath))
            {
                //Получаем инфу о папке
                DirectoryInfo parent = new DirectoryInfo(info.ScanPath);
                //Если нужно сканить папки
                if(info.IsFolder)
                {
                    //Сканим папки в коллекции
                    collectionList = LoadFoldersToCollections(parent);
                    //Удаляем все коллекции, в которых нет изображений
                    collectionList.RemoveAll(elem => !elem.FileNames.Any());
                }
                //В противном случае
                else
                    //Грузим изображения
                    collectionList = LoadImagesToCollections(parent);
            }
            //Возвращаем результат
            return collectionList;
        }

        /// <summary>
        /// Сканируем список папок
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        /// <returns>Список найденных папок</returns>
        public List<TargetFolderInfo> ScanFolders(SplitPathsInfo info)
        {
            List<TargetFolderInfo> ex = new List<TargetFolderInfo>();
            //Проверка наличия папки для сканирования
            if (Directory.Exists(info.MovePath))
            {
                //Получаем инфу о папке
                DirectoryInfo di = new DirectoryInfo(info.MovePath);
                //Проходимся по найденным папкам
                foreach (var dir in di.GetDirectories())
                    ex.Add(new TargetFolderInfo() { 
                        Name = dir.Name,
                        Path = dir.FullName + "\\"
                    });
            }
            //Сортируем список по именам и возвращаем
            return ex.OrderBy(folder => folder.Name).ToList();
        }


    }
}
