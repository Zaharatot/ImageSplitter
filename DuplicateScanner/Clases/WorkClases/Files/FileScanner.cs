using DuplicateScannerLib.Clases.DataClases;
using DuplicateScannerLib.Clases.DataClases.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScannerLib.Clases.WorkClases.Files
{
    /// <summary>
    /// Класс сканирования списка файлов
    /// </summary>
    internal class FileScanner
    {
        /// <summary>
        /// Массив поддерживаемых расширений для изображений
        /// </summary>
        private readonly string[] _imageExtensions = new string[] {
                ".bmp", ".png", ".jpg", ".jpeg", ".gif"
            };


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileScanner()
        {

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
        /// Метод выполнения рекурсивного сканирования файлов
        /// </summary>
        /// <param name="parent">Информация о родительской папке</param>
        /// <param name="duplicates">Список дубликатов</param>
        private void ScanFilesRecurse(DirectoryInfo parent, ref List<DuplicateInfo> duplicates)
        {
            //Проходимся по дочерним файлам
            foreach (FileInfo file in parent.GetFiles())
                //Если файл является поддерживаемой картинкой
                if (FileIsImage(file))
                    //Добавляем в список класс инфомрации о дубликате
                    duplicates.Add(new DuplicateInfo(file));
            //Проходимсся по дочерним папкам
            foreach (DirectoryInfo dir in parent.GetDirectories())
                //Для каждой из них вызываем рекурсивно этот метод
                ScanFilesRecurse(dir, ref duplicates);
        }



        /// <summary>
        /// Метод выполнения сканиварония файлов
        /// </summary>
        /// <param name="parentPath">Путь к родительской папке</param>
        /// <returns>Список классов дубликатов для файлов</returns>
        public List<DuplicateInfo> ScanFiles(string parentPath)
        {
            //Инициализируем выходной список дубликатов
            List<DuplicateInfo> duplicates = new List<DuplicateInfo>();
            //Инициализируем класс информации о родительской директории
            DirectoryInfo parent = new DirectoryInfo(parentPath);
            //Вызываем метод поиска дочерних элементов
            ScanFilesRecurse(parent, ref duplicates);
            //Возвращаем найденные файлы
            return duplicates;
        }
    }
}
