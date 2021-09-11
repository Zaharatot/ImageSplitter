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
using static ImageSplitter.Content.Clases.DataClases.Delegates;

namespace ImageSplitter.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для FileSplitParamsControl.xaml
    /// </summary>
    public partial class FileSplitParamsControl : UserControl
    {

        /// <summary>
        /// Событие запуска сплита файлов
        /// </summary>
        public event StartFileSplitEventHandler StartFileSplit;
        /// <summary>
        /// Событие запуска отмены сплита файлов
        /// </summary>
        public event StartBackEventHandler StartBack;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public FileSplitParamsControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Обработчик события нажатия на кнопку запуска сплита
        /// </summary>
        private void SplitButton_Click(object sender, RoutedEventArgs e) =>
            //Вызываем внешний ивент, передав в него все нужные параметры
            StartFileSplit?.Invoke(
                SplitPathTextBox.Text,
                int.Parse(SplitCountTextBox.Text),
                SplitChildsCheckBox.IsChecked.GetValueOrDefault(false));

        /// <summary>
        /// Обработчик события нажатия на кнопку запуска возврата файлов
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e) =>
            //Вызываем внешний ивент, передав в него все нужные параметры
            StartBack?.Invoke(SplitPathTextBox.Text);

    }
}
