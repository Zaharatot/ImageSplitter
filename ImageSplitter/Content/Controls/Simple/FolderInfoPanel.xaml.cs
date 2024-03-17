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
        /// Информация о папке
        /// </summary>
        private DirectoryInfo _directory { get; set; }

        /// <summary>
        /// Флаг наличия дочерних элементов
        /// </summary>
        public bool _isContainChilds = false;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        /// <param name="directory">Информация о папке</param>
        public FolderInfoPanel(DirectoryInfo directory)
        {
            InitializeComponent();
            Init(directory);
        }


        /// <summary>
        /// Игициализатор контролла
        /// </summary>
        /// <param name="directory">Информация о папке</param>
        private void Init(DirectoryInfo directory)
        {
            //Сохраняем переданную информацию о папке
            _directory = directory;
            //Проставляем значения в контроллы
            FolderNameRun.Text = directory.Name;
            //Обновляем дечерние
            UpdateChildsInfo();
        }

        /// <summary>
        /// Проверка на доступный для работы файл
        /// </summary>
        /// <param name="file">Файл для проверки</param>
        /// <returns>True - файл не скрытый</returns>
        private bool IsNotHiddenFile(FileInfo file) =>
            !file.Attributes.HasFlag(FileAttributes.System | FileAttributes.Hidden);



        /// <summary>
        /// Метод обновления информации о дочерних элементах
        /// </summary>
        public void UpdateChildsInfo()
        {
            //Количество дочерних файлов
            int countChildFiles = _directory.GetFiles().Where(IsNotHiddenFile).Count();
            int countChildDirectories = _directory.GetDirectories().Length;
            //Проставляем количество дочерних элементов
            CountChildsRun.Text = $" [{countChildFiles}] ";
            //Подсвечиваем красным те папки, где есть и файлы и дочерние папки
            CountFilesColor = ((countChildDirectories != 0) && (countChildFiles != 0)) ? Brushes.LightCoral : Brushes.LightGreen;
        }
    }
}
