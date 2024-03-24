using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit
{
    /// <summary>
    /// Класс загрузки элементов древа
    /// </summary>
    internal class TreeElementsProcessor
    {

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TreeElementsProcessor()
        {

        }

        /// <summary>
        /// Метод рекурсивной загрузки элементов древа
        /// </summary>
        /// <param name="parent">Родительский элемент</param>
        /// <param name="directory">Родительская директория</param>
        private void LoadElementsRecurse(TreeElementInfo parent, DirectoryInfo directory)
        {
            TreeElementInfo child;
            //Проходимся по дочерним папкам
            foreach (DirectoryInfo childDir in directory.GetDirectories())
            {
                //Создаём дочерний элемент
                child = new TreeElementInfo(childDir);
                //Добавляем созданный элемент в родительский
                parent.Childs.Add(child);
                //Выполняем рекурсивный вызов
                LoadElementsRecurse(child, childDir);
            }
        }

        /// <summary>
        /// Метод загрузки древа
        /// </summary>
        /// <param name="path">Путь к элементу древа</param>
        /// <returns>Корневой элемент древа</returns>
        private TreeElementInfo LoadTree(string path)
        {
            //Инициализируем родительскую папку
            DirectoryInfo directory = new DirectoryInfo(path);
            //Инициализируем родительский элемент
            TreeElementInfo parent = new TreeElementInfo(directory);
            //Выполняем рекурсивное заполнение дочерними
            LoadElementsRecurse(parent, directory);
            //Выполняем обновление дочерних элементов
            parent.UpdateInfo();
            //Возвращаем родительский элемент
            return parent;
        }



        /// <summary>
        /// Метод отображения древа
        /// </summary>
        /// <param name="path">Путь к древу</param>
        public void ShowTree(string path)
        {
            try
            {
                //Если строка не пустая
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    //Выполняем загрузку древа
                    TreeElementInfo parent = LoadTree(path);
                    //Инициализируем окно отображения
                    TreeVisualizerWindow treeVisualizerWindow = new TreeVisualizerWindow();
                    //Отображаем окно
                    treeVisualizerWindow.Show();
                    //Отображаем древо
                    treeVisualizerWindow.VisualizeTree(parent);
                }
            }
            catch { }
        }
    }
}
