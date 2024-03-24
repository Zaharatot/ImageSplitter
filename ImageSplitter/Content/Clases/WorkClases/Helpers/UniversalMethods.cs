using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static ImageSplitter.Content.Clases.DataClases.Global.Enums;

namespace ImageSplitter.Content.Clases.WorkClases.Helpers
{
    /// <summary>
    /// Класс универсальных методов работы с контроллами
    /// </summary>
    internal static class UniversalMethods
    {

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
    }
}
