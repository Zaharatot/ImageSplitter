using ImageSplitter.Content.Clases.WorkClases.Resources;
using ImageSplitter.Content.Controls.Simple;
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
using System.Windows.Shapes;

namespace ImageSplitter.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для TreeVisualizerWindow.xaml
    /// </summary>
    public partial class TreeVisualizerWindow : Window
    {
        /// <summary>
        /// Конструктор окна
        /// </summary>
        public TreeVisualizerWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Обработчик осбытия нажатия на кнопку обновления информации
        /// </summary>
        private void UpdatePanelButton_Click(object sender, RoutedEventArgs e) =>
            //Выполняем обновление древа 
            UpdateTree(TreePanel.Items);


        /// <summary>
        /// Обработчик осбытия нажатия на кнопку клавиатуры
        /// </summary>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Если нажали на "Escape"
            if (e.Key == Key.Escape)
                //Закрываем окно
                this.Close();
            else if (e.Key == Key.Enter)
                //Выполняем обновление древа 
                UpdateTree(TreePanel.Items);
            //Указываем, что нажатие было обработано
            e.Handled = true;
        }



        /// <summary>
        /// Метод обновления древа
        /// </summary>
        /// <param name="items">Элементы древа для перебора</param>
        private void UpdateTree(ItemCollection items)
        {
            //Проходимся по элементам древа
            foreach (TreeViewItem item in items)
            {
                //Получаем панель из заголовка, и вызываем для неё обновление дочерних
                (item.Header as FolderInfoPanel).UpdateChildsInfo();
                //Проходимся по дочерним
                UpdateTree(item.Items);
            }
        }



        /// <summary>
        /// Инициализируем элемент древа по элементу
        /// </summary>
        /// <param name="dir">Родительская папка</param>
        /// <returns>Элемент древа</returns>
        private TreeViewItem CreateElem(DirectoryInfo dir) =>
            //Возвращаем созданный контролл
            new TreeViewItem() {
                //В заголовок пишем имя папки и количество дочерних файлов
                Header = new FolderInfoPanel(dir)
            };
        

        /// <summary>
        /// Добавляем элементы
        /// </summary>
        /// <param name="dir">Родительская директория</param>
        /// <param name="parent">Родительский элемент</param>
        private void AddElementsRecurse(DirectoryInfo dir, TreeViewItem parent)
        {
            TreeViewItem elem;
            //Получаем отсортированные по имени дочерние папки
            List<DirectoryInfo> sortedDirs = dir.GetDirectories().OrderBy(ch => ch.Name).ToList();
            //Проходимся по дочерним папкам
            foreach (var child in sortedDirs)
            {
                //Создаём новый элемент
                elem = CreateElem(child);
                //Добавляем дочерние
                AddElementsRecurse(child, elem);
                //Добавляем в родительский
                parent.Items.Add(elem);
            }
        }

        /// <summary>
        /// Метод добавления элементов
        /// </summary>
        /// <param name="current">Папка для добавления</param>
        private void SetElements(DirectoryInfo current)
        {
            //Создаём для него корневой дочерний элемент
            TreeViewItem root = CreateElem(current);
            //Добавляем рекурсивно дочерние
            AddElementsRecurse(current, root);
            //Добавляем корневой элемент на контролл
            TreePanel.Items.Add(root);
            //Разворачиваем дочерние элементы
            root.ExpandSubtree();
        }




        /// <summary>
        /// Метод визуализации древа элементов
        /// </summary>
        /// <param name="path">Путь к элементу</param>
        public void VisualizeTree(string path)
        {
            try
            {
                //Если строка не пустая
                if (!string.IsNullOrEmpty(path))
                {
                    //Инициализируем родительскую папку
                    DirectoryInfo parent = new DirectoryInfo(path);
                    //Если папка существует
                    if (parent.Exists)
                        //Втыкаем элементы в панель
                        SetElements(parent);
                }
            }
            catch { }
        }
    }
}
