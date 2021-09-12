using ImageSplitter.Content.Clases.DataClases.Duplicates;
using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Controls;
using ImageSplitter.Content.Controls.ImageDuplicateScan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.DataClases.Global
{
    /// <summary>
    /// Класс глобальных делегатов событий
    /// </summary>
    public class Delegates
    {
        /// <summary>
        /// Делегат пустого события
        /// </summary>
        public delegate void EmptyEventHandler();

        /// <summary>
        /// Делегат события обновления основной информации на контролле сплита изображений
        /// </summary>
        /// <param name="pagesInfo">Инфомрация о текущих отображаемых страницах</param>
        /// <param name="folders">Список доступных папок</param>
        public delegate void UpdateImageSplitInfoEventHandler(string pagesInfo, List<TargetFolderInfo> folders);

        /// <summary>
        /// Делегат события запуска сканирования файлов для сплита
        /// </summary>
        /// <param name="scanPath">Путь сканирования</param>
        /// <param name="splitPath">Путь сплита</param>
        /// <param name="isFolder">Флаг сканирования папок</param>
        public delegate void StartSplitScanEventHandler(string scanPath, string splitPath, bool isFolder);

        /// <summary>
        /// Делегат события перехода к изображению
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        public delegate void MoveToImageEventHandler(int direction);

        /// <summary>
        /// Делегат события запуска сплита файлов
        /// </summary>
        /// <param name="countFiles">Количество файлов для сплита</param>
        /// <param name="path">Путь для сплита</param>
        /// <param name="isChildSplit">Флаг сплита в дочерних папках</param>
        public delegate void StartFileSplitEventHandler(string path, int countFiles, bool isChildSplit);

        /// <summary>
        /// Делегат события запуска отмены сплита файлов
        /// </summary>
        /// <param name="path">Путь для отмены сплита</param>
        public delegate void StartBackEventHandler(string path);

        /// <summary>
        /// Делегат события запуска переименования файлов
        /// </summary>
        /// <param name="mask">Маска имени для переименования</param>
        /// <param name="path">Путь для переименования</param>
        public delegate void RenameFilesEventHandler(string path, string mask);

        /// <summary>
        /// Делегат события запроса на удаление целевой папки из списка
        /// </summary>
        /// <param name="key">Клавиша, к которой привязана папка</param>
        /// <param name="folderName">Имя папки</param>
        public delegate void RemoveFolderRequestEventHandler(Key key, string folderName);

        /// <summary>
        /// Делегат события обновления выделение контроллу найденного зиолбражения
        /// </summary>
        /// <param name="control">Контролл для выделения</param>
        public delegate void UpdateFindedImageControlSelectionEventHandler(FindedImageControl control);

        /// <summary>
        /// Делегат события запуска сканирования на дубликаты
        /// </summary>
        /// <param name="path">Путь сканирования</param>
        public delegate void StartDuplicateScanEventHandler(string path);

        /// <summary>
        /// Делегат события обновления информации о прогрессе сканирования дубликатов
        /// </summary>
        /// <param name="current">Текущее значение</param>
        /// <param name="max">Максимальное значение</param>
        public delegate void DuplicateScanProgressEventHandler(int current, int max);
        
        /// <summary>
        /// Делегат события завершения поиска дубликатов
        /// </summary>
        /// <param name="duplicates">Список дубликатов для отображения</param>
        public delegate void DuplicateScanCompleteEventHandler(List<DuplicateImageInfo> duplicates);

        /// <summary>
        /// Делегат события удаления дубликатов
        /// </summary>
        /// <param name="duplicates">Список дубликатов для удаления</param>
        public delegate void DuplicateRemoveEventHandler(List<DuplicateImageInfo> duplicates);
    }
}
