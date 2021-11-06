using DCTHashZ.Clases.DataClases.ImageWork;
using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Duplicates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static DCTHashZ.Clases.DataClases.Other.Enums;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.FindDuplicates
{
    /// <summary>
    /// Класс конвертации результатов вычисленяи 
    /// хешей во внутренний тип инфомрации о файле
    /// </summary>
    internal class ImageInfoConverter
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImageInfoConverter()
        {

        }


        /// <summary>
        /// Получаем имя целоевой директории
        /// </summary>
        /// <param name="path">Путь к директории</param>
        /// <returns>Имя директории</returns>
        private string GetDirectoryName(string path) =>
            new DirectoryInfo(path).Name;


        /// <summary>
        /// Загружаем картинку по строке пути
        /// </summary>
        /// <param name="path">Путь к файлу картинки на диске</param>
        /// <returns>Класс картинки</returns>
        private BitmapImage LoadImageByPath(string path)
        {
            BitmapImage ex = new BitmapImage();
            ex.BeginInit();
            ex.CacheOption = BitmapCacheOption.None;
            ex.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            //Считываем байты файла в поток в памяти
            ex.StreamSource = File.OpenRead(path);
            ex.EndInit();
            return ex;
        }

        /// <summary>
        /// Получаем разрешение изображения
        /// </summary>
        /// <param name="image">Изображение для получения разрешения</param>
        /// <returns>Строка с разрешением изображения</returns>
        private string GetImageResolution(BitmapImage image) =>
            $"[{image.PixelWidth}x{image.PixelHeight}]";

        /// <summary>
        /// Получаем количество пикселей изображения
        /// </summary>
        /// <param name="image">Изображение для получения разрешения</param>
        /// <returns>Количество пикселей изображения</returns>
        private uint GetImagePixelsCount(BitmapImage image) =>
            (uint)(image.PixelWidth * image.PixelHeight);


        /// <summary>
        /// ВЫполняем конвертацию задачи по поиску хеша в класс информации о дубликате
        /// </summary>
        /// <param name="taskResult">Результат выполнения таски</param>
        /// <param name="id">Идентификатор изображения в списке</param>
        /// <returns>Класс нформации о дубликате</returns>
        private DuplicateImageInfo ConvertTaskResultToDuplicateInfo(CreateHashTask taskResult, int id)
        {
            //Получаем полный путь к картинке
            string path = taskResult.GetFullPath();
            //Грузим целевое изображение
            BitmapImage image = LoadImageByPath(path);
            //Формируем итоговый класс
            DuplicateImageInfo imageInfo = new DuplicateImageInfo() {
                Id = (uint)id,
                Name = taskResult.FileName,
                Hash = taskResult.Hash.GetValueOrDefault(0),
                Path = path,
                ParentFolderName = GetDirectoryName(taskResult.Path),
                Resolution = GetImageResolution(image),
                PixelsCount = GetImagePixelsCount(image)
            };
            //Завершаем поток изображения
            image.StreamSource.Dispose();
            //Возвращаем результат
            return imageInfo;
        }



        /// <summary>
        /// ВЫполняем конвертацию списка тасок в список классов информации о дубликатах
        /// </summary>
        /// <param name="files">Список найденных файлов</param>
        /// <returns>Список классов информации о дубикатах</returns>
        public List<DuplicateImageInfo> ConvertTasksToDuplicateInfo(List<CreateHashTask> files)
        {
            List<DuplicateImageInfo> ex = new List<DuplicateImageInfo>();
            //Проходимся по списку тасок
            for (int i = 0; i < files.Count; i++)
                //Если формирование хеша не завершилось ошибкой
                if (files[i].Status != CreateHashStatuses.Error)
                    //КОнвертируем результат таски в класс информации о
                    //дубликате и добавляем в выходной список
                    ex.Add(ConvertTaskResultToDuplicateInfo(files[i], i));                
            //Возвращаем результат
            return ex;
        }
    }
}
