using DuplicateScannerLib.Clases.DataClases.Result;
using SplitterResources;
using SplitterSimpleUI.Content.Clases.WorkClases.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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
using static SplitterDataLib.DataClases.Global.Delegates;

namespace DuplicateScanWindowLib.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для FindedImageControl.xaml
    /// </summary>
    public partial class FindedImageControl : UserControl
    {

        /// <summary>
        /// Событие обновления чекбокса для дубликата
        /// </summary>
        public event SetCheckToDuplicateEventHandler SetCheckToDuplicate;
        /// <summary>
        /// Событие обновления выделения для контроллов найденных изображений
        /// </summary>
        public event UpdateFindedImageControlSelectionEventHandler UpdateFindedImageControlSelection;



        /// <summary>
        /// Хеш дубликата
        /// </summary>
        public uint DuplicateHash => _result.PathHash;
        /// <summary>
        /// Строка полного пути к изображению
        /// </summary>
        public string DuplicateImagePath => _result.Path;
        /// <summary>
        /// Флаг выбора данного контролла
        /// </summary>
        public bool IsSelected => GetCheckBoxState();
        /// <summary>
        /// Флаг верхнеуровнегого статуса блокировки
        /// </summary>
        public bool IsParentCheckState { get; private set; }


        /// <summary>
        /// Информация о результате поиска
        /// </summary>
        private DuplicateResult _result;
        /// <summary>
        /// Название родительского элемента
        /// </summary>
        private string _parentName;
        /// <summary>
        /// Текст заголовка информации о блокировке
        /// </summary>
        private string _blockedHeader;
        /// <summary>
        /// Класс загрузки изображения
        /// </summary>
        private ImageSourceLoader _imageSourceLoader;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public FindedImageControl()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        private void Init()
        {
            //Проставляем дефолтные значения
            _result = null;
            IsParentCheckState = false;
            //Загружаем строки ресурсов
            _blockedHeader = ResourceLoader.LoadString("Text_FindedImageControl_BlockedHeader");
            //Инициализируем класс загрузки изображения
            _imageSourceLoader = new ImageSourceLoader();
        }


        /// <summary>
        /// Обработчик события нажатия на панель 
        /// </summary>
        private void MainPanel_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            UpdateFindedImageControlSelection?.Invoke(this);

        /// <summary>
        /// Обработчик события изменения статуса чекбокса
        /// </summary>
        private void SelectImageCheckBox_CheckedChange(object sender, RoutedEventArgs e)
        {
            //Если чекбокс активен
            if (!IsParentCheckState)
                //Вызываем внешний ивент
                SetCheckToDuplicate?.Invoke(_result.PathHash, _parentName, IsSelected);
        }


        /// <summary>
        /// Получаем значение статуса чекбокса
        /// </summary>
        /// <returns>True - чекбокс нажат</returns>
        private bool GetCheckBoxState() =>
            SelectImageCheckBox.IsChecked.GetValueOrDefault(false);







        /// <summary>
        /// Закрываем поток в памяти, связанный с изображением
        /// </summary>
        public void CloseImageSource() =>
            //Закрываем поток изображения
            _imageSourceLoader.CloseImageSource(FindedImageIcon);

        /// <summary>
        /// Обновляем значение статуса выделения
        /// </summary>
        /// <param name="state">Новое значение статуса выделения</param>
        public void SetSelectionState(bool state) =>
            //Проставляем цвет контролла
            MainPanel.Background = (state) ? Brushes.BlanchedAlmond : Brushes.White;


        /// <summary>
        /// Проставляем новое значение чекбоксу
        /// </summary>
        /// <param name="state">Новое значение чекбоксу</param>
        public void SetCheckBoxState(bool state)
        {
            //Если чекбокс активен
            if(!IsParentCheckState)
                //Меняем ему статус
                SelectImageCheckBox.IsChecked = state;
        }

        /// <summary>
        /// Проставляем инфу в контролл
        /// </summary>
        /// <param name="info">Информация о контролле</param>
        /// <param name="parentName">Название родительского элемента</param>
        public void SetControlInfo(DuplicateResult info, string parentName)
        {
            //Запоминаем переданные значения
            _result = info;
            _parentName = parentName;
            //Проставляем инфу в текстовые поля
            ImageParentFolderToolTip.Content = info.ParentPath;
            ImageParentFolderRun.Text = $"[{info.ParentName}]";
            ImageNameToolTip.Content = ImageNameRun.Text = info.Name;
            ImageSizeRun.Text = $"{info.Width}x{info.Height}";
        }


        /// <summary>
        /// Метод простановки цвета для расширения
        /// </summary>
        /// <param name="col">Цвет для расширения</param>
        public void SetResolutionColor(Color col) =>
            //Проставляем цвет фона для блока расширения
            ImageSizeRun.Foreground = new SolidColorBrush(col);

        /// <summary>
        /// Метод выполнения подгрузки изображения в контролл
        /// </summary>
        public async Task LoadImage() =>
            //Грузим картинку в контролл
            await _imageSourceLoader.LoadImageByPath(FindedImageIcon, _result.Path, 120);

        /// <summary>
        /// Метод простановки статуса для чекбокса, по значению из другой группы
        /// </summary>
        /// <param name="hash">Хеш элемента для простановки</param>
        /// <param name="parentName">Имя родительского элемента, откуда пришёл запрос блокировки</param>
        /// <param name="state">Статус для простановки</param>
        public void SetParentCheckBoxState(uint hash, string parentName, bool state)
        {
            //Если запрос предназначен для текущего элемента, и не от него он пошёл
            if ((hash == _result.PathHash) && (parentName != _parentName))
            {
                //Сохраняем значение стейта для внутренних блокировок
                IsParentCheckState = state;
                //Проставляем статус выбора чекбокса
                SelectImageCheckBox.IsChecked = state;
                //Если нужно проставить чекбокс, то мы блокируем изменения текущего
                SelectImageCheckBox.IsEnabled = !state;
                //Выводим в контролл информацию о блокировке
                BlockInfoTextBlock.Text = $"{_blockedHeader} {parentName}";
                //Проставляем видимость панели информации о блокировке
                BlockInfoTextBlock.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Флаг успешного поиска
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        /// <returns>True - поиск был успешным</returns>
        public bool IsSearchSucc(string searchString) =>
            //Если строка поиска пустая
            string.IsNullOrEmpty(searchString) ||
            //Или если в пути к файлу есть часть строки поиска
            _result.Path.ToLower().Contains(searchString);
    }
}
