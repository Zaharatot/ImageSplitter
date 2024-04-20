using DuplicateScannerLib.Clases.DataClases.File;
using DuplicateScannerLib.Clases.DataClases.Result;
using SplitterResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static DuplicateScanWindowLib.Content.Clases.DataClases.Global.Delegates;
using static SplitterDataLib.DataClases.Global.Delegates;

namespace DuplicateScanWindowLib.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для FindedImagesPanel.xaml
    /// </summary>
    public partial class FindedImagesPanel : UserControl
    {

        /// <summary>
        /// Событие обновления выделения для контроллов найденных изображений
        /// </summary>
        public event UpdateFindedImageControlSelectionEventHandler UpdateFindedImageControlSelection;
        /// <summary>
        /// Событие запроса на скрытие остальных панелей
        /// </summary>
        public event HidePanelRequestEventHandler HidePanelRequest;
        /// <summary>
        /// Событие обновления чекбокса для дубликата
        /// </summary>
        public event SetCheckToDuplicateEventHandler SetCheckToDuplicate;


        /// <summary>
        /// Строка заголовка контролла
        /// </summary>
        public string ElementHeader => GroupPanelHeaderRun.Text;
        /// <summary>
        /// Флаг развёртывания панели
        /// </summary>
        public bool IsExpanded
        {
            get => ShowPanelExpander.IsExpanded;
            set => ShowPanelExpander.IsExpanded = value;
        }


        /// <summary>
        /// Текст заголовка информации о блокировке
        /// </summary>
        private string _blockedHeader;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public FindedImagesPanel()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        private void Init()
        {
            //Загружаем строки ресурсов
            _blockedHeader = ResourceLoader.LoadString("Text_FindedImagesPanel_BlockedHeader");
        }

        /// <summary>
        /// Обработчик события выбора контролла
        /// </summary>
        /// <param name="control">Выбранный контролл</param>
        private void ImageControl_UpdateFindedImageControlSelection(FindedImageControl control) =>
            //Вызываем внешний ивент
            UpdateFindedImageControlSelection?.Invoke(control);

        /// <summary>
        /// Обработчик события нажатия на кнопку сброса всхе выделений
        /// </summary>
        private void CleanAllCheckBoxesButton_Click(object sender, RoutedEventArgs e) =>
            //Сбрасываем все галочки на чекбоксах
            SetAllCheckBoxesState(false);

        /// <summary>
        /// Обработчик события нажатия на кнопку простановки всхе выделений
        /// </summary>
        private void SetAllCheckBoxesButton_Click(object sender, RoutedEventArgs e) =>
            //Проставляем все галочки на чекбоксах
            SetAllCheckBoxesState(true);

        /// <summary>
        /// Обработчик события показа панели
        /// </summary>
        private async void ShowPanelExpander_Expanded(object sender, RoutedEventArgs e)
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
                //Подгружаем все картинки на контролл
                await imageControl.LoadImage();
            //Вызываем ивент скрытия остальных панелей
            HidePanelRequest?.Invoke(ElementHeader);
        }

        /// <summary>
        /// Обработчик события скрытия панели
        /// </summary>
        private void ShowPanelExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
                //Выгружаем все картинки с контролла
                imageControl.CloseImageSource();
        }



        /// <summary>
        /// Создаём контролл целевого изображения
        /// </summary>
        /// <param name="result">Информация о результате поиска</param>
        /// <param name="colors">Цвета для списка разрешений</param>
        /// <returns>Контроллл с изображением</returns>
        private FindedImageControl CreateControl(DuplicateResult result, Dictionary<double, Color> colors)
        {
            //Создаём новый контролл
            FindedImageControl imageControl = new FindedImageControl();
            //Добавляем обработчик события выделения контролла
            imageControl.UpdateFindedImageControlSelection += ImageControl_UpdateFindedImageControlSelection;
            //Добавляем обработчик события выбора контролла для удаления
            imageControl.SetCheckToDuplicate += ImageControl_SetCheckToDuplicate;
            //ПРоставляем в контролл информацию об иконке
            imageControl.SetControlInfo(result, GroupPanelHeaderRun.Text);
            //Проставляем в контролл цвет для расширения
            imageControl.SetResolutionColor(colors[result.Resolution]);
            //Возвращаем созданный контролл
            return imageControl;
        }

        /// <summary>
        /// Обработчик события выбора контролла для удаления
        /// </summary>
        /// <param name="hash">Хеш элемента для простановки</param>
        /// <param name="parentName">Имя родительского элемента, откуда пришёл запрос блокировки</param>
        /// <param name="state">Статус для простановки</param>
        private void ImageControl_SetCheckToDuplicate(uint hash, string parentName, bool state) =>
            //Передаём ивент родительскому контроллу
            SetCheckToDuplicate?.Invoke(hash, parentName, state);

        /// <summary>
        /// Метод простановки нового статуса всем дочерним чекбоксам
        /// </summary>
        /// <param name="state">Новый статус</param>
        private void SetAllCheckBoxesState(bool state)
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
                //Для каждого из них сбрасываем галочку
                imageControl.SetCheckBoxState(state);
        }

        /// <summary>
        /// Метод получения цветов для каждого из разрешений
        /// </summary>
        /// <param name="result">Класс результата поиска</param>
        /// <returns>Цвета для списка разрешений</returns>
        private Dictionary<double, Color> GetElementResolutionColors(DuplicatePair result)
        {
            //Выходной словарь цветов
            Dictionary<double, Color> colors = new Dictionary<double, Color>();
            //Получаем список униальных разрешений, отсортирвоанных по убыванию
            List<double> resolutions = result.GetResolutions();
            //0 - красный, Min - синий
            //Получаем значение шага
            double step = 255.0 / resolutions.Count;
            //Задаём стартовое значение цвета
            double value = 255;
            //Проходимся по разрешениям
            foreach (double res in resolutions)
            {
                //Добавляем разрешение и его цвет в список
                colors.Add(res, Color.FromRgb((byte)value, 0, 255));
                //Здвигаем значение цвета
                value -= step;
            }
            //Возвращаем результат
            return colors;
        }

        /// <summary>
        /// Обновляем заголовок блокировки
        /// </summary>
        /// <param name="blockedCount">Количество заблокированных элементов</param>
        private void UpdateBlockedHeader(int blockedCount) =>
            GroupPanelBlockedHeaderRun.Text = 
                //Если блокировок нет, то заголовок будет пустым
                (blockedCount == 0) ? "" : $"{_blockedHeader} {blockedCount}";

        /// <summary>
        /// Метод проверки поиска для дочерних элементов
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        /// <returns>True - поиск был успешным</returns>
        private bool IsChildElementSearchSucc(string searchString)
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
                //Если поиск был успешен для дочернего элемента
                if (imageControl.IsSearchSucc(searchString))
                    //Возвращаем флаг успеха
                    return true;
            //Если в дочерних такого нет, то возвращаем флаг отсутствия
            return false;
        }






        /// <summary>
        /// Проставляем изображения-дубликаты в контролл
        /// </summary>
        /// <param name="result">Класс результата поиска</param>
        /// <param name="id">Идентификатор элемента</param>
        public void SetImagesToControl(DuplicatePair result, int id)
        {
            //Удаляем старые изображения с панели
            ClearOldImages();
            //Втыкаем в загрзовок экспандера только
            //идентификатор - всё остальное не имеет смысла
            GroupPanelHeaderRun.Text = $"[#{id}]";
            //Получаем цвета для списка разрешений элементов
            Dictionary<double, Color> colors = GetElementResolutionColors(result);
            //Создаём и добавляем на панель контролл изображения для оригинала
            MainPanel.Children.Add(CreateControl(result.Original, colors));
            //Создаём и добавляем на панель контролл изображения для копии
            MainPanel.Children.Add(CreateControl(result.Copy, colors));
        }

        /// <summary>
        /// Сбрасыфваем выделение для всех дочерних контроллов
        /// </summary>
        public void UnselectAllChilds()
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
                //Для каждого из них сбрасываем выделение
                imageControl.SetSelectionState(false);
        }

        /// <summary>
        /// Удаляем старые изображения с панели
        /// </summary>
        public void ClearOldImages()
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
            {
                //Удаляем обработчик события выделения контролла
                imageControl.UpdateFindedImageControlSelection -= ImageControl_UpdateFindedImageControlSelection;
                //Удаляем обработчик события выбора контролла для удаления
                imageControl.SetCheckToDuplicate -= ImageControl_SetCheckToDuplicate;
                //Закрываем поток работы с изображением
                imageControl.CloseImageSource();
            }
            //Очищаем панель от старых контроллов
            MainPanel.Children.Clear();
        }

        /// <summary>
        /// Получаем информацию о хешах из дочерних контроллов
        /// </summary>
        /// <param name="notSelectedHashes">Список хешей не выбранных элементов</param>
        /// <param name="selectedHashes">Список хешей выбранных элементов</param>
        /// <returns>Список хешей из дочерних контроллов</returns>
        public void GetHashesFromChilds(out List<uint> selectedHashes, out List<uint> notSelectedHashes)
        {
            //Инициализируем выходные массивы
            selectedHashes = new List<uint>();
            notSelectedHashes = new List<uint>();
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
            {
                //Если это изображение выбрано
                if (imageControl.IsSelected)
                    //Добавляем хеш из него в список выбранных
                    selectedHashes.Add(imageControl.DuplicateHash);
                //Если нет
                else
                    //Добавляем хеш из него в список не выбранных
                    notSelectedHashes.Add(imageControl.DuplicateHash);
            }
        }

        /// <summary>
        /// Метод простановки статуса для чекбокса, по значению из другой группы
        /// </summary>
        /// <param name="hash">Хеш элемента для простановки</param>
        /// <param name="parentName">Имя родительского элемента, откуда пришёл запрос блокировки</param>
        /// <param name="state">Статус для простановки</param>
        /// <returns>True, если был заблокирован последний активный элемент в группе</returns>
        public bool SetParentCheckBoxState(uint hash, string parentName, bool state)
        {
            //КОличество заблокированных элементов
            int blockedCount = 0, beforeBlockedCount = 0;
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
            {
                //Если элемент был заблокирован ранее
                if (imageControl.IsParentCheckState)
                    //Обновляем счётчик заблокированных
                    beforeBlockedCount++;
                //Вызываем у них метод обновления
                imageControl.SetParentCheckBoxState(hash, parentName, state);
                //Если элемент был заблокирован
                if (imageControl.IsParentCheckState)
                    //Обновляем счётчик заблокированных
                    blockedCount++;
            }
            //Обновляем заголовок блокировки
            UpdateBlockedHeader(blockedCount);
            //Если был заблокирован элемент, и это был последний не заблокированный элемент
            return (beforeBlockedCount != blockedCount) && (blockedCount == MainPanel.Children.Count);
        }

        /// <summary>
        /// Метод проверки поиска
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        /// <returns>True - поиск был успешным</returns>
        public bool IsSearchSucc(string searchString) =>
            //Если строка поиска пустая
            string.IsNullOrEmpty(searchString) ||
            //Или если в заголовке панели есть строка поиска
            GroupPanelHeaderRun.Text.ToLower().Contains(searchString) ||
            //Или если в дочерних есть строка поиска
            IsChildElementSearchSucc(searchString);
    }
}
