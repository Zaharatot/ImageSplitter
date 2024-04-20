using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesWindowLib.Content.Clases.DataClases
{
    /// <summary>
    /// Класс перечислений с типами сообщений
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Типы месседжбоксов
        /// </summary>
        public enum MessageBoxTypes
        {
            /// <summary>
            /// Сообщение с уведомлением пользователя
            /// </summary>
            OkMessage,
            /// <summary>
            /// Сообщение с запросом пользователю
            /// </summary>
            YesNoMessage,
        }

        /// <summary>
        /// Уровни сообщений
        /// </summary>
        public enum MessageBoxLevels
        {
            /// <summary>
            /// Действие успешно завершено
            /// </summary>
            Done,
            /// <summary>
            /// Предупреждение о проблеме
            /// </summary>
            Warning,
            /// <summary>
            /// Ошибка действия
            /// </summary>
            Error,
            /// <summary>
            /// Запрос подтверждения
            /// </summary>
            Question,
        }



        /// <summary>
        /// Перечисление типов сообщений в окнах
        /// </summary>
        public enum MessageBoxMessages
        {
            /// <summary>
            /// Тестовое всплывающее сообщение
            /// </summary>
            Test,

            #region DuplicateScan

            /// <summary>
            /// Успешное удаление дубликатов
            /// </summary>
            DuplicateRemoveComplete,
            /// <summary>
            /// Запрос на удаление устаревших записей о дубликатах
            /// </summary>
            DuplicateRemoveRequest,
            /// <summary>
            /// Успешное удаление устаревших записей о дубликатах
            /// </summary>
            DuplicateRemoveOldElements,
            /// <summary>
            /// Не было найдено дубликатов
            /// </summary>
            DuplicateScanNotFound,
            /// <summary>
            /// Запрос на удаление выбранных дубликатов изображений
            /// </summary>
            DuplicateImagesRemoveRequest,

            #endregion

            #region ImageSplit

            /// <summary>
            /// Поиск изображений для раскладки был успешно завершён
            /// </summary>
            ImageSplitScanComplete,
            /// <summary>
            /// Запрос удаления папки из списка
            /// </summary>
            ImageSplitRemoveFolderRequest,

            #endregion

            #region FileRename

            /// <summary>
            /// Переименование файлов успешно завершено
            /// </summary>
            FileRenameComplete,

            #endregion

            #region FileSplit

            /// <summary>
            /// Возврат файлов успешно завершён
            /// </summary>
            FileReturnComplete,
            /// <summary>
            /// Разделение файлов успешно завершено
            /// </summary>
            FileSplitComplete,

            #endregion
        }

        /// <summary>
        /// Перечисление типов всплывающех сообщений
        /// </summary>
        public enum PopupMessages
        {
            /// <summary>
            /// Тестовое всплывающее сообщение
            /// </summary>
            Test,

            #region DuplicateScan

            /// <summary>
            /// Сообщение выбора последнего элемента в группах
            /// </summary>
            DuplicatesSelectLastGroup,


            #endregion

            #region ImageSplit

            #endregion
        }
    }
}
