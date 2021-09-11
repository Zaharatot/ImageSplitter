using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImageSplitter.Content.Clases.WorkClases.Addition
{
    /// <summary>
    /// Класс генерации имён папок
    /// </summary>
    internal class FolderNameGrnerator
    {
        /// <summary>
        /// Константа с маской имени генерируемой папки
        /// </summary>
        private const string FOLDER_NAME_MASK = "New Folder{0}";


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FolderNameGrnerator()
        {

        }

        /// <summary>
        /// Создаём валидное имя для папки
        /// </summary>
        /// <param name="path">Путь к родительской папке</param>
        /// <returns>Строка с новым именем папки</returns>
        public string CreateFolderName(string path)
        {
            //Ставим дефолтное имя
            string name = string.Format(FOLDER_NAME_MASK, ""); 
            //Проставляем идентификатор папки
            int id = 0;
            //Пока имя данной папки дублирует уже имеющиеся
            while (Directory.Exists($"{path}{name}"))
                //Генерируем новое имя папки и переходим к новому идентификатору
                name = string.Format(FOLDER_NAME_MASK, $" ({id++})");
            //Возвращаем результат
            return name;
        }

    }
}
