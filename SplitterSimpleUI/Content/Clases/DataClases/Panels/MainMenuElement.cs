using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SplitterSimpleUI.Content.Clases.DataClases.Global.Enums;

namespace SplitterSimpleUI.Content.Clases.DataClases.Panels
{
    /// <summary>
    /// Класс элемента главного меню
    /// </summary>
    public class MainMenuElement
    {

        /// <summary>
        /// Тип элемента меню
        /// </summary>
        public MainMenuElements Element { get; set; }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public MainMenuElement()
        {
            //Инициализируем список элементов
            Element = MainMenuElements.SelectPath;
        }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="element">Список элементов для инициализации</param>
        public MainMenuElement(MainMenuElements element)
        {
            //Проставляем переданные значения
            Element = element;
        }
    }
}
