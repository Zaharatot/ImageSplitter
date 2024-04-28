using SplitterDataLib.DataClases.Global.Split;
using SplitterSimpleUI.Content.Clases.DataClases.HotKey;
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
using System.Windows.Shapes;

namespace SplitPathWindowLib.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectPathsWinodw.xaml
    /// </summary>
    public partial class SelectPathsWinodw : Window
    {

        /// <summary>
        /// Информация о выбранном пути для сплита
        /// </summary>
        public SplitPathsInfo SplitPath 
        {
            get => _splitPath;
            set => SetSplitPath(value);
        }




        /// <summary>
        /// Информация о выбранном пути для сплита
        /// </summary>
        public SplitPathsInfo _splitPath;

        /// <summary>
        /// Класс обработки хоткеев
        /// </summary>
        private HotKeyProcessor _hotKeyProcessor;


        /// <summary>
        /// Коннструктор окна
        /// </summary>
        public SelectPathsWinodw()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем хоткеи
            InitHotkeys();
            //Инициализируем дефолтное значение для пути сплита
            _splitPath = new SplitPathsInfo();
            //Проставляем фокус на первый контролл
            ScanPathFolderSelector.FocusElement();
        }

        /// <summary>
        /// Инициализируем хоткеи
        /// </summary>
        private void InitHotkeys()
        {
            //Получаем экземпляр класса обработки хоткеев
            _hotKeyProcessor = new HotKeyProcessor();
            //Добавляем хоткеи для текущего окна
            AddHotKeys();
        }

        /// <summary>
        /// Метод добавления хоткеев для окна
        /// </summary>
        private void AddHotKeys() =>
            _hotKeyProcessor.AddWindow(this, new WindowHotKeys() {
                //Добавляем список хоткеев
                HotKeys = new List<HotKeyInfo>() { 
                    //При нажатии на "Ctrl + Enter" - вызываем закрытие окна
                    new HotKeyInfo(Key.Enter, () => { CompleteWork(true); }, true),
                    //При нажатии на "Enter" - вызываем закрытие окна с запуском сплита
                    new HotKeyInfo(Key.Enter, () => { CompleteWork(false); }),
                    //При нажатии на "Escape" - просто закрываем окно
                    new HotKeyInfo(Key.Escape, () => { this.DialogResult = true; }),
                }
            });


        /// <summary>
        /// Обработчик осбытия закрытия окна
        /// </summary>
        private void Window_Closed(object sender, EventArgs e) =>
            //Удаляем обюработку хоткеев для данного окна
            _hotKeyProcessor.RemoveWindow(this);

        /// <summary>
        /// Обработчик события нажатия на кнопку выбора путей
        /// </summary>
        private void SelectPathsButton_Click(object sender, RoutedEventArgs e) =>
            //Завершаем работу окна
            CompleteWork(false);

        /// <summary>
        /// Обработчик события нажатия на кнопку выбора путей и запуска сплита
        /// </summary>
        private void SelectPathsAndSplitButton_Click(object sender, RoutedEventArgs e) =>
            //Завершаем работу окна
            CompleteWork(true);


        /// <summary>
        /// Метод обработки пути
        /// </summary>
        /// <param name="path">Путь для обработки</param>
        /// <returns>Обработанный путь</returns>
        private string ProcessPath(string path) =>
            //Добавляем слеш на конце пути, если его нету + обработка пустой строки
            string.IsNullOrEmpty(path) ? "" : ((path.Last() != '\\') ? $"{path}\\" : path);


        /// <summary>
        /// Выполняем завершение работы окна
        /// </summary>
        /// <param name="isStartSplit">Флаг запуска сплита</param>
        private void CompleteWork(bool isStartSplit)
        {
            //Прставляем пути и флаги в выходной параметр
            _splitPath.ScanPath = ProcessPath(ScanPathFolderSelector.Path);
            _splitPath.MovePath = ProcessPath(MovePathFolderSelector.Path);
            _splitPath.IsFolder = IsFolderCheckBox.IsChecked;
            _splitPath.IsStartSplit = isStartSplit;
            //Выполняем закрытие окна
            this.DialogResult = true;
        }

        /// <summary>
        /// Метод вставки пути в контролл
        /// </summary>
        /// <param name="splitPath">Путь для вставки</param>
        private void SetSplitPath(SplitPathsInfo splitPath)
        {
            //Проставляем переданное значение
            _splitPath = splitPath;
            //Передаём значения в контроллы
            ScanPathFolderSelector.Path = splitPath.ScanPath;
            MovePathFolderSelector.Path = splitPath.MovePath;
            IsFolderCheckBox.State = UniversalMethods
                .GetComboCheckBoxStateByFlag(splitPath.IsFolder);
        }
    }
}
