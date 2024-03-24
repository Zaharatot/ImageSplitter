using ImageSplitter.Content.Clases.WorkClases.Addition;
using ImageSplitter.Content.Clases.WorkClases.Helpers;
using ImageSplitter.Content.Clases.WorkClases.Helpers.Selection;
using ImageSplitter.Content.Controls.Simple;
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

namespace ImageSplitter.Content.Controls.ImageSplit.Panels
{
    /// <summary>
    /// Логика взаимодействия для LeftPanelControl.xaml
    /// </summary>
    public partial class LeftPanelControl : UserControl
    {

        /// <summary>
        /// События запроса обновления пути сканирования
        /// </summary>
        public event EmptyEventHandler UpdateSplitPathRequest;
        /// <summary>
        /// События запуска сканирования сплита
        /// </summary>
        public event EmptyEventHandler StartSplitScan;
        /// <summary>
        /// Событие отображения древа
        /// </summary>
        public event EmptyEventHandler ShowTreeRequest;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public LeftPanelControl()
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
                LoadIcon, TreeIcon, ScanIcon
            });



        /// <summary>
        /// Обработчик события клика по иконке загрузки
        /// </summary>
        private void LoadIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            UpdateSplitPathRequest?.Invoke();

        /// <summary>
        /// Обработчик события клика по иконке отображения древа
        /// </summary>
        private void TreeIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            ShowTreeRequest?.Invoke();

        /// <summary>
        /// Обработчик события клика по иконке запуска сканирования
        /// </summary>
        private void ScanIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            StartSplitScan?.Invoke();
    }
}
