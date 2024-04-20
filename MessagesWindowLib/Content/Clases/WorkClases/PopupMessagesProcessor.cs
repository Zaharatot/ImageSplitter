using MessagesWindowLib.Content.Controls;
using SplitterResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static MessagesWindowLib.Content.Clases.DataClases.Enums;

namespace MessagesWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс обработки всплывающих сообщений
    /// </summary>
    internal class PopupMessagesProcessor
    {
        /// <summary>
        /// Панель всплывающего сообщения
        /// </summary>
        private PopupMessagePanel _popupMessagePanel;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public PopupMessagesProcessor()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем панель всплывающего сообщения
            _popupMessagePanel = new PopupMessagePanel();
        }


        /// <summary>
        /// Метод получения текста всплывающего сообщения
        /// </summary>
        /// <param name="message">Тип сообщения для отображения</param>
        /// <returns>Текст всплывающего сообщения</returns>
        private string GetMessageText(PopupMessages message) =>
            //Загружаем из ресурсов строку текста всплывающего сообщения
            ResourceLoader.LoadString($"Text_PopupMessage_{message}");




        /// <summary>
        /// Метод добавления 
        /// </summary>
        /// <param name="parent">Родительский контролл для всплывающего сообщения</param>
        public void AddPopupControl(Grid parent)
        {
            //Отображаем контролл поверх всего
            Grid.SetZIndex(_popupMessagePanel, 99);
            //Проставляем позицию контроллу внизу панели
            _popupMessagePanel.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            _popupMessagePanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            //Растягиваем контролл на все столбцы и строки
            Grid.SetColumnSpan(_popupMessagePanel, 99);
            Grid.SetRowSpan(_popupMessagePanel, 99);
            //Добавляем контролл на панель
            parent.Children.Add(_popupMessagePanel);
        }


        /// <summary>
        /// Метод отображения сообщения
        /// </summary>
        /// <param name="message">Тип сообщения для отображения</param>
        /// <param name="addInfo">Дополнительная информация</param>
        public void ShowMessage(PopupMessages message, string addInfo)
        {
            //Получаем текст сообщения
            string messageText = GetMessageText(message);
            //Добавляем в текст сообщения доп. инфу и отображаем его
            _popupMessagePanel.ShowMessage(string.Format(messageText, addInfo));
        }
    }
}
