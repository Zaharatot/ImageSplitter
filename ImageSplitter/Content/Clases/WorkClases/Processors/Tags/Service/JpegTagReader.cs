using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.Tags.Service
{
    /// <summary>
    /// Класс работы с тегами в Jpeg
    /// </summary>
    internal class JpegTagReader
    {
        /// <summary>
        /// Список поддерживаемых расширений
        /// </summary>
        private List<string> _jpegExtensions;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public JpegTagReader()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Получаем список поддерживаемых расширений
            _jpegExtensions = GetExtensions();
        }

        /// <summary>
        /// Метод получения списка поддерживаемых расширений
        /// </summary>
        /// <returns>Список расширений</returns>
        private List<string> GetExtensions() => 
            new List<string>() { ".jpg", ".jpeg" };

        /// <summary>
        /// Выполняем сохранение изменённого файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="encoder">Энкодер с изменённым кадром</param>
        private void SaveUpdatedFile(string path, JpegBitmapEncoder encoder)
        {
            //Инициализируем поток для записи
            using (FileStream output = File.OpenWrite(path))
            {
                //Сохраняем в него контент файла
                encoder.Save(output);
                //Обновляем файл
                output.Flush();
            }
        }

        /// <summary>
        /// Получаем метаданные изображения
        /// </summary>
        /// <param name="path">Путь к файлу изображения</param>
        /// <param name="frame">Кадр с изображением</param>
        /// <param name="metadata">Метаданные изображения</param>
        private void LoadImageMetadata(string path, out BitmapFrame frame, out BitmapMetadata metadata)
        {
            //Получаем потокцелевого изображения
            using (FileStream file = File.OpenRead(path))
            {
                //Получаем кадр изображения
                frame = BitmapFrame.Create(file);
                //Получаем метаданные из кадра
                metadata = (BitmapMetadata)frame.Metadata.Clone();
            }
        }



        /// <summary>
        /// Проверка наличия расширения файла в списке дозволенных
        /// </summary>
        /// <param name="extension">Строка расширения файла</param>
        /// <returns>True - расширение можно использовать</returns>
        public bool IsAllowExtension(string extension) =>
           _jpegExtensions.Contains(extension);

        /// <summary>
        /// Выполняем считывание тегов файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Считанный список тегов</returns>
        public List<string> ReadTags(string path)
        {
            //Получаем метаданные изображения
            LoadImageMetadata(path, out BitmapFrame frame, out BitmapMetadata metadata);
            //Получаем список тегов из метаданных
            return metadata.Keywords.ToList();
        }

        /// <summary>
        /// Выполняем запись тегов в файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="tags">Список тегов файла</param>
        public void WriteTags(string path, List<string> tags)
        {
            //Инициализируем энкодер для изображений
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            //Получаем метаданные изображения
            LoadImageMetadata(path, out BitmapFrame frame, out BitmapMetadata metadata);
            //Инициализируем коллекцию из списка тегов, и втыкаем в метаданные
            metadata.Keywords = new ReadOnlyCollection<string>(tags);
            //Добавляем кадр, созданный из оригинального изображения в энкодер
            encoder.Frames.Add(BitmapFrame.Create(frame, frame.Thumbnail, metadata, frame.ColorContexts));
            //Сохраняем обновлённый файл
            SaveUpdatedFile(path, encoder);
        }
    }
}
