using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static SplitterSimpleUI.Content.Clases.DataClases.Global.Delegates;

namespace SplitterSimpleUI.Content.Clases.DataClases.HotKey
{
    /// <summary>
    /// Класс информации о хоткеях для окна
    /// </summary>
    public class WindowHotKeys
    {
        /// <summary>
        /// Флаг завершения обработки на нажатии хоткея
        /// </summary>
        public bool IsHandled { get; set; }
        /// <summary>
        /// Метод, который необходимо вызывать при нажатии клавиши кроме хоткея
        /// </summary>
        public KeyPressEventHandler IsOtherPressMethod { get; set; }
        /// <summary>
        /// Список горячих клавишь для окна
        /// </summary>
        public List<HotKeyInfo> HotKeys { get; set; }

        /// <summary>
        /// Флаг наличия метода дополнительной обработки
        /// </summary>
        public bool IsContainOtherProcessMethod => (IsOtherPressMethod != null);


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="isHandled">Флаг завершения обработки на нажатии хоткея</param>
        /// <param name="isOtherPressMethod">Метод, который необходимо вызывать при нажатии клавиши кроме хоткея</param>
        public WindowHotKeys(bool isHandled = false, KeyPressEventHandler isOtherPressMethod = null)
        {
            //Проставляем переданные значения
            IsHandled = isHandled;
            IsOtherPressMethod = isOtherPressMethod;
            //Инициализируем список хоткеев
            HotKeys = new List<HotKeyInfo>();
        }

        /// <summary>
        /// Метод получения хоткея по нажатым клавишам
        /// </summary>
        /// <param name="isContrl">Флаг нажатия клавиши Ctrl</param>
        /// <param name="key">Нажатая клавиша</param>
        /// <returns>Класс хоткея, или null</returns>
        public HotKeyInfo GetPressedHotKey(Key key, bool isContrl) =>
            //Получаем из списка хоткей, который соответствует нажатой клавише и статусу нажатия на Ctrl
            HotKeys.FirstOrDefault(hotkey => hotkey.Key == key && hotkey.isContrl == isContrl);
    }
}
