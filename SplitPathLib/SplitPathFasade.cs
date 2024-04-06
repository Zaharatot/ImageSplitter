using SplitPathWindowLib.Content.Clases.WorkClases;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitPathWindowLib
{
    /// <summary>
    /// Фасадный класс библиотеки выбора пути сплита
    /// </summary>
    public class SplitPathFasade
    {
        /// <summary>
        /// Класс работы с путём сплита
        /// </summary>
        private SplitPathProcessor _splitPathProcessor;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SplitPathFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _splitPathProcessor = new SplitPathProcessor();
        }




        /// <summary>
        /// Метод обновления пути сплита
        /// </summary>
        /// <returns>Нвоый путь сплита</returns>
        public SplitPathsInfo UpdateSplitPath() =>
            //Вызываем внутренний метод
            _splitPathProcessor.UpdateSplitPath();

    }
}
