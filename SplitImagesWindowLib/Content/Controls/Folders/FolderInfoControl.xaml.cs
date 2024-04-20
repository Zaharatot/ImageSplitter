using SplitterDataLib.DataClases.Global.Split;
using SplitterSimpleUI.Content.Clases.WorkClases.Controls;
using SplitterSimpleUI.Content.Clases.WorkClases.HotKey;
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
using static SplitterDataLib.DataClases.Global.Delegates;

namespace SplitImagesWindowLib.Content.Controls.Folders
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
                RemoveIcon
            });

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
