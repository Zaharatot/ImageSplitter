using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases.Helpers;
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

namespace ImageSplitter.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectSplitFoldersWinodw.xaml
    /// </summary>
    public partial class SelectSplitFoldersWinodw : Window
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
        /// Коннструктор окна
        /// </summary>
        public SelectSplitFoldersWinodw()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем дефолтное значение для пути сплита
            _splitPath = new SplitPathsInfo();
        }


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
        /// Обработчик событяи нажатия на кнопку клавиатуры
        /// </summary>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Если юзер нажал "Enter"
            if (e.Key == Key.Enter)
                //Завершаем работу окна, и по статусу нажатия клавиши "Ctrl"
                //передаём тот или иной статус запуска сплита
                CompleteWork(!IsControlPressed(e));
            //Если юзер нажал "Escape"
            else if (e.Key == Key.Escape)
                //Просто закрываем окно
                this.DialogResult = true;
        }

        /// <summary>
        /// Метод обработки пути
        /// </summary>
        /// <param name="path">Путь для обработки</param>
        /// <returns>Обработанный путь</returns>
        private string ProcessPath(string path) =>
            //Добавляем слеш на конце пути, если его нету + обработка пустой строки
            string.IsNullOrEmpty(path) ? "" : ((path.Last() != '\\') ? $"{path}\\" : path);

        /// <summary>
        /// Проверка нажатия кнопки "Ctrl" на клавиатуре
        /// </summary>
        /// <param name="e">Информация о нажатой кнопке</param>
        /// <returns>TRue - кнопка "Ctrl" была нажата</returns>
        private bool IsControlPressed(KeyEventArgs e) =>
            (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0;


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
