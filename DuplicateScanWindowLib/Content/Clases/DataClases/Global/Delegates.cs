using DuplicateScannerLib.Clases.DataClases.Result;
using DuplicateScanWindowLib.Content.Controls;
using DuplicateScannerLib.Clases.DataClases.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanWindowLib.Content.Clases.DataClases.Global
{
    /// <summary>
    /// Класс глобальных делегатов событий
    /// </summary>
    public class Delegates
    {
        /// <summary>
        /// Делегат события обновления выделение контроллу найденного зиолбражения
        /// </summary>
        /// <param name="control">Контролл для выделения</param>
        public delegate void UpdateFindedImageControlSelectionEventHandler(FindedImageControl control);

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

    }
}
