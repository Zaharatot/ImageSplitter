using FilesRenameWindowLib.Content.Clases.WorkClases;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesRenameWindowLib
{
    /// <summary>
    /// Фасадный класс библиотеки выполнения переименования
    /// </summary>
    public class FilesRenameFasade
    {
        /// <summary>
        /// Класс выполнения переименования файлов
        /// </summary>
        private FileRenameProcessor _fileRenameProcessor;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FilesRenameFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем класс переименования
            _fileRenameProcessor = new FileRenameProcessor();
        }



        /// <summary>
        /// Метод выполнения переименования файлов
        /// </summary>
        /// <param name="info">Информация о путях для переименования</param>
        public void RenameFiles(SplitPathsInfo info) =>
            //Вызываем внутренний метод
            _fileRenameProcessor.RenameFiles(info);
    }
}
