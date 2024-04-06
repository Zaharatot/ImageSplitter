using FilesRenameWindowLib.Content.Windows;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesRenameWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс работы с переименованием файла
    /// </summary>
    internal class FileRenameProcessor
    {
        /// <summary>
        /// Класс выполнения переименования файлов по маске
        /// </summary>
        private FileRenamer _fileRenamer;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileRenameProcessor()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем класс переименования
            _fileRenamer = new FileRenamer();
        }


        /// <summary>
        /// Метод выполнения переименования файлов
        /// </summary>
        /// <param name="info">Информация о путях для переименования</param>
        public void RenameFiles(SplitPathsInfo info)
        {
            //Инициализируем окно переименования
            FilesRenameWindow filesRenameWindow = new FilesRenameWindow();
            //Если работа с окном закончилась успехом
            if (filesRenameWindow.ShowDialog().GetValueOrDefault(false))
                //Выполняем переименование файлов
                _fileRenamer.RenameFiles(info, filesRenameWindow.RenameMask);
        }
    }
}
