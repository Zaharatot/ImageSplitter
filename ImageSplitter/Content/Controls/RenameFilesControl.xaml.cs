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

namespace ImageSplitter.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для RenameFilesControl.xaml
    /// </summary>
    public partial class RenameFilesControl : UserControl
    {
        /// <summary>
        /// Событие запуска переименования файлов
        /// </summary>
        public event RenameFilesEventHandler RenameFiles;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public RenameFilesControl()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        private void Init()
        {
            //Проставляем дефолтную маску переименования файлов
            RenameMaskTextBox.Text = "{0}";
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку запуска переименования
        /// </summary>
        private void RenameButton_Click(object sender, RoutedEventArgs e) =>
            //Запускаем переименование
            RenameFiles?.Invoke(RenamePathTextBox.Path, RenameMaskTextBox.Text);
    }
}
