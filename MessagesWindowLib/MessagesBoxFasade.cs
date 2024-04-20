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
    public class MessagesBoxFasade
    {


        /// <summary>
        /// Метод отображения информации о сообщении
        /// </summary>
        /// <param name="type">Тип сообщения</param>
        /// <param name="level">Уровень сообщения</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="addInfo">Дополнительная информация</param>
        public void SetMessageBoxInfo(MessageBoxTypes type, MessageBoxLevels level, MessageBoxMessages message, string addInfo = "") =>
            //Передаём во внутренний метод
            MessageBoxProcessor.ShowMessageBox(type, level, message, addInfo);


        /// <summary>
        /// Метод отображения сообщения с запросом
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="addInfo">Дополнительная информация</param>
        /// <returns>Флаг результата</returns>
        public static bool ShowMessageBoxQuestion(MessageBoxMessages message, string addInfo = "") =>
            //Передаём во внутренний метод
            MessageBoxProcessor.ShowMessageBox(MessageBoxTypes.YesNoMessage, 
                MessageBoxLevels.Question, message, addInfo);

        /// <summary>
        /// Метод отображения сообщения с ошибкой
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="addInfo">Дополнительная информация</param>
        public static void ShowMessageBoxError(MessageBoxMessages message, string addInfo = "") =>
            //Передаём во внутренний метод
            MessageBoxProcessor.ShowMessageBox(MessageBoxTypes.OkMessage,
                MessageBoxLevels.Error, message, addInfo);

        /// <summary>
        /// Метод отображения сообщения с уведомлением
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="addInfo">Дополнительная информация</param>
        public static void ShowMessageBoxWarning(MessageBoxMessages message, string addInfo = "") =>
            //Передаём во внутренний метод
            MessageBoxProcessor.ShowMessageBox(MessageBoxTypes.OkMessage,
                MessageBoxLevels.Warning, message, addInfo);

        /// <summary>
        /// Метод отображения сообщения с успехом
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="addInfo">Дополнительная информация</param>
        public static void ShowMessageBoxDone(MessageBoxMessages message, string addInfo = "") =>
            //Передаём во внутренний метод
            MessageBoxProcessor.ShowMessageBox(MessageBoxTypes.OkMessage,
                MessageBoxLevels.Done, message, addInfo);
    }
}
