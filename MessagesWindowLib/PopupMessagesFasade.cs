using MessagesWindowLib.Content.Clases.WorkClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static MessagesWindowLib.Content.Clases.DataClases.Enums;

namespace MessagesWindowLib
{

    /// <summary>
    /// Фасадный класс библиотеки работы с сообщениями
    /// </summary>
    public class PopupMessagesFasade
    {
        /// <summary>
        /// Класс обработки всплывающих сообщений
        /// </summary>
        private PopupMessagesProcessor _popupMessagesProcessor;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public PopupMessagesFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _popupMessagesProcessor = new PopupMessagesProcessor();
        }



        /// <summary>
        /// Метод добавления контролла всплывающего сообщения
        /// </summary>
        /// <param name="parent">Родительский контролл для всплывающего сообщения</param>
        public void AddPopupControl(Grid parent) =>
            //Вызываем внутренний метод
            _popupMessagesProcessor.AddPopupControl(parent);

        /// <summary>
        /// Метод отображения всплывающего сообщения
        /// </summary>
        /// <param name="message">Тип сообщения для отображения</param>
        /// <param name="addInfo">Дополнительная информация</param>
        public void ShowPopupMessage(PopupMessages message, string addInfo = "") =>
            //Вызываем внутренний метод
            _popupMessagesProcessor.ShowMessage(message, addInfo);

    }
}
