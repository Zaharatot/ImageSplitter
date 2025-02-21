using SplitterResources;
using SplitterSimpleUI.Content.Clases.DataClases.Panels;
using SplitterSimpleUI.Content.Clases.WorkClases.Controls;
using SplitterSimpleUI.Content.Clases.WorkClases.Properties;
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
using static SplitterSimpleUI.Content.Clases.DataClases.Global.Delegates;
using static SplitterSimpleUI.Content.Clases.DataClases.Global.Enums;

namespace SplitterSimpleUI.Content.Controls.Panels
{
    /// <summary>
    /// Логика взаимодействия для MainMenuPanel.xaml
    /// </summary>
    public partial class MainMenuPanel : UserControl
    {

        /// <summary>
        /// Событие выбора элемента основного меню
        /// </summary>
        public event MainMenuSelectItemEventHandler MainMenuSelectItem;


        /// <summary>
        /// Свойство зависимостей для списка элементов основного меню
        /// </summary>
        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register("Elements", typeof(MainMenuElementsList), typeof(MainMenuPanel),
                new UIPropertyMetadata(
                    new MainMenuElementsList(),
                    new PropertyChangedCallback(ControlPropertyCallbacks.ControlPropertyChangedNew)
                ));
        /// <summary>
        /// Список элементов основного меню
        /// </summary>
        public MainMenuElementsList Elements
        {
            get => (MainMenuElementsList)GetValue(ElementsProperty);
            set => SetValue(ElementsProperty, value);
        }

        /// <summary>
        /// Предзагруженный стиль для тултипа
        /// </summary>
        private Style _toolTipStyle;
        /// <summary>
        /// Предзаданное значение отступа иконки
        /// </summary>
        private Thickness _iconMargin;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public MainMenuPanel()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        private void Init()
        {
            //Заранее подгружаем стиль для тултипа
            _toolTipStyle = ResourceLoader.LoadStyle("Style_ToolTip_WithWrapping");
            //Инициализируем предзаданные отступы для иконки
            _iconMargin = new Thickness(10);
        }



        /// <summary>
        /// Метод создания подсказки для меню
        /// </summary>
        /// <param name="element">Тип элемента</param>
        /// <returns>Созданный контролл подсказки</returns>
        private ToolTip CreateToolTip(MainMenuElements element)
        {
            //Инициализируем контролл подсказки
            ToolTip toolTip = new ToolTip();
            //Проставляем тултипу стиль
            toolTip.Style = _toolTipStyle;
            //Грузим контент для тултипа
            toolTip.Content = ResourceLoader.LoadString($"Text_MainMenu_{element}_Tooltip");
            //Возвращаем созданную подсказку
            return toolTip;
        }

        /// <summary>
        /// Метод создания иконки для меню
        /// </summary>
        /// <param name="element">Тип элемента</param>
        /// <returns>Созданный контролл иконки</returns>
        private SvgImageControl CreateIcon(MainMenuElement element)
        {
            //Инициализируем контролл иконки
            SvgImageControl icon = new SvgImageControl();
            //Создаём подсказку для иконки
            icon.ToolTip = CreateToolTip(element.Element);
            //Подгрузаем саму иконку
            icon.Image = ResourceLoader.LoadIcon($"Icon_MainMenu_{element.Element}");
            //Проставляем курсор для иконки
            icon.Cursor = Cursors.Hand;
            //Проставляем отступы для иконки
            icon.Margin = _iconMargin;
            //Сохраняем тип элемента в тэг
            icon.Tag = element;
            //Добавляем обработчик события клика по иконке
            icon.MouseDown += Icon_MouseDown;
            //Возвращаем созданную иконку
            return icon;
        }

        /// <summary>
        /// Обработчик события клика по иконке
        /// </summary>
        private void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Если у нас действительно иконка
            if (sender is SvgImageControl icon)
                //Получаем из тега иконки элемент, и передаём его в ивенте
                MainMenuSelectItem?.Invoke((icon.Tag as MainMenuElement).Element);
        }

        /// <summary>
        /// Метод удаления старых элементов меню
        /// </summary>
        /// <returns>Список удалённых контроллов иконок</returns>
        private List<SvgImageControl> ClearOldElements()
        {
            //Возвращаем список контроллов иконок, которые находятся на панели
            List<SvgImageControl> icons = MainPanel.Children.Cast<SvgImageControl>().ToList();
            //Проходимся по иконкам панели
            foreach (SvgImageControl icon in icons)
                //Удаляем обработчик события клика по иконке
                icon.MouseDown -= Icon_MouseDown;
            //Удаляем иконки с панели
            MainPanel.Children.Clear();
            //Возвращаем список иконок
            return icons;
        }


        /// <summary>
        /// Метод обновления списка элементов основного меню
        /// </summary>
        /// <param name="elements">Список элементов для обновления</param>
        public void SetElements(MainMenuElementsList elements)
        {
            //Получаем экземпляр класса обработки событий для иконок
            IconsSelectionProcessor selectionProcessor = IconsSelectionProcessor.GetInstance();
            //Удаляем старые иконки с панели, и получаем их список
            List<SvgImageControl> icons = ClearOldElements();
            //Запрашиваем удаление этих иконок из класса выделения
            selectionProcessor.DeleteIcons(icons);

            //Создаём для каждого переданного элемента контролл иконки
            icons = elements.Elements.ConvertAll(element => CreateIcon(element));
            //Передаём в класс обработки выделений новый список иконок
            selectionProcessor.AddIcons(icons);
            //Добавляем иконки на панель
            icons.ForEach(icon => MainPanel.Children.Add(icon));
        }
    }
}
