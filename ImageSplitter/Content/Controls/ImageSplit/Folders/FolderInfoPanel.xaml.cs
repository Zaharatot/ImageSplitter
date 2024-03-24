using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases.Resources;
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
using static ImageSplitter.Content.Clases.DataClases.Global.Enums;

namespace ImageSplitter.Content.Controls.ImageSplit.Folders
{
    /// <summary>
    /// Логика взаимодействия для FolderInfoPanel.xaml
    /// </summary>
    public partial class FolderInfoPanel : UserControl
    {
        /// <summary>
        /// Статус сплита элемента
        /// </summary>
        public bool IsNotSplitted => _element.IsNotSplitted;

        /// <summary>
        /// Информация об элемент
        /// </summary>
        private TreeElementInfo _element;
        /// <summary>
        /// Словарь цветов папки
        /// </summary>
        private Dictionary<DirectoryStatuses, SolidColorBrush> _dirStatusColorsDict;




        /// <summary>
        /// Конструктор контролла
        /// </summary>
        /// <param name="element">Информация об элементе</param>
        public FolderInfoPanel(TreeElementInfo element)
        {
            InitializeComponent();
            Init(element);
        }


        /// <summary>
        /// Игициализатор контролла
        /// </summary>
        /// <param name="element">Информация об элементе</param>
        private void Init(TreeElementInfo element)
        {
            //Грузим словарь кистей
            _dirStatusColorsDict = LoadColorsDict();
            //Сохраняем переданную информацию об элементе
            _element = element;
            //Проставляем значения в контроллы
            FolderNameRun.Text = element.Name;
            //Обновляем дочерние элементы
            UpdateChildsInfo();
        }

        /// <summary>
        /// Инициализируем словарь цветов
        /// </summary>
        /// <returns>Словарь соответствия цветов статусам</returns>
        private Dictionary<DirectoryStatuses, SolidColorBrush> LoadColorsDict() =>
            new Dictionary<DirectoryStatuses, SolidColorBrush>() {
                //Грузим цвета по статусам
                { DirectoryStatuses.Splitted,  ResourceLoader.LoadBrush("Brush_Tree_Splitted")},
                { DirectoryStatuses.NotSplitted,  ResourceLoader.LoadBrush("Brush_Tree_NotSplitted")},
                { DirectoryStatuses.ChildsNotSplitted,  ResourceLoader.LoadBrush("Brush_Tree_ChildsNotSplitted")},
            };




        /// <summary>
        /// Метод обновления информации о дочерних элементах
        /// </summary>
        public void UpdateChildsInfo()
        {
            //Проставляем количество дочерних элементов
            CountChildsRun.Text = $" [{_element.CountChildFiles}] ";
            //Подсвечиваем красным те папки, где есть и файлы и дочерние папки
            CountChildsRun.Background = _dirStatusColorsDict[_element.Status];
        }
    }
}
