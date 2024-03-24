using DuplicateScanner.Clases.DataClases.Properties;
using ImageSplitter.Content.Clases.DataClases.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageSplitter.Content.Clases.DataClases.Global.Delegates;

namespace ImageSplitter.Content.Clases.DataClases.Global
{
    /// <summary>
    /// Класс глобальных событий
    /// </summary>
    internal static class GlobalEvents
    {
        /// <summary>
        /// Событие запроса на обновление инфы о сплите картинок
        /// </summary>
        public static event UpdateImageSplitInfoEventHandler UpdateImageSplitInfoRequest;
        /// <summary>
        /// Событие завершения сканирования
        /// </summary>
        public static event EmptyEventHandler ScanComplete;
        /// <summary>
        /// Событие завершения переноса изображения
        /// </summary>
        public static event EmptyEventHandler MoveImageComplete;
        /// <summary>
        /// Событие запуска скнирования на дубликаты
        /// </summary>
        public static event StartDuplicateScanEventHandler StartDuplicateScan;
        /// <summary>
        /// Событие запроса удаления старых записей
        /// </summary>
        public static event EmptyEventHandler RemoveOldRequest;





        /// <summary>
        /// Метод вызова ивента обновления основной информации на контролле сплита изображений
        /// </summary>
        /// <param name="pagesInfo">Инфомрация о текущих отображаемых страницах</param>
        /// <param name="folders">Список доступных папок</param>
        public static void InvokeUpdateImageSplitInfoRequest(string pagesInfo, List<TargetFolderInfo> folders) =>
            UpdateImageSplitInfoRequest?.Invoke(pagesInfo, folders);

        /// <summary>
        /// Метод вызова ивента завершения сканирования
        /// </summary>
        public static void InvokeScanComplete() =>
            ScanComplete?.Invoke();

        /// <summary>
        /// Метод вызова ивента завершения переноса изображения
        /// </summary>
        public static void InvokeMoveImageComplete() =>
            MoveImageComplete?.Invoke();

        /// <summary>
        /// Метод вызова ивента запуска сканирования
        /// </summary>
        /// <param name="properties">Параметры сканирования</param>
        public static void InvokeStartDuplicateScan(ScanProperties properties) =>
            StartDuplicateScan?.Invoke(properties);

        /// <summary>
        /// Метод вызова ивента удаления старых записей
        /// </summary>
        public static void InvokeRemoveOldRequest() =>
            RemoveOldRequest?.Invoke();
        
    }
}
