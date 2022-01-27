using ImageSplitter.Content.Clases.DataClases.Tags;
using ImageSplitter.Content.Clases.WorkClases.Processors.Tags.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.Tags
{
    /// <summary>
    /// Класс работы с изображениями
    /// </summary>
    internal class ImagesWork
    {

        /// <summary>
        /// Класс считывания тегов
        /// </summary>
        private JpegTagReader _tagReader;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImagesWork()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _tagReader = new JpegTagReader();
        }




        /// <summary>
        /// Метод загрузки файлов из папки
        /// </summary>
        /// <param name="path">Путь к папке с файлами</param>
        /// <returns>Список загруженных изображений</returns>
        public List<TaggedImage> LoadFiles(string path)
        {
            //Инициализируем список изображений
            List<TaggedImage> images = new List<TaggedImage>();
            //Получаем информацию о папке
            DirectoryInfo dir = new DirectoryInfo(path);
            //Проходимся по файлам из папки
            foreach (FileInfo file in dir.GetFiles())
            {
                //Если файл с данным расширением можно использовать
                if (_tagReader.IsAllowExtension(file.Extension))
                    //Добавляем картинку в список
                    images.Add(new TaggedImage()
                    {
                        Name = file.Name,
                        Path = file.FullName,
                        //Грузим имена тегов из файла
                        Tags = _tagReader.ReadTags(file.FullName)
                    });
            }
            //Возвращаем результат
            return images;
        }

        /// <summary>
        /// Выполняем сохранение тегов для изображений
        /// </summary>
        /// <param name="images">Список изображений для сохранения тегов</param>
        public void SaveTags(List<TaggedImage> images)
        {
            //Проходимся по списку изображений
            foreach(var image in images)
                //Выполняем сохранение тегов для изображений
                _tagReader.WriteTags(image.Path, image.Tags);
            //Выводим сообщение об успешном завершении действа
            MessageBox.Show("Сохранение тегов успешно завершено");
        }
    }
}
