using DuplicateScanner.Clases.DataClases.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanner.Clases.DataClases.Global
{
    /// <summary>
    /// Класс глобальных делегатов событий
    /// </summary>
    public class Delegates
    {
        /// <summary>
        /// Делегат события обновления информации о прогрессе сканирования
        /// </summary>
        /// <param name="info">Информация о прогрессе сканирования</param>
        public delegate void UpdateScanInfoEventHandler(ScanProgressInfo info);
        /// <summary>
        /// Делегат события завершения сканирования
        /// </summary>
        /// <param name="result">Список результатов поиска дубликатов</param>
        public delegate void CompleteScanEventHandler(List<FindResult> result);

        /// <summary>
        /// Делегат события обновления информации о прогрессе обработки выбранных файлов
        /// </summary>
        /// <param name="info">Информация о прогрессе удаления выбранных файлов</param>
        public delegate void UpdateProgressInfoEventHandler(ProgressInfo info);
        /// <summary>
        /// Делегат события завершения обработки выбранных файлов
        /// </summary>
        public delegate void CompleteProgressEventHandler();



    }
}
