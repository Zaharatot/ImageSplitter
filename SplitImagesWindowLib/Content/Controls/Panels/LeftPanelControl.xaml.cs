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
        /// Событие запроса сплита файлов
        /// </summary>
        public event EmptyEventHandler StartFileSplitRequest;
        /// <summary>
        /// Событие запроса переименования файлов
        /// </summary>
        public event EmptyEventHandler StartFileRenameRequest;
        /// <summary>
        /// Событие запроса поиска дубликатов
        /// </summary>
        public event EmptyEventHandler ScanDuplicatesRequest;

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
                LoadIcon, TreeIcon, ScanIcon, SplitIcon, RenameIcon, DuplicatesIcon
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

        /// <summary>
        /// Обработчик события клика по иконке запроса сплита файлов
        /// </summary>
        private void SplitIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            StartFileSplitRequest?.Invoke();

        /// <summary>
        /// Обработчик события клика по иконке запроса переименования файлов
        /// </summary>
        private void RenameIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            StartFileRenameRequest?.Invoke();

        /// <summary>
        /// Обработчик события клика по иконке запроса поиска дубликатов
        /// </summary>
        private void DuplicatesIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            ScanDuplicatesRequest?.Invoke();
    }
}
