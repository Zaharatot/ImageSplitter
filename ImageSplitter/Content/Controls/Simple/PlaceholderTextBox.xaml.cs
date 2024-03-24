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

namespace ImageSplitter.Content.Controls.Simple
{
    /// <summary>
    /// Логика взаимодействия для PlaceholderTextBox.xaml
    /// </summary>
    public partial class PlaceholderTextBox : UserControl
    {
        /// <summary>
        /// Обработчик события изменения текста
        /// </summary>
        public event ChangeTextEventHandler ChangeText;


        /// <summary>
        /// Текст заглушки
        /// </summary>
        public string PlaceholderText
        {
            get => PlaceholderTextBlock.Text;
            set
            {
                //Вставляем текст в плейсхолдер
                PlaceholderTextBlock.Text = value;
                //Обновляем видимость плейсхолдера
                UpdatePlaceholderVisiblity();
            }
        }
        /// <summary>
        /// Текст контента блока
        /// </summary>
        public string Text
        {
            get => ContentTextBox.Text;
            set => ContentTextBox.Text = value;
        }
        /// <summary>
        /// Начало выдления текста
        /// </summary>
        public int SelectionStart
        {
            get => ContentTextBox.SelectionStart;
            set => ContentTextBox.SelectionStart = value;
        }
        /// <summary>
        /// Длинна выдления текста
        /// </summary>
        public int SelectionLength
        {
            get => ContentTextBox.SelectionLength;
            set => ContentTextBox.SelectionLength = value;
        }


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public PlaceholderTextBox()
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
        /// Метод обновления видимости плейсхолдера
        /// </summary>
        private void UpdatePlaceholderVisiblity()
        {
            //Если текста в контролле нет
            if (string.IsNullOrEmpty(Text))
            {
                //Скрываем текстовый контролл и отображаем заглушку
                PlaceholderTextBlock.Visibility = Visibility.Visible;
                ContentTextBox.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Обработчик соыбтия клика по контроллу
        /// </summary>
        private void MainBorder_PreviewMouseDown(object sender, MouseButtonEventArgs e) =>
            //Проставляем фокус на элемент
            FocusElement();

        /// <summary>
        /// Обработчик события потери фокуса контроллом
        /// </summary>
        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e) =>
             //Обновляем видимость плейсхолдера
             UpdatePlaceholderVisiblity();


        /// <summary>
        /// Обработчик события изменения текста в контролле
        /// </summary>
        private void ContentTextBox_TextChanged(object sender, TextChangedEventArgs e) =>
            //Вызываем внешний ивент
            ChangeText?.Invoke(ContentTextBox.Text);


        /// <summary>
        /// Метод фокусировки на контролле
        /// </summary>
        public void FocusElement()
        {
            //Если фокуса не было (виден плейсхолдер)
            if (PlaceholderTextBlock.Visibility == Visibility.Visible)
            {
                //Скрываем заглушку и отображаем текстовый контролл
                PlaceholderTextBlock.Visibility = Visibility.Collapsed;
                ContentTextBox.Visibility = Visibility.Visible;
                //Проставляем фокус на текстовом контролле
                ContentTextBox.Focus();
            }
        }
    }
}
