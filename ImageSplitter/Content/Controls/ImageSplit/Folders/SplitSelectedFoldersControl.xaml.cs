using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases.Helpers;
using ImageSplitter.Content.Clases.WorkClases.Helpers.Selection;
using ImageSplitter.Content.Clases.WorkClases.Resources;
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

namespace ImageSplitter.Content.Controls.ImageSplit.Folders
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
        /// Текст для пустого значения
        /// </summary>
        private string _emptyText;


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
            _emptyText = ResourceLoader.LoadString("Text_EmptyValue");
        }

        /// <summary>
        /// Обработчик события клика мышью по Run-у
        /// </summary>
        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Типизируем элемент
            Run element = (sender as Run);
            //Если он активен
            if (element.IsEnabled)
                //Втыкаем его текст в буфер обмена
                Clipboard.SetText(element.Text);
        }


        /// <summary>
        /// Метод обработки строкового значения
        /// </summary>
        /// <param name="value">Строка для обработки</param>
        /// <returns>Обработанная строка</returns>
        private string ProcessValue(string value) =>
            //Если строка пустая - втыкаем вместо неё прочерк
            string.IsNullOrEmpty(value) ? _emptyText : value;

        /// <summary>
        /// Получаем текст для флага папки
        /// </summary>
        /// <param name="isFolder">Значение флага папки</param>
        /// <returns>Строка текста для папки</returns>
        private string GetIsFolderStringValue(bool isFolder) =>
            //Выбираем текст по флагу
            isFolder ? _folderText : _fileText;

        /// <summary>
        /// Вставляем текст в Run
        /// </summary>
        /// <param name="elem">Элемент для вставки текста</param>
        /// <param name="text">Текст для вставки</param>
        private void SetRunText(Run elem, string text)
        {
            //Вставляем текст в контролл
            elem.Text = ProcessValue(text);
            //Дизейблим контролл, если текста нету
            elem.IsEnabled = !string.IsNullOrEmpty(text);
        }



        /// <summary>
        /// Метод простановки информации о путях сплита
        /// </summary>
        /// <param name="info">Информация о пути</param>
        public void SetSplitPathInfo(SplitPathsInfo info)
        {
            //Проставляем значения в контроллы
            SetRunText(ScanPathRun, info.ScanPath);
            SetRunText(MovePathRun, info.MovePath);
            IsFolderRun.Text = GetIsFolderStringValue(info.IsFolder);
            //Обновляем текст в тултипах
            UniversalMethods.SetTooltipContent(ScanPathToolTip, info.ScanPath);
            UniversalMethods.SetTooltipContent(MovePathToolTip, info.MovePath);
        }
    }
}
