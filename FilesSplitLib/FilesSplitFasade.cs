using FilesSplitWindowLib.Content.Clases.WorkClases;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesSplitWindowLib
{
    /// <summary>
    /// Фасадный класс переноса файлов
    /// </summary>
    public class FilesSplitFasade
    {
        /// <summary>
        /// Класс обработки сплита
        /// </summary>
        private FileSplitProcessor _fileSplitProcessor;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FilesSplitFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем класс обработки сплита
            _fileSplitProcessor = new FileSplitProcessor();
        }


        /// <summary>
        /// Метод выполнения сплита файлов
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public void SplitFiles(SplitPathsInfo info) =>
            //Вызываем внутренний метод
            _fileSplitProcessor.SplitFiles(info);
    }
}
