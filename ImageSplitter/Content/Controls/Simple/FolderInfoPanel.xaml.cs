using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для FolderInfoPanel.xaml
    /// </summary>
    public partial class FolderInfoPanel : UserControl
    {
        /// <summary>
        /// Фцвет поля количества файлов
        /// </summary>
        public SolidColorBrush CountFilesColor
        {
            get => (SolidColorBrush)CountChildsRun.Background;
            set => CountChildsRun.Background = value;
        }

        /// <summary>
        /// Количество дочерних элементов
        /// </summary>
        public string CountChilds
        {
            get => CountChildsRun.Text;
            set => CountChildsRun.Text = value;
        }

        /// <summary>
        /// Имя родительской папки
        /// </summary>
        public string FolderName
        {
            get => FolderNameRun.Text;
            set => FolderNameRun.Text = value;
        }

        /// <summary>
        /// Флаг наличия дочерних элементов
        /// </summary>
        public bool _isContainChilds = false;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public FolderInfoPanel()
        {
            InitializeComponent();
        }


    }
}
