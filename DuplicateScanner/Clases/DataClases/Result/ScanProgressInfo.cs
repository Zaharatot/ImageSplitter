using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DuplicateScanner.Clases.DataClases.Global.Enums;

namespace DuplicateScanner.Clases.DataClases.Result
{
    /// <summary>
    /// Класс информации о прогрессе сканирования
    /// </summary>
    public class ScanProgressInfo
    {
        /// <summary>
        /// Текущая стадия выполнения сканирования
        /// </summary>
        public ScanStages Stage { get; set; }
        /// <summary>
        /// Количество найденных файлов
        /// </summary>
        public int FilesFinded { get; set; }
        /// <summary>
        /// Количество файлов с ошибками
        /// </summary>
        public int ErrorFilesCount { get; set; }
        /// <summary>
        /// Количество обработанных файлов
        /// </summary>
        public int ProcessedFiles { get; set; }
        /// <summary>
        /// Общее количество файлов для обработки
        /// </summary>
        public int FilesToProcess { get; set; }
        /// <summary>
        /// Количество файлов, данные для которых были загружены
        /// </summary>
        public int LoadedFiles { get; set; }
        /// <summary>
        /// Расчётное оставшееся время
        /// </summary>
        public double? TimeLeft { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="stage">Текущая стадия сканирования</param>
        public ScanProgressInfo(ScanStages stage)
        {
            //Проставляем переданные значения
            Stage = stage;
            //Проставляем дефолтные значения
            FilesFinded = ErrorFilesCount = 
                ProcessedFiles = FilesToProcess = LoadedFiles = 0;
            TimeLeft = null;
        }
    }
}
