using ImageSplitter.Content.Clases.DataClases.Tags;
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

namespace ImageSplitter.Content.Windows.Tags
{
    /// <summary>
    /// Логика взаимодействия для EditTagWindow.xaml
    /// </summary>
    public partial class EditTagWindow : Window
    {
        /// <summary>
        /// Текущий редактируемый тег
        /// </summary>
        private string _currentTag;

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public EditTagWindow()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор окна
        /// </summary>
        private void Init()
        {
            //Добавляем обработчик событяи нажатия на кнопку мыши
            this.KeyDown += EditTagWindow_KeyDown;
        }

        /// <summary>
        /// Обработчик событяи нажатия на кнопку мыши
        /// </summary>
        private void EditTagWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //Если нажат "Escape"
            if (e.Key == Key.Escape)
                //Закрываем окно
                this.DialogResult = true;
            //Если нажат "Enter"
            else if (e.Key == Key.Enter)
                //Выполняем сохранение изменений
                SaveChanges();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку сохранения параметров
        /// </summary>
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e) =>
            // Выполняем сохранение изменений
            SaveChanges();


        /// <summary>
        /// Выполняем сохранение изменений
        /// </summary>
        private void SaveChanges()
        {
            //Указываем, что всё ок
            this.DialogResult = true;
            //Закрываем окно
            this.Close();
        }


        /// <summary>
        /// Проставляем тег для редактирования
        /// </summary>
        /// <param name="tag">Тег для редактирования</param>
        public void SetTag(string tag)
        {
            //Запоминаем текст текущего тега
            _currentTag = tag;
            //ПРоставляем имя тега в текстовое поле
            TagNameTextBox.Text = tag;
        }

        /// <summary>
        /// Возвращаем сформированный тег
        /// </summary>
        /// <returns>Текущий тег и тег для замены</returns>
        public KeyValuePair<string, string> GetTagInfo() =>
            //Возвращаем пару тегов
            new KeyValuePair<string, string>(_currentTag, TagNameTextBox.Text);

    }
}
