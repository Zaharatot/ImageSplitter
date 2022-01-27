using ImageSplitter.Content.Clases.DataClases.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.Tags.Service
{
    /// <summary>
    /// Класс загрузчика тегов
    /// </summary>
    public class TagsLoader
    {
        /// <summary>
        /// Путь к файлу хранения тегов
        /// </summary>
        private string _tagsPath;
        /// <summary>
        /// Класс сериализации в XML
        /// </summary>
        XmlSerializer _serializer;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TagsLoader()
        {
            Init();
        }

        /// <summary>
        /// ИНициализатор класса
        /// </summary>
        private void Init()
        {
            //Проставляем дефолтные значения
            _tagsPath = CompilePath();
            _serializer = new XmlSerializer(typeof(List<string>));
        }

        /// <summary>
        /// Формируем путь к файлу тегов
        /// </summary>
        /// <returns>Строка пути к файлу тегов</returns>
        private string CompilePath() => 
            $"{Environment.CurrentDirectory}\\tags.xml";
            

        /// <summary>
        /// Выполняем загрузку списка тегов
        /// </summary>
        /// <returns>Список тегов</returns>
        public List<string> LoadTags()
        {
            //Инициализируем список тегов
            List<string> ex = new List<string>();
            //Если файл существует
            if (File.Exists(_tagsPath))
            {
                //Инициаализируем поток в памяти
                using (FileStream ms = File.OpenRead(_tagsPath))
                    //Десериализуем xml в объект
                    ex = (List<string>)_serializer.Deserialize(ms);
            }
            //Возвращаем результат
            return ex;
        }

        /// <summary>
        /// Выполняем сохранение списка тегов
        /// </summary>
        /// <param name="tags">Список тегов для сохранения</param>
        public void SaveTags(List<string> tags)
        {
            //Инициаализируем поток в памяти
            using (FileStream ms = File.OpenWrite(_tagsPath))
                //Сериализуем класс в xml
                _serializer.Serialize(ms, tags);
        }

    }
}
