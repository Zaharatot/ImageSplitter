using ImageSplitter.Content.Clases.DataClases.Controls;
using ImageSplitter.Content.Clases.WorkClases.Addition;
using ImageSplitter.Content.Clases.WorkClases.Helpers;
using ImageSplitter.Content.Clases.WorkClases.Helpers.Selection;
using ImageSplitter.Content.Clases.WorkClases.Resources;
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
using static ImageSplitter.Content.Clases.DataClases.Global.Enums;

namespace ImageSplitter.Content.Controls.Simple
{
    /// <summary>
    /// Логика взаимодействия для ComboCheckBoxControl.xaml
    /// </summary>
    public partial class ComboCheckBoxControl : UserControl
    {
        /// <summary>
        /// Событие обновления статуса чекбокса
        /// </summary>
        public event CheckBoxUpdateStateEventHandler CheckBoxUpdateState;


        /// <summary>
        /// Заголовок чекбокса
        /// </summary>
        public string Header
        {
            get => CheckBoxHeader.Text;
            set => CheckBoxHeader.Text = value;
        }

        /// <summary>
        /// Подсказка чекбокса
        /// </summary>
        public string Tooltip
        {
            get => (string)CheckBoxTooltip.Content;
            set => UniversalMethods.SetTooltipContent(CheckBoxTooltip, value);
        }


        /// <summary>
        /// Свойство зависимостей для статуса чекбокса
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(ComboCheckBoxStates), typeof(ComboCheckBoxControl),
                new UIPropertyMetadata(
                    ComboCheckBoxStates.Unchecked,
                    new PropertyChangedCallback(ControlPropertyCallbacks.ControlPropertyChangedNew)
                ));
        /// <summary>
        /// Статус чекбокса
        /// </summary>
        public ComboCheckBoxStates State
        {
            get => (ComboCheckBoxStates)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        /// <summary>
        /// Флаг чекнутого состояния
        /// </summary>
        public bool IsChecked => (State == ComboCheckBoxStates.Checked);


        /// <summary>
        /// Класс обработки иконок
        /// </summary>
        private IconsSelectionProcessor _iconsSelectionProcessor;

        /// <summary>
        /// Словарь иконок, по их статусам
        /// </summary>
        private Dictionary<ComboCheckBoxStates, SvgImage> _stateIconsDict;


        /// <summary>
        /// Статус чекбокса
        /// </summary>
        public ComboCheckBoxStates _state;

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        public ComboCheckBoxControl()
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
            _state = ComboCheckBoxStates.Unchecked;
            _stateIconsDict = CreateIconsDict();
            //Получаем экземпляр класса обработки событий для иконок
            _iconsSelectionProcessor = IconsSelectionProcessor.GetInstance();
        }

        /// <summary>
        /// Метод инициализации словаря иконок
        /// </summary>
        /// <returns>Созданный словарь иконок</returns>
        private Dictionary<ComboCheckBoxStates, SvgImage> CreateIconsDict() =>
            new Dictionary<ComboCheckBoxStates, SvgImage>() {
                { ComboCheckBoxStates.Checked, ResourceLoader.LoadIcon("Icon_Checkbox_Enabled") },
                { ComboCheckBoxStates.Unchecked, ResourceLoader.LoadIcon("Icon_Checkbox_Disabled") },
                { ComboCheckBoxStates.Partial, ResourceLoader.LoadIcon("Icon_Checkbox_Partial") },
            };


        //Вот такая схема с ивентами нужна для того, чтобы перехватывать ивент
        //для всей панели, и отправлять для обработки, как будто мы навелись на иконку

        /// <summary>
        /// Обработчик события наведения курсора на панель
        /// </summary>
        private void CheckBoxPanel_MouseEnter(object sender, MouseEventArgs e) =>
            //Иызываем обработчик ивента из класса выделения иконки
            _iconsSelectionProcessor.Icon_MouseEnter(CheckIcon, e);

        /// <summary>
        /// Обработчик события ухода курсора с панели
        /// </summary>
        private void CheckBoxPanel_MouseLeave(object sender, MouseEventArgs e) =>
            //Иызываем обработчик ивента из класса выделения иконки
            _iconsSelectionProcessor.Icon_MouseLeave(CheckIcon, e);

        /// <summary>
        /// Обработчик события включения/выключения панели
        /// </summary>
        private void CheckBoxPanel_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) =>
            //Иызываем обработчик ивента из класса выделения иконки
            _iconsSelectionProcessor.Icon_IsEnabledChanged(CheckIcon, e);


        /// <summary>
        /// Обработчик события клика по панели
        /// </summary>
        private void CheckBoxPanel_MouseDown(object sender, MouseButtonEventArgs e) =>
            //ВЫполняем обновление контролла
            ProcessClick();


        /// <summary>
        /// Метод перемещения статуса иконки к следующему
        /// </summary>
        private void MoveState()
        {
            //Если чекбокс частично или полностью чекнут
            if (_state == ComboCheckBoxStates.Checked || _state == ComboCheckBoxStates.Partial)
                //Ставим ему статус выключенного
                State = ComboCheckBoxStates.Unchecked;
            //Противный случай тут только один - выключенный
            else
                //И при нём только включаем
                State = ComboCheckBoxStates.Checked;
        }


        /// <summary>
        /// Метод обработке нажатия на контролл
        /// Позволяет эмулировать клик по контроллу
        /// </summary>
        public void ProcessClick()
        {
            //Обновляем статус чекбокса
            MoveState();
            //Вызываем ивент обновления статуса
            CheckBoxUpdateState?.Invoke(_state);
        }

        /// <summary>
        /// Выставление свойства при изменении свойства зависимостей State
        /// </summary>
        /// <param name="state">Новый статус изображения</param>
        public void SetState(ComboCheckBoxStates state)
        {
            //Сохраняем полученное значение
            _state = state;
            //Выставляем иконку соответствующую статусу
            CheckIcon.Image = _stateIconsDict[state];
        }
    }
}
