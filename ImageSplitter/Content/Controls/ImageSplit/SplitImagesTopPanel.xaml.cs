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
    /// Логика взаимодействия для SplitImagesTopPanel.xaml
    /// </summary>
    public partial class SplitImagesTopPanel : UserControl
    {

        /// <summary>
        /// Событие запроса на переход к изображению
        /// </summary>
        public event MoveToImageEventHandler MoveToImageRequest;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public SplitImagesTopPanel()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Обработчик события нажатия на кнопку "Вверх"
        /// </summary>
        private void UpPageButton_Click(object sender, RoutedEventArgs e) =>
            //Переходим к предыдущему изображению в коллекции
            MoveToImageRequest?.Invoke(-1);

        /// <summary>
        /// Обработчик события нажатия на кнопку "Вниз"
        /// </summary>
        private void DownPageButton_Click(object sender, RoutedEventArgs e) =>
            //Переходим к следующему изображению в коллекции
            MoveToImageRequest?.Invoke(1);

        /// <summary>
        /// Проставляем инфорацию о коллекции в контролл
        /// </summary>
        /// <param name="info">Строка информации о коллекции</param>
        public void SetCollectionInfo(string info) =>
             ImageInfoTextBlock.Text = info;

        /// <summary>
        /// Проставляем статус активности кнопкам
        /// </summary>
        /// <param name="state">Новый статус активности</param>
        public void SettButtonsEnableState(bool state) =>
            //Проставляем статус активности в кнопки
            DownPageButton.IsEnabled = UpPageButton.IsEnabled = state;
    }
}
