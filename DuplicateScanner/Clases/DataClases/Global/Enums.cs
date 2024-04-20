using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScannerLib.Clases.DataClases.Global
{
    /// <summary>
    /// Класс глобальных перечислений
    /// </summary>
    public class Enums
    {

        /// <summary>
        /// Типы сканирования
        /// </summary>
        public enum ScanTypes
        {
            /// <summary>
            /// Оба вида проверок
            /// </summary>
            Both = 0,
            /// <summary>
            /// Проверка по ДКП-хешу
            /// </summary>
            DcpScan = 1,
            /// <summary>
            /// Проверка по лайновой версии ДКП-хеша
            /// </summary>
            LinedDcpScan = 2
        }


        /// <summary>
        /// Перечисление статусов дубликата
        /// </summary>
        public enum DuplicateStates
        {
            /// <summary>
            /// Дубликат был добавлен в список
            /// </summary>
            Added = 0,
            /// <summary>
            /// Для файла были корректно вычислены значения
            /// </summary>
            Calculated = 1,
            /// <summary>
            /// Ошибка загрузки изображения
            /// </summary>
            ImageLoadError = 2,
            /// <summary>
            /// Ошибка вычисления хешей
            /// </summary>
            CalculateHashesError = 3,
        }

        /// <summary>
        /// Перечисление стадий сканирования
        /// </summary>
        public enum ScanStages
        {
            /// <summary>
            /// Поиск файлов изображений по указанному пути
            /// </summary>
            FindFiles = 0,
            /// <summary>
            /// Генерация хешей для найденных файлов
            /// </summary>
            HashGeneration = 1,
            /// <summary>
            /// Сохранение данных о хешах
            /// </summary>
            SavingData = 2,
            /// <summary>
            /// Поиск дубликатов
            /// </summary>
            DuplicateFind = 3,

            /// <summary>
            /// Поиск устаревших записей
            /// </summary>
            FindOldDuplicates = 4,
            /// <summary>
            /// Удаление устаревших записей
            /// </summary>
            RemoveOldDuplicates = 5,
        }
    }
}
