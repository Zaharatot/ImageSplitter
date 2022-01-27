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
    /// Логика взаимодействия для TagsPanel.xaml
    /// </summary>
    public partial class TagsPanel : UserControl
    {
        /// <summary>
        /// Запрос удаления тега
        /// </summary>
        public event TagActionRequestEventHandler DeleteTegRequest;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public TagsPanel()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        private void Init()
        {

        }


        /// <summary>
        /// Обработчик события запроса на уджаление тега
        /// </summary>
        /// <param name="tagId">Id тега для удаления</param>
        private void Elem_DeleteTegRequest(uint tagId) =>
            //Вызываем внешний ивент
            DeleteTegRequest?.Invoke(tagId);




        /// <summary>
        /// Формируем контролл тега
        /// </summary>
        /// <param name="tag">Инфомрация о теге</param>
        /// <returns>Контролл тега</returns>
        private TagPanel CreateTagPanel(Tag tag)
        {
            //Инициализируем контролл панели
            TagPanel elem = new TagPanel();
            //Проставляем тег в контролл
            elem.SetNewTag(tag);
            //Проставляем параметры панели
            elem.SetProperties(false, true);
            //Добавляем обработчик события запроса на уджаление тега
            elem.DeleteTegRequest += Elem_DeleteTegRequest;
            //Возвращаем результат
            return elem;
        }

        /// <summary>
        /// Удаляем ивенты панели тега
        /// </summary>
        /// <param name="elem">Панель тега для удаления ивентов</param>
        private void RemoveTagPanelEvents(TagPanel elem)
        {
            //Удаляем обработчик события запроса на уджаление тега
            elem.DeleteTegRequest -= Elem_DeleteTegRequest;
        }

        /// <summary>
        /// Очищаем панель тегов
        /// </summary>
        private void ClearPanel()
        {
            //Проходимся по всем панелям тегов
            foreach (TagPanel elem in TagsWrapPanel.Children)
                //Отписываем их от ивентов
                RemoveTagPanelEvents(elem);
            //Очищаем элементы с панели
            TagsWrapPanel.Children.Clear();
        }


        /// <summary>
        /// ПРоставляем список тегов
        /// </summary>
        /// <param name="tags">Список тегов для добавления</param>
        public void SetTagList(List<Tag> tags)
        {
            //Очищаем панель тегов
            ClearPanel();
            //Проходимся по переданным тегам
            foreach (Tag tag in tags)
                //Добавляем тег на панель
                TagsWrapPanel.Children.Add(CreateTagPanel(tag));
        }


    }
}
