using DuplicateScannerLib.Clases.DataClases;
using DuplicateScannerLib.Clases.DataClases.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DuplicateScannerLib.Clases.WorkClases.Files
{
    /// <summary>
    /// Класс выполнения загрузки и сохранения данных о дубликатах
    /// </summary>
    internal class DuplicateInfoLoader
    {
        /// <summary>
        /// Класс информации о дубликатах
        /// </summary>
        private const string DUPLICATES_INFO_FILE_NAME = "duplicates.xml";


        /// <summary>
        /// Класс сериализции XML
        /// </summary>
        private XmlSerializer _serializer;

        /// <summary>
        /// Строка пути к файлу сохранения
        /// </summary>
        private string _path;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicateInfoLoader()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Формируем путь к файлу для сохранения
            _path = $"{Environment.CurrentDirectory}\\{DUPLICATES_INFO_FILE_NAME}";
            //Инициализируем класс серивализации
            _serializer = new XmlSerializer(typeof(List<DuplicateInfo>));
        }




        /// <summary>
        /// Метод загрузки списка дубликатов
        /// </summary>
        /// <returns>Загруженный список дубликатов</returns>
        public List<DuplicateInfo> LoadDuplicates()
        {
            //Инициализируем пустой список по дефолту
            List<DuplicateInfo> duplicates = new List<DuplicateInfo>();
            try
            {
                //Если есть путь к файлу
                if (File.Exists(_path))
                {
                    //Открываем поток для чтения из файла
                    using(FileStream stream = File.OpenRead(_path))
                        //Выполняем десериализацию контента файла в выходную переменную
                        duplicates = (List<DuplicateInfo>)_serializer.Deserialize(stream);
                }
            }
            catch { }
            //Возвращаем результат
            return duplicates;
        }

        /// <summary>
        /// Метод выполнения сохранения дубликатов
        /// </summary>
        /// <param name="duplicates">Список дубликатов сохранения</param>
        public void SaveDuplicates(List<DuplicateInfo> duplicates)
        {
            try 
            {
                //Удаляем файл
                if (File.Exists(_path))
                    //Если он до этого существовал
                    File.Delete(_path);
                //Открываем файл для записи
                using (FileStream stream = File.OpenWrite(_path))
                    //Сериализуем и записываем в файл данные
                    _serializer.Serialize(stream, duplicates);
            }
            catch { }
        }


    }
}
