using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitterSimpleUI.Content.Clases.DataClases.Progress
{
    /// <summary>
    /// Класс дополнительной информации о прогрессе
    /// </summary>
    public class ProgressAddInfo
    {
        /// <summary>
        /// Заголовок панели дополнительной информации
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// Значение для панели дополнительной информации
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Текстовое значение для контролла
        /// </summary>
        public string GetText => $"{Header} {Value}";



        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="header">Заголовок панели дополнительной информации</param>
        /// <param name="value">Значение для панели дополнительной информации</param>
        public ProgressAddInfo(string header, string value)
        {
            //Протсавляем переданные значения
            Header = header;
            Value = value;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="header">Заголовок панели дополнительной информации</param>
        public ProgressAddInfo(string header)
        {
            //Протсавляем переданные значения
            Header = header;
            //Протсавляем дефолтные значения
            Value = "0";
        }
    }
}
