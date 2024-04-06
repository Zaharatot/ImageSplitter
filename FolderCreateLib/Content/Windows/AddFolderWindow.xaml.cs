using SplitterSimpleUI.Content.Clases.DataClases.HotKey;
using SplitterSimpleUI.Content.Clases.WorkClases;
using SplitterSimpleUI.Content.Controls;
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
using System.Windows.Shapes;

namespace FolderCreateWindowLib.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddFolderWindow.xaml
    /// </summary>
    public partial class AddFolderWindow : Window
    {
        /// <summary>
        /// Имя созданной папки
        /// </summary>
        public string FolderName => FolderNamePlaceholderTextBox.Text;

        /// <summary>
        /// Класс обработки хоткеев
        /// </summary>
        private HotKeyProcessor _hotKeyProcessor;

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public AddFolderWindow()
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
                AddFolderIcon
            });

        /// <summary>
        /// Метод добавления хоткеев для окна
        /// </summary>
        private void AddHotKeys() =>
            _hotKeyProcessor.AddWindow(this, new WindowHotKeys(
                //Добавляем отдельную обработку нажатия не на хоткеи, которое
                //будет вызывать простановку фокуса на поле ввода имени новой папки
                false, (e) => { FolderNamePlaceholderTextBox.FocusElement(); }) { 
                //Добавляем список хоткеев
                HotKeys = new List<HotKeyInfo>() { 
                    //При нажатии на "Enter" - вызываем закрытие окна с успешным результатом
                    new HotKeyInfo(Key.Enter, () => { this.DialogResult = true; }),
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
        /// Обработчик нажатия на кнопку создания папки
        /// </summary>
        private void AddFolderIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Закрываем текущее диалоговое окно
            this.DialogResult = true;
    }
}
