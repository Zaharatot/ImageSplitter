using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ImageSplitter.Content.Clases.DataClases;
using System.Windows.Input;
using ImageSplitter.Content.Clases.DataClases.Split;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit
{
    /// <summary>
    /// Класс сканирования папки на наличие коллекций
    /// </summary>
    internal class CollectionsScanner
    {
        /// <summary>
        /// Массив поддерживаемых расширений для изображений
        /// </summary>
        private string[] _imageExtensions;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public CollectionsScanner()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Формируем список поддерживаемых расширений
            _imageExtensions = new string[] { 
                ".bmp", ".png", ".jpg", ".jpeg", ".gif"
            };
        }

        /// <summary>
        /// Проверяем формат файла на допустимость
        /// </summary>
        /// <param name="file">Инфомрация о файле</param>
        /// <returns>True - файл является поддерживаемой картинкой</returns>
        private bool FileIsImage(FileInfo file) =>
            //Проверяем наличие расширения этого файла в списке допустимых
            _imageExtensions.Contains(file.Extension.ToLower());

        /// <summary>
        /// Конвертируем информацию о файле в информацию о коллекции
        /// </summary>
        /// <param name="file">Информация о файле</param>
        /// <param name="parentPath">Путь к родительской папке</param>
        /// <returns>ИНформация о коллекции</returns>
        private CollectionInfo ConvertFileInfoToCollectionInfo(FileInfo file, string parentPath) =>
            new CollectionInfo() {
                ElementName = file.Name,
                ParentPath = parentPath,
                Length = file.Length,
                IsFolder = false
            };
        /// <summary>
        /// Конвертируем информацию о папке в информацию о коллекции
        /// </summary>
        /// <param name="folder">Информация о папке</param>
        /// <param name="parentPath">Путь к родительской папке</param>
        /// <returns>ИНформация о коллекции</returns>
        private CollectionInfo ConvertDirectoryInfoToCollectionInfo(DirectoryInfo folder, string parentPath) =>
            new CollectionInfo() {
                ElementName = folder.Name,
                ParentPath = parentPath,
                FileNames = GetFolderChildImageNames(folder),
                IsFolder = true
            };

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
                .Where(file => FileIsImage(file))
                //От изображений берём только имена файлов
                .Select(image => image.Name)
                //Возвращаем в виде списка
                .ToList();

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
                .Where(file => FileIsImage(file))
                //Возвращаем в виде списка
                .ToList()
                //Конвертируем элементы списка в коллекции
                .ConvertAll(image => ConvertFileInfoToCollectionInfo(image, parent.FullName + "\\"));       

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
                .ConvertAll(folder => ConvertDirectoryInfoToCollectionInfo(folder, parent.FullName + "\\"));
       


        /// <summary>
        /// Сканируем изображения
        /// </summary>
        /// <param name="path">Путь для сканирования</param>
        /// <param name="isFolder">Флаг поиска папок</param>
        /// <returns>Список найденных изображений</returns>
        public List<CollectionInfo> ScanCollections(string path, bool isFolder)
        {
            List<CollectionInfo> collectionList = new List<CollectionInfo>();
            //Проверка наличия папки для сканирования
            if (Directory.Exists(path))
            {
                //Получаем инфу о папке
                DirectoryInfo parent = new DirectoryInfo(path);
                //Если нужно сканить папки
                if(isFolder)
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
        /// <param name="path">Путь к корневой папке</param>
        /// <returns>Список найденных папок</returns>
        public List<TargetFolderInfo> ScanFolders(string path)
        {
            List<TargetFolderInfo> ex = new List<TargetFolderInfo>();
            //Проверка наличия папки для сканирования
            if (Directory.Exists(path))
            {
                //Получаем инфу о папке
                DirectoryInfo di = new DirectoryInfo(path);
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
