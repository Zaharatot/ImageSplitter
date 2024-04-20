using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static SplitterSimpleUI.Content.Clases.DataClases.Global.Enums;

namespace SplitterSimpleUI.Content.Clases.DataClases.Global
{
    /// <summary>
    /// Класс глобальных делегатов событий
    /// </summary>
    public class Delegates
    {

        /// <summary>
        /// Делегат события обновления статуса чекбокса
        /// </summary>
        /// <param name="state">Новый статус чекбокса</param>
        public delegate void CheckBoxUpdateStateEventHandler(ComboCheckBoxStates state);

        /// <summary>
        /// Делегат события изменения текстового контента
        /// </summary>
        /// <param name="text">ИЗменённый текст</param>
        public delegate void ChangeTextEventHandler(string text);

        /// <summary>
        /// Делегат события нажатия клавиши
        /// </summary>
        /// <param name="key">Нажатая клавиша</param>
        public delegate void KeyPressEventHandler(Key key);
    }
}
