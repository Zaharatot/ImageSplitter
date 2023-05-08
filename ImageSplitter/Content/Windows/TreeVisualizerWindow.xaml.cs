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
        private TreeViewItem CreateElem(DirectoryInfo dir) =>
            new TreeViewItem() {
                Header = dir.Name
            };

        /// <summary>
        /// Добавляем элементы
        /// </summary>
        /// <param name="dir">Родительская директория</param>
        /// <param name="parent">Родительский элемент</param>
        private void AddElements(DirectoryInfo dir, TreeViewItem parent)
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
                AddElements(child, elem);
                //Добавляем в родительский
                parent.Items.Add(elem);
            }
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
                    //Чистим древо от элементов
                    FoldersTreeView.Items.Clear();
                    //Инициализируем родительскую папку
                    DirectoryInfo parent = new DirectoryInfo(path);
                    //Если папка существует
                    if (parent.Exists)
                    {
                        //Создаём новый элемент
                        TreeViewItem elem = CreateElem(parent);
                        //Добавляем рекурсивно дочерние
                        AddElements(parent, elem);
                        //Добавляем элемент на контролл
                        FoldersTreeView.Items.Add(elem);
                        //Разворачиваем элемент
                        elem.ExpandSubtree();
                    }
                }
            }
            catch { }
        }
    }
}
