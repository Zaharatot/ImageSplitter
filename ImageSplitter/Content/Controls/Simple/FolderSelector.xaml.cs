using ImageSplitter.Content.Clases.WorkClases.Addition;
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

namespace ImageSplitter.Content.Controls.Simple
{
    /// <summary>
    /// Логика взаимодействия для FolderSelector.xaml
    /// </summary>
    public partial class FolderSelector : UserControl
    {
        /// <summary>
        /// Выбранный путь
        /// </summary>
        public string Path
        {
            get => PathTextBox.Text;
            set => PathTextBox.Text = value;
        }

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public FolderSelector()
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
                BrowseIcon
            });

        /// <summary>
        /// Обработчик события нажатия на иконку выбора пути
        /// </summary>
        private void BrowseIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Инициализируем диалоговое окно выбора папки
            System.Windows.Forms.FolderBrowserDialog dialog =
                new System.Windows.Forms.FolderBrowserDialog();
            //Обновляем выбранный путь
            dialog.SelectedPath = PathTextBox.Text;
            //Если результат выбора был успешным
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //Отображаем выбранный путь
                PathTextBox.Text = dialog.SelectedPath;
        }
    }
}
