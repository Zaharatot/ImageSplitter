using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static SplitterDataLib.DataClases.Global.Delegates;

namespace SplitterSimpleUI.Content.Clases.DataClases.HotKey
{
    /// <summary>
    /// Класс информации о хоткее
    /// </summary>
    public class HotKeyInfo
    {
        /// <summary>
        /// Клавиша для обработки
        /// </summary>
        public Key Key { get; set; }
        /// <summary>
        /// Флаг надатия кнопки Contrl
        /// </summary>
        public bool isContrl { get; set; }
        /// <summary>
        /// Метод, который необходимо вызывать при нажатии хоткея
        /// </summary>
        public EmptyEventHandler Method { get; set; }



        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="key">Нажатая клавиша</param>
        /// <param name="isCtrl">Флаг нажатия кнопки Ctrl</param>
        /// <param name="isHandled">Флаг завершения обработки на нажатии хоткея</param>
        /// <param name="method">Вызываемый по хоткею метод</param>
        public HotKeyInfo(Key key, EmptyEventHandler method, bool isCtrl = false)
        {
            //Проставляем переданные значения
            Key = key;
            isContrl = isCtrl;
            Method = method;
        }

    }
}
