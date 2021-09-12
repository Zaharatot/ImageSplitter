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

namespace ImageSplitter.Content.Controls.ImageSplit
{
    /// <summary>
    /// Логика взаимодействия для SplitParamsControl.xaml
    /// </summary>
    public partial class SplitParamsControl : UserControl
    {
        /// <summary>
        /// События запуска сплита
        /// </summary>
        public event StartSplitScanEventHandler StartSplitScan;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public SplitParamsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку сканирования
        /// </summary>
        private void ScanButton_Click(object sender, RoutedEventArgs e) =>
            //ВЫзываем внешний ивент, передавая в него данные
            StartSplitScan?.Invoke(ScanPathTextBox.Text, MovePathTextBox.Text, GetCheckBoxState());

        /// <summary>
        /// Получаем значение статуса чекбокса
        /// </summary>
        /// <returns>True - чекбокс нажат</returns>
        private bool GetCheckBoxState() =>
            IsFolderCheckBox.IsChecked.GetValueOrDefault(false);



    }
}
