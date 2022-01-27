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
    /// Логика взаимодействия для TagsListPanel.xaml
    /// </summary>
    public partial class TagsListPanel : UserControl
    {
        /// <summary>
        /// Запрос добавления тега
        /// </summary>
        public event TagActionRequestEventHandler AddTegRequest;
        /// <summary>
        /// Запрос редактирования тега
        /// </summary>
        public event TagActionRequestEventHandler EditTegRequest;
        /// <summary>
        /// Запрос удаления тега
        /// </summary>
        public event TagActionRequestEventHandler DeleteTegRequest;



        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public TagsListPanel()
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
        /// Обработчик события добавления нового тега
        /// </summary>
        private void AddTagButton_Click(object sender, RoutedEventArgs e) =>
            //Вызываем внешний ивент
            AddTegRequest?.Invoke(0);

        /// <summary>
        /// Обработчик события запроса на уджаление тега
        /// </summary>
        /// <param name="tagId">Id тега для удаления</param>
        private void Elem_DeleteTegRequest(uint tagId) =>
            //Вызываем внешний ивент
            DeleteTegRequest?.Invoke(tagId);

        /// <summary>
        /// Обработчик события запроса на редактирование тега
        /// </summary>
        /// <param name="tagId">Id тега для редактирования</param>
        private void Elem_EditTegRequest(uint tagId) =>
            //Вызываем внешний ивент
            EditTegRequest?.Invoke(tagId);


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
            //Добавляем обработчик события запроса на редактирование тега
            elem.EditTegRequest += Elem_EditTegRequest;
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
            //Удаляем обработчик события запроса на редактирование тега
            elem.EditTegRequest -= Elem_EditTegRequest;
            //Удаляем обработчик события запроса на уджаление тега
            elem.DeleteTegRequest -= Elem_DeleteTegRequest;
        }

        /// <summary>
        /// Очищаем панель тегов
        /// </summary>
        private void ClearPanel()
        {
            //Проходимся по всем панелям тегов
            foreach (TagPanel elem in TagsPanel.Children)
                //Отписываем их от ивентов
                RemoveTagPanelEvents(elem);
            //Очищаем элементы с панели
            TagsPanel.Children.Clear();
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
                TagsPanel.Children.Add(CreateTagPanel(tag));
        }

        /// <summary>
        /// Проставляем видимость по наличию идентификатора тега в списке
        /// </summary>
        /// <param name="elem">Элемент для простановки</param>
        /// <param name="ids">ИДентификаторы скрытых элеменов</param>
        private void SetElementVIsiblity(TagPanel elem, List<uint> ids) =>
            elem.Visibility = ids.Contains(elem.TagId) ? Visibility.Collapsed : Visibility.Visible;

        /// <summary>
        /// Выполняем фильтрацию тегов
        /// </summary>
        /// <param name="ids">ИДентификаторы скрытых элеменов</param>
        public void FilterTags(List<uint> ids)
        {
            //Проходимся по всем панелям тегов
            foreach (TagPanel elem in TagsPanel.Children)
                //ПРоставляем видимость тегу
                SetElementVIsiblity(elem, ids);
        }
    }
}
