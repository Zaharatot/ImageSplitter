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
using static SplitImagesWindowLib.Content.Clases.DataClases.Delegates;

namespace SplitImagesWindowLib.Content.Controls.Panels
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
             ImageInfoTextBlock.Text = info;

        /// <summary>
        /// Проставляем статус активности кнопкам
        /// </summary>
        /// <param name="state">Новый статус активности</param>
        public void SettButtonsEnableState(bool state) =>
            //Проставляем статус активности в кнопки
            DownPageIcon.IsEnabled = UpPageIcon.IsEnabled = state;

    }
}
