using SplitterDataLib.DataClases.Global.Split;
using SplitterResources;
using SplitterSimpleUI.Content.Clases.WorkClases.Controls;
using SplitterSimpleUI.Content.Clases.WorkClases.HotKey;
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

namespace SplitImagesWindowLib.Content.Controls.Folders
{
    /// <summary>
    /// Логика взаимодействия для SplitSelectedFoldersControl.xaml
    /// </summary>
    public partial class SplitSelectedFoldersControl : UserControl
    {
        /// <summary>
        /// Текст для поиска папок
        /// </summary>
        private string _folderText;
        /// <summary>
        /// Текст для поиска файлов
        /// </summary>
        private string _fileText;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public SplitSelectedFoldersControl()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            // Инициализируем текстовые значения
            InitText();
        }

        /// <summary>
        /// Инициализируем текстовые значения
        /// </summary>
        private void InitText()
        {
            //Загружаем текст из ресурсов
            _folderText = ResourceLoader.LoadString("Text_SplitImagesControl_SplitSelectedFoldersControl_IsFolder_Value_Folder");
            _fileText = ResourceLoader.LoadString("Text_SplitImagesControl_SplitSelectedFoldersControl_IsFolder_Value_File");
        }

        /// <summary>
        /// Обработчик события клика мышью по Run-у
        /// </summary>
        private void Run_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Закидываем текст с контролла в буфер обмена
            UniversalMethods.GetRunTextToClipboard(sender);


        /// <summary>
        /// Получаем текст для флага папки
        /// </summary>
        /// <param name="isFolder">Значение флага папки</param>
        /// <returns>Строка текста для папки</returns>
        private string GetIsFolderStringValue(bool isFolder) =>
            //Выбираем текст по флагу
            isFolder ? _folderText : _fileText;




        /// <summary>
        /// Метод простановки информации о путях сплита
        /// </summary>
        /// <param name="info">Информация о пути</param>
        public void SetSplitPathInfo(SplitPathsInfo info)
        {
            //Проставляем значения в контроллы
            UniversalMethods.SetRunTextOrEmpty(ScanPathRun, info.ScanPath);
            UniversalMethods.SetRunTextOrEmpty(MovePathRun, info.MovePath);
            IsFolderRun.Text = GetIsFolderStringValue(info.IsFolder);
            //Обновляем текст в тултипах
            UniversalMethods.SetTooltipContent(ScanPathToolTip, info.ScanPath);
            UniversalMethods.SetTooltipContent(MovePathToolTip, info.MovePath);
        }
    }
}
