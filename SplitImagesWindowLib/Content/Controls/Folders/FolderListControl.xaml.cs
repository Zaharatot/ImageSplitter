using SplitterDataLib.DataClases.Global.Split;
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
using static SplitterDataLib.DataClases.Global.Delegates;

namespace SplitImagesWindowLib.Content.Controls.Folders
{
    /// <summary>
    /// Логика взаимодействия для FolderListControl.xaml
    /// </summary>
    public partial class FolderListControl : UserControl
    {
        /// <summary>
        /// Событие запроса на удаление папки
        /// </summary>
        public event RemoveFolderRequestEventHandler RemoveFolderRequest;
        /// <summary>
        /// Событие запроса на добавление новой папки
        /// </summary>
        public event EmptyEventHandler AddNewFolderRequest;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public FolderListControl()
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
                AddFolderIcon
            });

        /// <summary>
        /// Обработчик событяи запроса на удаление папки
        /// </summary>
        /// <param name="key">Клавиша, привязанная к целевой папке</param>
        /// <param name="folderName">Имя папки</param>
        private void FolderInfo_RemoveFolderRequest(Key key, string folderName)
        {
            //Если пользователь подтвердил удаление папки
            if (IsNeedRemoveFolder(folderName))
                //Вызываем внешний ивент
                RemoveFolderRequest?.Invoke(key, folderName);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку добавления папки
        /// </summary>
        private void AddFolderIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            AddNewFolderRequest?.Invoke();


        /// <summary>
        /// Запрос о необходимости удаления папки
        /// </summary>
        /// <param name="folderName">Имя целевой папки</param>
        /// <returns>True - папку нужно удалить</returns>
        private bool IsNeedRemoveFolder(string folderName)
        {
            //Вызываем сообщение с запросом подтверждения удаления папки
            var result = MessageBox.Show(
                $"Вы действительно хотите удалить папку \"{folderName}\" из списка?",
                "Запрос подтверждения",
                MessageBoxButton.YesNo);
            //Если пользователь нажал "Да" - то всё ок
            return (result == MessageBoxResult.Yes);
        }


        /// <summary>
        /// Создаём контролл информации о папке
        /// </summary>
        /// <param name="info">Информация о папке</param>
        /// <returns>Сгенерированный контролл</returns>
        private FolderInfoControl CreateFolderInfoControl(TargetFolderInfo info)
        {
            //Инициализируем контролл информации о папке
            FolderInfoControl folderInfo = new FolderInfoControl();
            //Добавляем обработчик событяи запроса на удаление папки
            folderInfo.RemoveFolderRequest += FolderInfo_RemoveFolderRequest;
            //Проставляем в контролл информацию о папке
            folderInfo.SetTargetFolderInfo(info);
            //Возвращаем результат
            return folderInfo;
        }



        /// <summary>
        /// Обновляем список папок
        /// </summary>
        /// <param name="folders">Список папок</param>
        public void UpdateFoldersList(List<TargetFolderInfo> folders)
        {
            //Очищаем список папок
            FoldersList.Children.Clear();
            //Проходимся по папкам
            foreach (var folder in folders)
                //Генерируем и добавляем на панель контролл информации о папке
                FoldersList.Children.Add(CreateFolderInfoControl(folder));
        }

    }
}
