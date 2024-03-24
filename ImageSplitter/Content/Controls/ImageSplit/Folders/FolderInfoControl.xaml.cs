using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Split;
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

namespace ImageSplitter.Content.Controls.ImageSplit.Folders
{
    /// <summary>
    /// Логика взаимодействия для FolderInfoControl.xaml
    /// </summary>
    public partial class FolderInfoControl : UserControl
    {
        /// <summary>
        /// Событие запроса на удаление папки
        /// </summary>
        public event RemoveFolderRequestEventHandler RemoveFolderRequest;

        /// <summary>
        /// Информация об отображаемой папке
        /// </summary>
        private TargetFolderInfo _info;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public FolderInfoControl()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        private void Init()
        {
            //Проставляем дефолтные значения
            _info = new TargetFolderInfo();
        }


        /// <summary>
        /// Обработчик события наведения мыши на иконку удаления
        /// </summary>
        private void RemoveIcon_MouseEnter(object sender, MouseEventArgs e) =>
            //Обновляем отступы контролла
            RemoveIcon.Margin = new Thickness(6, 8, 6, 8);

        /// <summary>
        /// Обработчик события ухода мыши с иконки удаления
        /// </summary>
        private void RemoveIcon_MouseLeave(object sender, MouseEventArgs e) =>
            //Обновляем отступы контролла
            RemoveIcon.Margin = new Thickness(7, 10, 7, 10);

        /// <summary>
        /// Обработчик события нажатия на на иконку удаления
        /// </summary>
        private void RemoveIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем ивент запроса удаления папки
            RemoveFolderRequest?.Invoke(_info.TargetKey, _info.Name);



        /// <summary>
        /// Проставляем в контролл информацию о целевой папке
        /// </summary>
        /// <param name="info">Информация о целевой папке</param>
        public void SetTargetFolderInfo(TargetFolderInfo info)
        {
            //Запоминаем переданное значение
            _info = info;
            //Проставляем значеняи в контроллы
            FolderKeyTextBlock.Text = $"[{info.TargetKey.ToString()}]";
            FolderNameToolTip.Content = FolderNameTextBlock.Text = info.Name;
        }
    }
}
