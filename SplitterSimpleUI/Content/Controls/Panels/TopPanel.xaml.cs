using SplitterSimpleUI.Content.Clases.WorkClases.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SplitterSimpleUI.Content.Clases.DataClases.Global.Delegates;

namespace SplitterSimpleUI.Content.Controls.Panels
{
    /// <summary>
    /// Логика взаимодействия для TopPanel.xaml
    /// </summary>
    public partial class TopPanel : UserControl
    {

        /// <summary>
        /// Событие запроса на переход к изображению
        /// </summary>
        public event MoveToImageEventHandler MoveToImageRequest;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public TopPanel()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
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
                 UpPageIcon, DownPageIcon
            });


        /// <summary>
        /// Обработчик события нажатия на кнопку "Вверх"
        /// </summary>
        private void UpPageIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Переходим к предыдущему изображению в коллекции
            MoveToImageRequest?.Invoke(-1);

        /// <summary>
        /// Обработчик события нажатия на кнопку "Вниз"
        /// </summary>
        private void DownPageIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Переходим к следующему изображению в коллекции
            MoveToImageRequest?.Invoke(1);




        /// <summary>
        /// Проставляем инфорацию о коллекции в контролл
        /// </summary>
        /// <param name="info">Строка информации о коллекции</param>
        public void SetCollectionInfo(string info) =>
            //Ставим инфу об изображении
            ImageInfoToolTip.Content = ImageInfoTextBlock.Text = info;

        /// <summary>
        /// Проставляем статус активности кнопкам
        /// </summary>
        /// <param name="state">Новый статус активности</param>
        public void SettButtonsEnableState(bool state) =>
            //Проставляем статус активности в кнопки
            DownPageIcon.IsEnabled = UpPageIcon.IsEnabled = state;

    }
}
