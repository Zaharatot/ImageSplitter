using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageSplitter.Content.Clases.DataClases.Delegates;

namespace ImageSplitter.Content.Clases.DataClases
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
        /// Событие завершения сканирования дубликатов
        /// </summary>
        public static event DuplicateScanCompleteEventHandler DuplicateScanComplete;
        /// <summary>
        /// Событие завершения сканирования дубликатов, при котором не было найдено дублей
        /// </summary>
        public static event EmptyEventHandler DuplicateScanNotFound;
        /// <summary>
        /// Событие обновления информации о прогрессе сканирования дубликатов
        /// </summary>
        public static event DuplicateScanProgressEventHandler DuplicateScanProgress;
        /// <summary>
        /// Событие завершения удаления дубликатов
        /// </summary>
        public static event EmptyEventHandler RemoveDuplicatesComplete;


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
        /// Метод вызова ивента завершения сканирования дубликатов
        /// </summary>
        /// <param name="duplicates">Список дубликатов для отображения</param>
        public static void InvokeDuplicateScanComplete(List<DuplicateImageInfo> duplicates) =>
            DuplicateScanComplete?.Invoke(duplicates);

        /// <summary>
        /// Метод вызова ивента обновления информации о прогрессе сканирования дубликатов
        /// </summary>
        /// <param name="current">Текущее значение</param>
        /// <param name="max">Максимальное значение</param>
        public static void InvokeDuplicateScanProgress(int current, int max) =>
            DuplicateScanProgress?.Invoke(current, max);

        /// <summary>
        /// Метод вызова ивента завершения удаления дубликатов
        /// </summary>
        public static void InvokeRemoveDuplicatesComplete() =>
            RemoveDuplicatesComplete?.Invoke();

        /// <summary>
        /// Метод вызова ивента завершения сканирования 
        /// дубликатов, при котором не было найдено дублей
        /// </summary>
        public static void InvokeDuplicateScanNotFound() =>
            DuplicateScanNotFound?.Invoke();

    }
}
