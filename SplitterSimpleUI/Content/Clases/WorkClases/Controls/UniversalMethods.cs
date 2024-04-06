using SplitterResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static SplitterDataLib.DataClases.Global.Enums;

namespace SplitterSimpleUI.Content.Clases.WorkClases.Controls
{
    /// <summary>
    /// Класс универсальных методов работы с контроллами
    /// </summary>
    public static class UniversalMethods
    {

        /// <summary>
        /// Метод обработки строкового значения
        /// </summary>
        /// <param name="value">Строка для обработки</param>
        /// <returns>Обработанная строка</returns>
        private static string ProcessValue(string value) =>
            //Если строка пустая - втыкаем вместо неё прочерк
            string.IsNullOrEmpty(value) ? ResourceLoader.LoadString("Text_EmptyValue") : value;




        /// <summary>
        /// Метод вставки контента в тултип
        /// </summary>
        /// <param name="elem">Тултип для вставки</param>
        /// <param name="content">КОнтент для вставки</param>
        public static void SetTooltipContent(ToolTip elem, string content)
        {
            //Втыкаем контент
            elem.Content = content;
            //Обновляем видимость, 
            elem.Visibility = (string.IsNullOrEmpty(content) ?
                //Если текста подсказки нет - то и тултип отображать не нужно
                Visibility.Collapsed : Visibility.Visible);
        }

        /// <summary>
        /// Метод простановки статуса чекбокса п офлагу
        /// </summary>
        /// <param name="isChecked">Флаг включения чекбокса</param>
        /// <returns>Статус чекбокса</returns>
        public static ComboCheckBoxStates GetComboCheckBoxStateByFlag(bool isChecked) =>
            (isChecked ? ComboCheckBoxStates.Checked : ComboCheckBoxStates.Unchecked);

        /// <summary>
        /// Метод получения текста из Run в буфер обмена
        /// </summary>
        /// <param name="sender">Контролл для получения текста</param>
        public static void GetRunTextToClipboard(object sender)
        {
            //Типизируем элемент
            Run element = (sender as Run);
            //Если он активен
            if (element.IsEnabled)
                //Втыкаем его текст в буфер обмена
                Clipboard.SetText(element.Text);
        }


        /// <summary>
        /// Вставляем текст в Run
        /// </summary>
        /// <param name="elem">Элемент для вставки текста</param>
        /// <param name="text">Текст для вставки</param>
        public static void SetRunTextOrEmpty(Run elem, string text)
        {
            //Вставляем текст в контролл
            elem.Text = ProcessValue(text);
            //Дизейблим контролл, если текста нету
            elem.IsEnabled = !string.IsNullOrEmpty(text);
        }

    }
}
