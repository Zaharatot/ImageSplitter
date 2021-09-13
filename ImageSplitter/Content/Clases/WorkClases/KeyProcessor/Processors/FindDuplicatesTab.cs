using ImageSplitter.Content.Clases.DataClases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.WorkClases.KeyProcessor.Processors
{
    /// <summary>
    /// Класс обработки нажатия на хоткей, для вкладки поиска дубликатов
    /// </summary>
    internal class FindDuplicatesTab : IHotKeyProcessor
    {
        /// <summary>
        /// Идентификатор вкладки сплита изображений
        /// </summary>
        public int TabId => 3;


        /// <summary>
        /// Ссылка на основной рабочий класс
        /// </summary>
        private MainWork _mainWork;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="mainWork">Ссылка на основной рабочий класс</param>
        public FindDuplicatesTab(MainWork mainWork)
        {
            //Проставляем переданное значение
            _mainWork = mainWork;

        }


        /// <summary>
        /// Метод обработки нажатия на сочетание клавиши действия и Ctrl
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>True - нажатие было обработано</returns>
        public bool ProcessControlKeys(Key key)
        {
            //Никак не обрабатывается
            return false;
        }

        /// <summary>
        /// Метод обработки нажатия на клавишу действия
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>True - нажатие было обработано</returns>
        public bool ProcessKeys(Key key)
        {
            //Никак не обрабатывается
            return false;
        }
    }
}
