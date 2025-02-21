using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitterSimpleUI.Content.Clases.DataClases.Global
{
    /// <summary>
    /// Список глобальных перечислений
    /// </summary>
    public class Enums
    {

        /// <summary>
        /// Статусы комбинированного чекбокса
        /// </summary>
        public enum ComboCheckBoxStates
        {
            /// <summary>
            /// Включен
            /// </summary>
            Checked = 0,
            /// <summary>
            /// Выключен
            /// </summary>
            Unchecked = 1,
            /// <summary>
            /// Частично включен
            /// </summary>
            Partial = 2
        }

        /// <summary>
        /// Элементы основного меню
        /// </summary>
        public enum MainMenuElements
        {
            /// <summary>
            /// Выбор пути
            /// </summary>
            SelectPath,
            /// <summary>
            /// Обзор древа
            /// </summary>
            TreeView,
            /// <summary>
            /// Поиск изображений для сплита
            /// </summary>
            SplitScanImages,
            /// <summary>
            /// Сплит файлов
            /// </summary>
            FilesSplit,
            /// <summary>
            /// Иконка переименования файлов
            /// </summary>
            RenameFiles,
            /// <summary>
            /// Иконка поиска дубликатов
            /// </summary>
            DuplicateScan,

        }
    }
}
