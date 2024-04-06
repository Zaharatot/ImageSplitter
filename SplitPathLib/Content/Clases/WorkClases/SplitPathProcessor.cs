using SplitPathWindowLib.Content.Windows;
using SplitterDataLib.DataClases.Global.Split;
using SplitterSimpleUI.Content.Clases.WorkClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitPathWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс работы с путём сплита
    /// </summary>
    internal class SplitPathProcessor
    {

        /// <summary>
        /// Информация о пути сплита
        /// </summary>
        public SplitPathsInfo SplitPath { get; set; }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SplitPathProcessor()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем дефолтное значение пути сплита
            SplitPath = new SplitPathsInfo();
        }




        /// <summary>
        /// Метод обновления пути сплита
        /// </summary>
        /// <returns>Нвоый путь сплита</returns>
        public SplitPathsInfo UpdateSplitPath()
        {
            //Инициализируем окно для выбора путей сплита
            SelectPathsWinodw selectPathWindow = new SelectPathsWinodw();
            //Проставляем в окно текущий путь сплита
            selectPathWindow.SplitPath = SplitPath;
            //Если работа с окном завершилось успехом
            if (selectPathWindow.ShowDialog().GetValueOrDefault(false))
                //Втыкаем его путь сплита в текущее значение
                SplitPath = selectPathWindow.SplitPath;
            //Возвращаем текущий путь сплита
            return SplitPath;
        }
    }
}
