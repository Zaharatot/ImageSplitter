using SplitterSimpleUI.Content.Clases.WorkClases;
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
using static SplitterDataLib.DataClases.Global.Delegates;

namespace SplitImagesWindowLib.Content.Controls.Panels
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
        /// Событие запроса отмены переноса изображения
        /// </summary>
        public event EmptyEventHandler UndoMove;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public SplitImagesBottomPanel()
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
                LeftPageIcon, RightPageIcon, UndoMoveIcon
            });

        /// <summary>
        /// Обработчик события нажатия на кнопку "Влево"
        /// </summary>
        private void LeftPageIcon_MouseDown(object sender, MouseButtonEventArgs e)=>
            //Идём к предыдущей коллекции
            MoveToCollectionRequest?.Invoke(-1);

        /// <summary>
        /// Обработчик события нажатия на кнопку "Вправо"
        /// </summary>
        private void RightPageIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Идём к следующей коллекции
            MoveToCollectionRequest?.Invoke(1);


        /// <summary>
        /// Обработчик события нажатия на кнопку отмены переноса
        /// </summary>
        private void UndoMoveIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            UndoMove?.Invoke();



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
            //Обновляем активность кнопки отмены переноса
            UndoMoveIcon.IsEnabled = isMoved;
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
