using DuplicateScanner.Clases.DataClases.Result;
using ImageSplitter.Content.Clases.DataClases;
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
using static ImageSplitter.Content.Clases.DataClases.Global.Delegates;

namespace ImageSplitter.Content.Controls.ImageDuplicateScan
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
        /// Строка заголовка контролла
        /// </summary>
        public string ElementHeader => (string)ShowPanelExpander.Header;
        /// <summary>
        /// Флаг развёртывания панели
        /// </summary>
        public bool IsExpanded
        {
            get => ShowPanelExpander.IsExpanded;
            set => ShowPanelExpander.IsExpanded = value;
        }

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
        private void ShowPanelExpander_Expanded(object sender, RoutedEventArgs e)
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
                //Подгружаем все картинки на контролл
                imageControl.LoadImage();
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
        /// <returns>Контроллл с изображением</returns>
        private FindedImageControl CreateControl(DuplicateResult result)
        {
            //Создаём новый контролл
            FindedImageControl imageControl = new FindedImageControl();
            //Добавляем обработчик события выделения контролла
            imageControl.UpdateFindedImageControlSelection += ImageControl_UpdateFindedImageControlSelection;
            //ПРоставляем в контролл информацию об иконке
            imageControl.SetControlInfo(result);
            //Возвращаем созданный контролл
            return imageControl;
        }

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
        /// Проставляем изображения-дубликаты в контролл
        /// </summary>
        /// <param name="result">Класс результата поиска</param>
        /// <param name="id">Идентификатор элемента</param>
        public void SetImagesToControl(FindResult result, int id)
        {
            //Удаляем старые изображения с панели
            ClearOldImages();
            //Втыкаем в загрзовок экспандера только
            //идентификатор - всё остальное не имеет смысла
            ShowPanelExpander.Header = $"[#{id}]";
            //Проходимся по списку дубликатов
            foreach (var duplicate in result.Results)
                //Создаём и добавляем на панель контролл изображения
                MainPanel.Children.Add(CreateControl(duplicate));
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
    }
}
