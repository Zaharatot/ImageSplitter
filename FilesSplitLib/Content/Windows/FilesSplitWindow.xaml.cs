using SplitterSimpleUI.Content.Clases.DataClases.HotKey;
using SplitterSimpleUI.Content.Clases.WorkClases.Controls;
using SplitterSimpleUI.Content.Clases.WorkClases.HotKey;
using SplitterSimpleUI.Content.Controls;
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
using System.Windows.Shapes;

namespace FilesSplitWindowLib.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для FilesSplitWindow.xaml
    /// </summary>
    public partial class FilesSplitWindow : Window
    {
        /// <summary>
        /// Флаг выполнения сплита, а не возврата
        /// </summary>
        public bool IsSplit { get; private set; }
        /// <summary>
        /// Количество файлов для сплита
        /// </summary>
        public int CountSplitFiles { get; private set; }
        /// <summary>
        /// Флаг выбора сплита дочерних
        /// </summary>
        public bool IsChildSplit => SplitChildsCheckBox.IsChecked;


        /// <summary>
        /// Класс обработки хоткеев
        /// </summary>
        private HotKeyProcessor _hotKeyProcessor;

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public FilesSplitWindow()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор окна
        /// </summary>
        private void Init()
        {
            //Инициализируем хоткеи
            InitHotkeys();
            //Инициализируем события для иконок
            InitIconsEvents();
        }

        /// <summary>
        /// Инициализируем хоткеи
        /// </summary>
        private void InitHotkeys()
        {
            //Получаем экземпляр класса обработки хоткеев
            _hotKeyProcessor = HotKeyProcessor.GetInstance();
            //Добавляем хоткеи для текущего окна
            AddHotKeys();
        }

        /// <summary>
        /// Метод инициализации событий для иконок
        /// </summary>
        private void InitIconsEvents() =>
            //Получаем экземпляр класса обработки событий для иконок
            IconsSelectionProcessor.GetInstance()
                //Добавляем в него иконки для обработки
                .AddIcons(new List<SvgImageControl>() {
                    SplitIcon, ReturnIcon
            });

        /// <summary>
        /// Метод добавления хоткеев для окна
        /// </summary>
        private void AddHotKeys() =>
            _hotKeyProcessor.AddWindow(this, new WindowHotKeys() {
                //Добавляем список хоткеев
                HotKeys = new List<HotKeyInfo>() { 
                    //При нажатии на "Enter" - вызываем закрытие окна с запуском сплита
                    new HotKeyInfo(Key.Enter, () => { ProcessCloseWindow(true); }),
                    //При нажатии на "Ctrl + Enter" - вызываем закрытие окна с запуском возврата
                    new HotKeyInfo(Key.Enter, () => { ProcessCloseWindow(false); }, true),
                    //При нажатии на "Escape" - вызываем закрытие окна без результата
                    new HotKeyInfo(Key.Escape, () => { this.DialogResult = false; }),
                }
            });

        /// <summary>
        /// Обработчик осбытия закрытия окна
        /// </summary>
        private void Window_Closed(object sender, EventArgs e) =>
            //Удаляем обюработку хоткеев для данного окна
            _hotKeyProcessor.RemoveWindow(this);


        /// <summary>
        /// Обработчик события нажатия на иконку запуска сплита
        /// </summary>
        private void SplitIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем закрытие окна с запуском сплита
            ProcessCloseWindow(true);

        /// <summary>
        /// Обработчик события нажатия на иконку запуска возврата файлов
        /// </summary>
        private void ReturnIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем закрытие окна с запуском возврата
            ProcessCloseWindow(false);

        /// <summary>
        /// Метод простановки количества файлов для сплита
        /// </summary>
        /// <returns>True - простановка успешна</returns>
        private bool SetSplitCount()
        {
            //Пытаемся распарсить число, введённое в текстовое поле
            if(int.TryParse(SplitCountTextBox.Text, out int result))
            {
                //Проставляем значение в параметр
                CountSplitFiles = result;
                //Возвращаем успешный результат
                return true;
            }
            //В случае ошибок - вернём false
            return false;
        }


        /// <summary>
        /// Метод обработки корректного закрытия окна
        /// </summary>
        /// <param name="isSplit">Флаг выполнения сплита</param>
        private void ProcessCloseWindow(bool isSplit)
        {
            //Если число для сплита было введено
            //корректно, или требуется возврат
            if (SetSplitCount() || !isSplit)
            {
                //Проставляем флаг выполнения сплита
                IsSplit = isSplit;
                //Успешно закрываем окно
                this.DialogResult = true;
            }
            //TODO: Тут ошибку выводить нужно во всплывашке
        }


    }
}
