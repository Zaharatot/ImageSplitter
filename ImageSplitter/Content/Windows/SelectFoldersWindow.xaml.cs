using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases.Helpers;
using ImageSplitter.Content.Clases.WorkClases.Resources;
using ImageSplitter.Content.Controls.Simple;
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
using static ImageSplitter.Content.Clases.DataClases.Global.Enums;

namespace ImageSplitter.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectFoldersWindow.xaml
    /// </summary>
    public partial class SelectFoldersWindow : Window
    {


        /// <summary>
        /// Конструктор окна
        /// </summary>
        public SelectFoldersWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Обработчик события клика по кнопке "Продолжить"
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e) =>
            //Закрываем окно
            this.DialogResult = true;

        /// <summary>
        /// Обработчик события клика по глобальному чекбоксу
        /// </summary>
        private void GlobalComboCheckBox_CheckBoxUpdateState(ComboCheckBoxStates state)
        {
            //Проходимся по списку контроллов чекбоксов
            foreach (ComboCheckBoxControl checkBox in CheckBoxesPanel.Children)
                //Проставляем соответствующий статус дочерним
                checkBox.State = state;
            //Обновляем подсказку по статусу
            GlobalComboCheckBox.Header = LoadStateText(state);
        }


        /// <summary>
        /// Обработчик события изменения статуса чекбокса
        /// </summary>
        /// <param name="state">Новый статус чекбокса</param>
        private void Elem_CheckBoxUpdateState(ComboCheckBoxStates state) =>
            //Обновляем статус глобального чекбокса 
            UpdateGlobalCheckBoxState();



        /// <summary>
        /// Метод обновления статуса глобального чекбокса
        /// </summary>
        private void UpdateGlobalCheckBoxState()
        {
            //Обновляем глобальный статус в чекбоксе
            GlobalComboCheckBox.State = GetGlobalStateFromPanel();
            //Обновляем подсказку по статусу
            GlobalComboCheckBox.Header = LoadStateText(GlobalComboCheckBox.State);
        }

        /// <summary>
        /// Метод загрузки текста подсказки по статусу
        /// </summary>
        /// <param name="state">Статус для получения подсказки</param>
        /// <returns>Подсказка для статуса</returns>
        private string LoadStateText(ComboCheckBoxStates state) =>
            //Грузим текст подсказки по статусу
            ResourceLoader.LoadString($"Text_SelectFoldersWindow_GlobalComboCheckBox_Status_{state}");

        /// <summary>
        /// Метод получения количества выбранных элементов
        /// </summary>
        /// <returns>Количество выбранных элементов</returns>
        private int GetCountChecked()
        {
            //Инициализируем счётчик
            int countChecked = 0;
            //Проходимся по списку контроллов чекбоксов
            foreach (ComboCheckBoxControl checkBox in CheckBoxesPanel.Children)
                //Увеличиваем счётчик, если чекбокс чекнут
                countChecked += (checkBox.IsChecked) ? 1 : 0;
            //Возвращаем количество выбранных элементов
            return countChecked;
        }

        /// <summary>
        /// Метод получения глобального статуса из панели
        /// </summary>
        /// <returns>Глобальный статус по панели</returns>
        private ComboCheckBoxStates GetGlobalStateFromPanel()
        {
            //Получаем количество выбранных элементов
            int countChecked = GetCountChecked();
            //Если все элементв выбраны
            if (CheckBoxesPanel.Children.Count == countChecked)
                return ComboCheckBoxStates.Checked;
            //Если не выбрано не одного
            else if (countChecked == 0)
                return ComboCheckBoxStates.Unchecked;
            //Если результат - смешанный
            else
                return ComboCheckBoxStates.Partial;
        }

        /// <summary>
        /// Метод создания контролла чекбокса
        /// </summary>
        /// <param name="info">Информация о папке, для которой создаём контролл</param>
        /// <returns>Созданный контролл</returns>
        private ComboCheckBoxControl CreateFolderControl(TargetFolderInfo info)
        {
            //Инициализируем контролл чекбокса
            ComboCheckBoxControl elem = new ComboCheckBoxControl();
            //Проставляем базовые параметры чекбокса
            elem.Tooltip = elem.Header = info.Name;
            elem.Margin = new Thickness(5);
            //Проставляем статус чекбокса по флагу
            elem.State = UniversalMethods.GetComboCheckBoxStateByFlag(info.IsSelected);
            //Добавляем обработчик события изменения статуса чекбокса
            elem.CheckBoxUpdateState += Elem_CheckBoxUpdateState;
            //Возвращаем созданный контролл
            return elem;
        }

        /// <summary>
        /// Метод очистки панели
        /// </summary>
        private void ClearPanel()
        {
            //Проходимся по списку контроллов чекбоксов
            foreach (ComboCheckBoxControl checkBox in CheckBoxesPanel.Children)
                //Удаляем обработчик события изменения статуса чекбокса
                checkBox.CheckBoxUpdateState -= Elem_CheckBoxUpdateState;
            //Очищаем список от старых элементов
            CheckBoxesPanel.Children.Clear();
        }



        /// <summary>
        /// Возвращаем список выбранных папок
        /// </summary>
        /// <param name="folders">Новый список папок</param>
        /// <returns>Список выбранных папок</returns>
        public void GetSelectedFolders(ref List<TargetFolderInfo> folders)
        {
            //Инициализируем id папки
            int folderId = 0;
            //Проходимся по чекбоксам (их порядок такой же как у папок
            foreach (ComboCheckBoxControl checkBox in CheckBoxesPanel.Children)
                //Проставляем для папок флаг выбора
                folders[folderId++].IsSelected = checkBox.IsChecked;
        }

        /// <summary>
        /// Обновляем список папок
        /// </summary>
        /// <param name="folders">Новый список папок</param>
        public void UpdateFoldersList(List<TargetFolderInfo> folders)
        {
            //Очищаем список от старых элементов
            ClearPanel();
            //Добавляем в список чекбоксы по списку папок
            foreach (var folder in folders)
                //Создаём контролл и добавляем на панель
                CheckBoxesPanel.Children.Add(CreateFolderControl(folder));
            //Обновляем статус глобального чекбокса 
            UpdateGlobalCheckBoxState();
        }
    }
}
