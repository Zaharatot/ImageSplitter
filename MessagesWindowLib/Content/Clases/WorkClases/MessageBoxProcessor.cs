using MessagesWindowLib.Content.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MessagesWindowLib.Content.Clases.DataClases.Enums;

namespace MessagesWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс обработки окон сообщений
    /// </summary>
    internal static class MessageBoxProcessor
    {


        /// <summary>
        /// Метод отображения сообщения
        /// </summary>
        /// <param name="type">Тип сообщения</param>
        /// <param name="level">Уровень сообщения</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="addInfo">Дополнительная информация</param>
        /// <returns>Флаг результата</returns>
        public static bool ShowMessageBox(MessageBoxTypes type, MessageBoxLevels level, 
            MessageBoxMessages message, string addInfo)
        {
            //Инициализируем окно всплывающего сообщения
            MessageBoxWindow messageBox = new MessageBoxWindow();
            //Втыкаем в окно информацию
            messageBox.SetMessageBoxInfo(type, level, message, addInfo);
            //Вызываем окно как диалоговое и возмращаем результат
            return messageBox.ShowDialog().GetValueOrDefault(false);
        }
    }
}
