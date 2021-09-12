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
    /// Логика взаимодействия для SplitImagesBottomPanel.xaml
    /// </summary>
    public partial class SplitImagesBottomPanel : UserControl
    {
        /// <summary>
        /// Событие запроса на переход к изображению
        /// </summary>
        public event MoveToImageEventHandler MoveToCollectionRequest;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public SplitImagesBottomPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку "Влево"
        /// </summary>
        private void LeftPageButton_Click(object sender, RoutedEventArgs e) =>
            //Идём к предыдущей коллекции
            MoveToCollectionRequest?.Invoke(-1);

        /// <summary>
        /// Обработчик события нажатия на кнопку "Вправо"
        /// </summary>
        private void RightPageButton_Click(object sender, RoutedEventArgs e) =>
            //Идём к следующей коллекции
            MoveToCollectionRequest?.Invoke(1);



        /// <summary>
        /// Проставляем информацию о перемещённой папке
        /// </summary>
        /// <param name="newFolderName">Имя папки в которую был перемещён контролл</param>
        /// <param name="isMoved">Флаг перемещения коллекции</param>
        public void SetMovedFolderInfo(string newFolderName, bool isMoved)
        {
            //Проставляем видимость контроллу инфы о переносе
            MovedInfoTextBox.Visibility = (isMoved)
                ? Visibility.Visible : Visibility.Hidden;
            //Проставляем перемещённую папку
            MovedFolderTextBox.Text = $"[{newFolderName}]";
        }

        /// <summary>
        /// Проставляем информацию о количестве страниц
        /// </summary>
        /// <param name="pagesInfo">Инфомрация о текущих отображаемых страницах</param>
        public void SetPagesInfo(string pagesInfo) =>
            //Ставим инфу о количестве элементов в коллекции
            CountImagesTextBlock.Text = pagesInfo;

    }
}
