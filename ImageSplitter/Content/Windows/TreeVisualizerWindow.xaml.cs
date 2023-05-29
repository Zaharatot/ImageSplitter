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
        /// Инициализируем элемент древа по элементу
        /// </summary>
        /// <param name="dir">Родительская папка</param>
        /// <returns>Элемент древа</returns>
        private TreeViewItem CreateElem(DirectoryInfo dir) {
            //Количество дочерних файлов
            int countChildFiles = dir.GetFiles().Length;
            int countChildDirectories = dir.GetDirectories().Length;
            //Возвращаем созданный контролл
            return new TreeViewItem() {
                //В заголовок пишем имя папки и количество дочерних файлов
                Header = new FolderInfoPanel() {
                    //Подсвечиваем красным те папки, где есть и файлы и дочерние папки
                    CountFilesColor = ((countChildDirectories != 0) && (countChildFiles != 0)) ? Brushes.LightCoral : Brushes.LightGreen,
                    CountChilds = $" [{countChildFiles}] ",
                    FolderName = dir.Name
                }
            };
        }

        /// <summary>
        /// Инициализируем элемент просмотра древа
        /// </summary>
        /// <returns>Элемент просмотра древа</returns>
        private TreeView CreateTreePanel() =>
            new TreeView() {
                //Добавляем отсутп для древа
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                MaxHeight = 400
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
        private void AddElements(DirectoryInfo current)
        {
            //Создаём новый элемент
            TreeView tree = CreateTreePanel();
            //Создаём для него корневой дочерний элемент
            TreeViewItem root = CreateElem(current);
            //Добавляем рекурсивно дочерние
            AddElementsRecurse(current, root);
            //Добавляем корневой элемент на контролл
            tree.Items.Add(root);
            //Добавляем древо на панель
            FoldersWrapPanel.Children.Add(tree);
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
                    //Чистим панель от элементов
                    FoldersWrapPanel.Children.Clear();
                    //Инициализируем родительскую папку
                    DirectoryInfo parent = new DirectoryInfo(path);
                    //Если папка существует
                    if (parent.Exists)
                        //Проходимся по корневым дочерним папкам
                        foreach (DirectoryInfo dir in parent.GetDirectories())
                            //Добавляем элементы на панель
                            AddElements(dir);
                }
            }
            catch { }
        }
    }
}
