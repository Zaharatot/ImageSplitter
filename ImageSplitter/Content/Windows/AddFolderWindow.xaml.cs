using ImageSplitter.Content.Clases.WorkClases.Addition;
using ImageSplitter.Content.Clases.WorkClases.Helpers.Selection;
using ImageSplitter.Content.Controls.Simple;
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

namespace ImageSplitter.Content.Windows
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
            //Инициализируем события для иконок
            InitIconsEvents();
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
        /// Обработчик события нажатия на клавишу клавиатуры
        /// </summary>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Если нажат "Enter"
            if (e.Key == Key.Enter)
                //Закрываем текущее диалоговое окно
                this.DialogResult = true;
            //Если был нажат "Escape"
            else if (e.Key == Key.Escape)
                //Сбрасываем текущее диалоговое окно
                this.DialogResult = false;
            //Если нажата любая другая клавиша
            else
                //Проставляем фокус в текстовое поле
                FolderNamePlaceholderTextBox.FocusElement();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку создания папки
        /// </summary>
        private void AddFolderIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Закрываем текущее диалоговое окно
            this.DialogResult = true;
    }
}
