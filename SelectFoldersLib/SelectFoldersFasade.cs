using SelectFoldersWindowLib.Content.Clases.WorkClases;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SelectFoldersWindowLib
{
    /// <summary>
    /// Фасадный класс библиотеки выбора папок
    /// </summary>
    public class SelectFoldersFasade
    {
        /// <summary>
        /// Основной класс обработки выбора папок
        /// </summary>
        private SelectFoldersProcessor _selectFoldersProcessor;



        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SelectFoldersFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _selectFoldersProcessor = new SelectFoldersProcessor();
        }



        /// <summary>
        /// Метод запуска выбора папок
        /// </summary>
        /// <param name="folders">Список папок для обработки</param>
        /// <returns>Список выбранных папок</returns>
        public List<TargetFolderInfo> SelectFolders(List<TargetFolderInfo> folders) =>
            //Вызываем внутренний метод
            _selectFoldersProcessor.SelectFolders(folders);

    }
}
