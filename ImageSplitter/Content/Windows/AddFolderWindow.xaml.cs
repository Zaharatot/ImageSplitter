using ImageSplitter.Content.Clases.WorkClases.Addition;
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
    /// Логика взаимодействия для AddFolderWindow.xaml
    /// </summary>
    public partial class AddFolderWindow : Window
    {
        /// <summary>
        /// Класс генерации имён добавляемых папок
        /// </summary>
        private FolderNameGenerator _folderNameGrnerator;
        /// <summary>
        /// Путь к родительской папке
        /// </summary>
        private string _path;

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public AddFolderWindow()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор окна
        /// </summary>
        private void Init()
        {
            //Проставляем дефолтные значения
            _path = null;
            //Инициализируем класс генерации имён добавляемых папок
            _folderNameGrnerator = new FolderNameGenerator();
            //Добавляем обработчик события нажатия на кнопку
            this.PreviewKeyDown += AddFolderWindow_PreviewKeyDown;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку
        /// </summary>
        private void AddFolderWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Если нажат "Enter"
            if (e.Key == Key.Enter)
            {
                //Проверяем результат добавления имени
                CheckAddNameResult();
                //Указываем что кнопка была обрабаотана
                e.Handled = true;
            }
            //Если был нажат "Escape"
            else if (e.Key == Key.Escape)
                //Сбрасываем текущее диалоговое окно
                this.DialogResult = false;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку создания папки
        /// </summary>
        private void AddFolderButton_Click(object sender, RoutedEventArgs e) =>
            //Проверяем результат добавления имени
            CheckAddNameResult();

        /// <summary>
        /// Отображаем ошибку имени папки
        /// </summary>
        /// <param name="resultFlag">Флаг результата для простановки ошибки</param>
        /// <param name="errorMessage">Текст сообщения об ошибке</param>
        private void ShowFolderNameError(ref bool resultFlag, string errorMessage)
        {
            //Проставляем флаг в ошибку
            resultFlag = false;
            //Выводим сообщение обю ошибке
            MessageBox.Show(errorMessage, "Ошибка!");
        }

        /// <summary>
        /// Проверка валидности вводимого имени папки
        /// </summary>
        /// <param name="name">Имя папки для проверки</param>
        /// <param name="path">Путь к родительской папке</param>
        /// <returns>True - имя папки валидно</returns>
        private bool IsFolderNameValid(string name, string path)
        {
            bool ex = true;
            //Если имя пустое
            if (string.IsNullOrEmpty(name))
                //Выводим ошибку пустого имени
                ShowFolderNameError(ref ex, "Имя папки не должно быть пустым!");
            ////Если папка уже существует
            //if (Directory.Exists($"{path}{name}"))
            //    //Выводим ошибку дубликата
            //    ShowFolderNameError(ref ex, "Папка с таким именем уже существует в текущей директории!");
            //Возвращаем результат
            return ex;
        }

        /// <summary>
        /// Проверяем результат добавления имени
        /// </summary>
        private void CheckAddNameResult()
        {
            //Если имя папки корректно
            if (IsFolderNameValid(FolderNameTextBox.Text, _path))
                //Закрываем текущее диалоговое окно
                this.DialogResult = true;
        }

        /// <summary>
        /// Проставляем дефолтное имя папки
        /// </summary>
        /// <param name="path">Путь к родительской папке</param>
        private void SetDefaultFolderName(string path)
        {
            //Создаём новое дефолтное имя для папки
            FolderNameTextBox.Text = _folderNameGrnerator.CreateFolderName(path);
            //Пытаемся проставить фокус в контролл
            FolderNameTextBox.Focus();
            //Выделяем весь текст
            FolderNameTextBox.SelectionStart = 0;
            FolderNameTextBox.SelectionLength = FolderNameTextBox.Text.Length;
        }


        
        /// <summary>
        /// Получаем имя новой папки
        /// </summary>
        /// <param name="path">Путь к родительской папке</param>
        /// <returns>Новое имя папки</returns>
        public string GetNewFolderName(string path)
        {
            string ex = null;
            //Сохраняем полученный путь к папке
            _path = path;
            //Проставляем дефолтное имя папки
            SetDefaultFolderName(path);
            //Отображаем данное окно как диалоговое
            bool? result = this.ShowDialog();
            //Если окно было успешно закрыто
            if (result.HasValue && result.Value)
                //Возвращаем введённое имя папки
                ex = FolderNameTextBox.Text;
            //Возвращаем результат
            return ex;
        }

    }
}
