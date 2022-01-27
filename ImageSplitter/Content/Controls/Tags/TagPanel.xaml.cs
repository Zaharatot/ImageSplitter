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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ImageSplitter.Content.Clases.DataClases.Global.Delegates;

namespace ImageSplitter.Content.Controls.Tags
{
    /// <summary>
    /// Логика взаимодействия для TagPanel.xaml
    /// </summary>
    public partial class TagPanel : UserControl
    {
        /// <summary>
        /// Запрос редактирования тега
        /// </summary>
        public event TagActionRequestEventHandler EditTegRequest;
        /// <summary>
        /// Запрос удаления тега
        /// </summary>
        public event TagActionRequestEventHandler DeleteTegRequest;

        /// <summary>
        /// ВОзвращаем идентификатор тега
        /// </summary>
        public uint TagId => _tag.Id;

        /// <summary>
        /// Привязанный тег
        /// </summary>
        private Tag _tag;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public TagPanel()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем дефолтные значения
            _tag = new Tag();
        }

        /// <summary>
        /// Обработчик события запроса на удаление тега
        /// </summary>
        private void RemoveIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            DeleteTegRequest?.Invoke(_tag.Id);

        /// <summary>
        /// Обработчик события запроса на редактирование тега
        /// </summary>
        private void EditIcon_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            EditTegRequest?.Invoke(_tag.Id);

        /// <summary>
        /// Проставляем нвоый тег
        /// </summary>
        /// <param name="tag">Текст тега</param>
        public void SetNewTag(Tag tag) 
        { 
            //Проставляем переданное значение
            _tag = tag;
            //ПРоставляем текст тега
            TagNameToolTip.Text = tag.GetTagText();
            TagNameRun.Text = tag.Name;
            TagLetterRun.Text = tag.GetTagLetter();
        }

        /// <summary>
        /// Проставляем параметры панели
        /// </summary>
        /// <param name="isEditable">Флаг иконки редактирования</param>
        /// <param name="isRemoveable">Флаг иконки удаления</param>
        public void SetProperties(bool isEditable, bool isRemoveable)
        {
            //Проставляем ширину столбцов
            DeleteColumn.Width = (isRemoveable) ? new GridLength(40) : new GridLength(0);
            EditColumn.Width = (isEditable) ? new GridLength(40) : new GridLength(0);
        }
    }
}
