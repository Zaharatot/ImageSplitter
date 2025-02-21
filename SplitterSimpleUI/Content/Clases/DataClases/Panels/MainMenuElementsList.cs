using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SplitterSimpleUI.Content.Clases.DataClases.Global.Enums;

namespace SplitterSimpleUI.Content.Clases.DataClases.Panels
{
    /// <summary>
    /// Класс списка элементов главного меню
    /// </summary>
    public class MainMenuElementsList
    {

        /// <summary>
        /// Список элементов основного меню
        /// </summary>
        public List<MainMenuElement> Elements { get; set; }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public MainMenuElementsList()
        {
            //Инициализируем список элементов
            Elements = new List<MainMenuElement>();
        }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="elements">Список элементов для инициализации</param>
        public MainMenuElementsList(List<MainMenuElement> elements)
        {
            //Проставляем переданные значения
            Elements = elements;
        }
    }
}
