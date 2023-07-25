﻿using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases.Addition;
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
using System.Windows.Shapes;

namespace ImageSplitter.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectFoldersWindow.xaml
    /// </summary>
    public partial class SelectFoldersWindow : Window
    {
        /// <summary>
        /// Класс поиска клавиш по id папки
        /// </summary>
        private KeyFinder _keyFinder;

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public SelectFoldersWindow()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор окна
        /// </summary>
        private void Init()
        {
            //Инициализируем класс поиска клавиш
            _keyFinder = new KeyFinder();
        }



        /// <summary>
        /// Обработчик события клика по кнопке "Продолжить"
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e) =>
            //ЗЩакрываем окно
            this.DialogResult = true;

        /// <summary>
        /// Обработчик события клика по кнопке сброса выделения
        /// </summary>
        private void UncheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            //Проходимся по списку контроллов чекбоксов
            foreach (CheckBox checkBox in CheckBoxesPanel.Children)
                //Сбрасываем выделение на каждом из них
                checkBox.IsChecked = false;
        }

        /// <summary>
        /// Метод проверки наличия старых папок в новом списке
        /// </summary>
        /// <param name="folders">Новый список папок</param>
        /// <param name="targets">Текущий список выбранных папок</param>
        /// <returns>Старые папки есть в списке</returns>
        private bool IsContainOldFolders(List<TargetFolderInfo> folders, List<TargetFolderInfo> targets)
        {
            //Если старых папок нет
            if (targets.Count == 0)
                //Возвращаем успех
                return true;

            //Проходимся по списку папок
            foreach (var folder in folders)
                //Если папка есть в старом списке
                if (targets.Any(dir => dir.Name.Equals(folder.Name)))
                    //Возвращаем флаг нахождения
                    return false;
            //Если ни одной папки не совпало - возвращаем флаг отсутствия
            return true;
        }

        /// <summary>
        /// Метод создания контролла чекбокса
        /// </summary>
        /// <param name="content">Текстовый контент контролла</param>
        /// <param name="isEmpty">Флаг пустого контролла</param>
        /// <param name="targets">Текущий список выбранных папок</param>
        /// <returns>Созданный контролл</returns>
        private CheckBox CreateFolderControl(string content, bool isEmpty, List<TargetFolderInfo> targets) =>
            new CheckBox() {
                Content = content,
                IsChecked = isEmpty || targets.Any(dir => dir.Name.Equals(content)),
                Foreground = ResourceLoader.LoadBrush("Brush_ForegroundColor"),
                Style = ResourceLoader.LoadStyle("Style_CheckBox")
            };

        /// <summary>
        /// Обновляем список папок
        /// </summary>
        /// <param name="folders">Новый список папок</param>
        /// <param name="targets">Текущий список выбранных папок</param>
        private void UpdateFoldersList(List<TargetFolderInfo> folders, List<TargetFolderInfo> targets)
        {
            //Получаем флаг пустого 
            bool isEmpty = IsContainOldFolders(folders, targets);
            //Очищаем список от старых элементов
            CheckBoxesPanel.Children.Clear();
            //Добавляем в список чекбоксы по списку папок
            foreach (var folder in folders)
                //Создаём контролл и добавляем на панель
                CheckBoxesPanel.Children.Add(
                    CreateFolderControl(folder.Name, isEmpty, targets));
        }

        /// <summary>
        /// Возвращаем список выбранных папок
        /// </summary>
        /// <param name="folders">Новый список папок</param>
        /// <returns>Список выбранных папок</returns>
        private List<TargetFolderInfo> GetSelectedFolders(List<TargetFolderInfo> folders)
        {
            List<TargetFolderInfo> ex = new List<TargetFolderInfo>();
            CheckBox buff;
            int folderId = 0;
            //Проходимся по папкам
            for (int i = 0; i < folders.Count; i++)
            {
                //Получаем чекбокс из списка
                buff = CheckBoxesPanel.Children[i] as CheckBox;
                //Если галочка стоит
                if (buff.IsChecked.HasValue && buff.IsChecked.Value)
                {
                    //Проставляем папке код клавиши
                    folders[i].TargetKey = _keyFinder.GetKeyByNumber(folderId++);
                    //Добавляем папку в выходной список
                    ex.Add(folders[i]);
                }
            }
            return ex;
        }



        /// <summary>
        /// Получаем список папок для работы
        /// </summary>
        /// <param name="folders">Список папок для обработки</param>
        /// <param name="targets">Текущий список выбранных папок</param>
        /// <returns>Список выбранных папок</returns>
        public List<TargetFolderInfo> GetFoldersToWork(List<TargetFolderInfo> folders, List<TargetFolderInfo> targets)
        {
            //Обновляем список папок
            UpdateFoldersList(folders, targets);
            //Отображаем данное окно как диалоговое
            bool? result = this.ShowDialog();
            //Если окно было успешно закрыто
            if (result.HasValue && result.Value)
                //ПОлучаем полный список папок
                return GetSelectedFolders(folders);
            //Если окно было отменено
            else
                //Возвращаем пустой список папок
                return new List<TargetFolderInfo>();
        }
    }
}
