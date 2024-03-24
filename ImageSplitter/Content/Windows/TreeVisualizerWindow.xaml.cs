using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases.Addition;
using ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit;
using ImageSplitter.Content.Clases.WorkClases.Resources;
using ImageSplitter.Content.Controls.ImageDuplicateScan;
using ImageSplitter.Content.Controls.ImageSplit;
using ImageSplitter.Content.Controls.ImageSplit.Folders;
using ImageSplitter.Content.Controls.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Родительский элемент древа
        /// </summary>
        private TreeElementInfo _parentTreeElement;


        /// <summary>
        /// Конструктор окна
        /// </summary>
        public TreeVisualizerWindow()
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
                UpdateTreeIcon
            });




        /// <summary>
        /// Обработчик осбытия нажатия на кнопку обновления информации
        /// </summary>
        private void UpdateTreeIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Выполняем обновление древа 
            StartUpdateTree();

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
                StartUpdateTree();
            //Указываем, что нажатие было обработано
            e.Handled = true;
        }

        /// <summary>
        /// Метод запуска обновления древа
        /// </summary>
        private void StartUpdateTree()
        {
            //Если древо уже было загружено
            if (_parentTreeElement != null)
            {
                //Инициализируем счётчик нерасспличенных дочерних
                int countNotSplitted = 0;
                //Обновляем информацию о всех дочерних элементах
                _parentTreeElement.UpdateInfo();
                //Выполняем обновление древа 
                UpdateTree(TreePanel.Items, ref countNotSplitted);
                //Проставляем количество нерасспличенных в контролл
                NotSplittedRun.Text = countNotSplitted.ToString();
            }
        }



        /// <summary>
        /// Метод обновления древа
        /// </summary>
        /// <param name="items">Элементы древа для перебора</param>
        /// <param name="countNotSplitted">Возвращаемое значение количества нерасспличенных дочерних</param>
        private void UpdateTree(ItemCollection items, ref int countNotSplitted)
        {
            FolderInfoPanel panel;
            //Проходимся по элементам древа
            foreach (TreeViewItem item in items)
            {
                //Получаем панель из заголовка, 
                panel = item.Header as FolderInfoPanel;
                //Вызываем для неё обновление дочерних
                panel.UpdateChildsInfo();
                //Проходимся по дочерним
                UpdateTree(item.Items, ref countNotSplitted);
                //Увеличиваем счётчик, если элемент не расспличен
                countNotSplitted += (panel.IsNotSplitted) ? 1 : 0;
            }
        }



        /// <summary>
        /// Инициализируем элемент древа по элементу
        /// </summary>
        /// <param name="current">Элемент для добавления</param>
        /// <returns>Элемент древа</returns>
        private TreeViewItem CreateElem(TreeElementInfo current) =>
            //Возвращаем созданный контролл
            new TreeViewItem() {
                //В заголовок пишем имя папки и количество дочерних файлов
                Header = new FolderInfoPanel(current)
            };


        /// <summary>
        /// Добавляем элементы
        /// </summary>
        /// <param name="current">Элемент для добавления</param>
        /// <param name="parent">Родительский элемент</param>
        /// <param name="countNotSplitted">Возвращаемое значение количества нерасспличенных дочерних</param>
        private void AddElementsRecurse(TreeElementInfo current, TreeViewItem parent, ref int countNotSplitted)
        {
            TreeViewItem elem;
            //Проходимся по дочерним элементам
            foreach (TreeElementInfo child in current.Childs)
            {
                //Создаём новый элемент древа
                elem = CreateElem(child);
                //Рекурсивно добавляем в него дочерние
                AddElementsRecurse(child, elem, ref countNotSplitted);
                //Добавляем созданный элемент в родительский
                parent.Items.Add(elem);
                //Увеличиваем счётчик, если элемент не расспличен
                countNotSplitted += (child.IsNotSplitted) ? 1 : 0;
            }
        }

        /// <summary>
        /// Выполняем прокрутку к элементу
        /// </summary>
        /// <param name="elem">Элемент для прокрутки</param>
        private void ScrollToElement(TreeViewItem elem) =>
            //Прокрутку вверх нужно делать только после завершения
            //разворачивания, и вот такая обёртка позволяет это сделать
            TreePanel.Dispatcher.Invoke(new Action(async () => {
                //Ждём пол секунды
                await Task.Delay(500);
                //Прокручиваем к элементу
                elem.BringIntoView(); 
            }));
        //Как альтернатива - добавление аргумента в Invoke:
        //System.Windows.Threading.DispatcherPriority.ContextIdle
        //это позволит точно дождаться завершения действия, но задержка будет большой


        /// <summary>
        /// Метод добавления элементов
        /// </summary>
        /// <param name="current">Элемент для добавления</param>
        /// <returns>Количество нерасспличенных дочерних</returns>
        private int SetElements(TreeElementInfo current)
        {
            //Инициализируем счётчик нерасспличенных дочерних
            int countNotSplitted = (current.IsNotSplitted) ? 1 : 0;
            //Создаём для него корневой дочерний элемент
            TreeViewItem root = CreateElem(current);
            //Добавляем рекурсивно дочерние
            AddElementsRecurse(current, root, ref countNotSplitted);
            //Добавляем корневой элемент на контролл
            TreePanel.Items.Add(root);
            //Разворачиваем дочерние элементы
            root.ExpandSubtree();
            //Выполняем прокрутку к корневому элементу
            ScrollToElement(root);
            //Возвращаем значение счётчика
            return countNotSplitted;
        }



        /// <summary>
        /// Метод визуализации древа элементов
        /// </summary>
        /// <param name="parent">Родительский элемент древа</param>
        public void VisualizeTree(TreeElementInfo parent)
        {
            //Сохраняем переданное значение
            _parentTreeElement = parent;
            //Втыкаем элементы в панель, и получаем количество нерасспличенных
            int countNotSplitted = SetElements(_parentTreeElement);
            //Проставляем количество нерасспличенных в контролл
            NotSplittedRun.Text = countNotSplitted.ToString();
        }

    }
}
