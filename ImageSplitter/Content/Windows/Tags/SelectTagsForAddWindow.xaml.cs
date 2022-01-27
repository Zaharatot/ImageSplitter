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
    /// Логика взаимодействия для SelectTagsForAddWindow.xaml
    /// </summary>
    public partial class SelectTagsForAddWindow : Window
    {

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public SelectTagsForAddWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события сборса выделения всех тегов
        /// </summary>
        private void UncheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            //Проходимся по списку контроллов чекбоксов
            foreach (CheckBox checkBox in ContentListBox.Items)
                //Сбрасываем выделение на каждом из них
                checkBox.IsChecked = false;
        }

        /// <summary>
        /// Обработчик события простановки выделения всех тегов
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e) =>
            //ЗЩакрываем окно
            this.DialogResult = true;



        /// <summary>
        /// Проставляем теги для добавления
        /// </summary>
        /// <param name="tags">Список тегов для добавления</param>
        public void SetTags(List<string> tags)
        {
            //Удаляем все старые теги
            ContentListBox.Items.Clear();
            //Проходимся по списку тегов
            foreach (var tag in tags)
                //Добавлчем в список чекбокс с именем тега
                ContentListBox.Items.Add(new CheckBox() { 
                    Content = tag,
                    IsChecked = true
                });
        }

        /// <summary>
        /// Возвращаем вписок выбранных тегов
        /// </summary>
        /// <returns>Список выбранных тегов</returns>
        public List<string> GetSelectedTags()
        {
            //Инициализируем список тегов
            List<string> ex = new List<string>();
            //Проходимся по списку контроллов чекбоксов
            foreach (CheckBox checkBox in ContentListBox.Items)
                //Если чекбокс выбран
                if (checkBox.IsChecked.GetValueOrDefault(false))
                    //Добавляем текст тега в список
                    ex.Add((string)checkBox.Content);
            //ВОзвращаем результат
            return ex;
        }
    }
}
