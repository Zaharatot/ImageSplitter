using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ImageSplitter.Content.Clases.DataClases;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit
{
    /// <summary>
    /// Класс сканирования папки на наличие изображений
    /// </summary>
    internal class ImageScanner
    {
        /// <summary>
        /// Массив поддерживаемых расширений для изображений
        /// </summary>
        private string[] _imageExtensions;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImageScanner()
        {
            Init();
        }

        private void Init()
        {
            //Формируем список поддерживаемых расширений
            _imageExtensions = new string[] { 
                ".bmp", ".png", ".jpg", ".jpeg", ".gif"
            };
        }


        /// <summary>
        /// Сканируем изображения
        /// </summary>
        /// <param name="path">Путь для сканирования</param>
        /// <returns>Список найденных изображений</returns>
        public List<ImageInfo> ScanImages(string path)
        {
            List<ImageInfo> images = new List<ImageInfo>();
            //Проверка наличия папки для сканирования
            if (Directory.Exists(path))
            {
                //Получаем инфу о папке
                DirectoryInfo di = new DirectoryInfo(path);
                //Проходимся по найденным файлам
                foreach(var file in di.GetFiles())
                {
                    //Проверяем файл на то, что он является картинкой
                    if (FileIsImage(file))
                        //Добавляем инфу о картинке в список
                        images.Add(new ImageInfo() { 
                            OriginalFileName = file.Name,
                            OriginalPath = file.DirectoryName
                        });
                }
            }
            return images;
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
                        Path = dir.FullName
                    });
            }
            //Сортируем список по именам и возвращаем
            return ex.OrderBy(folder => folder.Name).ToList();
        }


        /// <summary>
        /// Проверяем формат файла на допустимость
        /// </summary>
        /// <param name="file">Инфомрация о файле</param>
        /// <returns>True - файл является поддерживаемой картинкой</returns>
        private bool FileIsImage(FileInfo file) =>
            //Проверяем наличие расширения этого файла в списке допустимых
            (_imageExtensions.Contains(file.Extension.ToLower()));
    }
}
