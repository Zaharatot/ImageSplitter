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

namespace FilesRenameWindowLib.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для FilesRenameWindow.xaml
    /// </summary>
    public partial class FilesRenameWindow : Window
    {
        /// <summary>
        /// Маска для переименования
        /// </summary>
        public string RenameMask => RenameMaskTextBox.Text;


        /// <summary>
        /// Класс обработки хоткеев
        /// </summary>
        private HotKeyProcessor _hotKeyProcessor;



        /// <summary>
        /// Конструктор окна
        /// </summary>
        public FilesRenameWindow()
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
            _hotKeyProcessor = new HotKeyProcessor();
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
                    RenameIcon
            });

        /// <summary>
        /// Метод добавления хоткеев для окна
        /// </summary>
        private void AddHotKeys() =>
            _hotKeyProcessor.AddWindow(this, new WindowHotKeys() {
                //Добавляем список хоткеев
                HotKeys = new List<HotKeyInfo>() { 
                    //При нажатии на "Enter" - вызываем закрытие окна с успехом
                    new HotKeyInfo(Key.Enter, () => { ProcessCloseWindow(); }),
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
        /// Обработчик события нажатия на иконку запуска переименования
        /// </summary>
        private void RenameIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем метод закрытия окна
            ProcessCloseWindow();


        /// <summary>
        /// Метод проверки корректности введённой маски
        /// </summary>
        /// <returns>True - маска корректна</returns>
        private bool IsCorrectMask() =>
            //Если строка маски не пустая, и включает в себя итератор
            !string.IsNullOrEmpty(RenameMask) && RenameMask.Contains("{0}");


        /// <summary>
        /// Метод обработки корректного закрытия окна
        /// </summary>
        private void ProcessCloseWindow()
        {
            //Если маска для переименования корректна
            if (IsCorrectMask())
                //Успешно закрываем окно
                this.DialogResult = true;
            //TODO: Тут ошибку выводить нужно во всплывашке
        }
    }
}
