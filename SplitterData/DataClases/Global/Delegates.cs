using SplitterDataLib.DataClases.Global.DuplicateScan;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static SplitterDataLib.DataClases.Global.Enums;

namespace SplitterDataLib.DataClases.Global
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
        /// Делегат события запроса на отображение древа
        /// </summary>
        /// <param name="path">Путь для отображения древа</param>
        public delegate void ShowTreeRequestEventHandler(string path);

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
        /// Делегат события обновления пути сплита
        /// </summary>
        /// <param name="info">Информация о пути для сплита</param>
        public delegate void UpdateSplitPathEventHandler(SplitPathsInfo info);

        /// <summary>
        /// Делегат события обновления статуса чекбокса
        /// </summary>
        /// <param name="state">Новый статус чекбокса</param>
        public delegate void CheckBoxUpdateStateEventHandler(ComboCheckBoxStates state);


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
        public delegate void UpdateFindedImageControlSelectionEventHandler(object control);


        /// <summary>
        /// Делегат события удаления дубликатов
        /// </summary>
        /// <param name="groups">Список запрещённых групп</param>
        /// <param name="toRemove">Группа хешей для удаления</param>
        public delegate void DuplicateRemoveEventHandler(HashesGroup toRemove, List<HashesGroup> groups);
        /// <summary>
        /// Делегат события запуска сканирования на дубликаты
        /// </summary>
        /// <param name="properties">Параметры сканирования</param>
        public delegate void StartDuplicateScanEventHandler(ScanProperties properties);

        /// <summary>
        /// Делегат события запроса на переход ко вкладке
        /// </summary>
        /// <param name="tabId">Id вкладки для перехода</param>
        public delegate void SendToTabRequestEventHandler(int tabId);

        /// <summary>
        /// Делегат события запроса на скрытие панелей
        /// </summary>
        /// <param name="showedPanelHeader">Заголовок только что открытой панели</param>
        public delegate void HidePanelRequestEventHandler(string showedPanelHeader);

        /// <summary>
        /// Делегат событяи выбора чекбокса для дубликата
        /// </summary>
        /// <param name="hash">Хеш элемента для простановки</param>
        /// <param name="parentName">Имя родительского элемента, откуда пришёл запрос блокировки</param>
        /// <param name="state">Статус для простановки</param>
        public delegate void SetCheckToDuplicateEventHandler(uint hash, string parentName, bool state);

        /// <summary>
        /// Делегат события изменения текстового контента
        /// </summary>
        /// <param name="text">ИЗменённый текст</param>
        public delegate void ChangeTextEventHandler(string text);

        /// <summary>
        /// Делегат события запроса на выбор папок
        /// </summary>
        /// <param name="currentFolders">Текущий список папок</param>
        public delegate void SelectFoldersRequestEventHandler(List<TargetFolderInfo> currentFolders);

        /// <summary>
        /// Делегат события нажатия клавиши
        /// </summary>
        /// <param name="key">Нажатая клавиша</param>
        public delegate void KeyPressEventHandler(Key key);

    }
}
