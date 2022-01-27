using ImageSplitter.Content.Clases.DataClases.Tags;
using ImageSplitter.Content.Clases.WorkClases.Processors.Tags.Service;
using ImageSplitter.Content.Windows.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.Tags
{
    /// <summary>
    /// Класс работы с тегами
    /// </summary>
    internal class TagsWork
    {
        /// <summary>
        /// Класс загрузки списка тегов
        /// </summary>
        private TagsLoader _tagsLoader;

        /// <summary>
        /// Список рабочих тегов
        /// </summary>
        public List<string> TagList { get; private set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TagsWork()
        {

            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _tagsLoader = new TagsLoader();
            //Загружаем список тегов
            TagList = _tagsLoader.LoadTags();
        }

        /// <summary>
        /// Выполняем сохранение новых тегов
        /// </summary>
        /// <param name="newTags">Список новых тегов</param>
        private void SaveNewTags(List<string> newTags)
        {     
            //Инициализируем окно добавления тегов
            SelectTagsForAddWindow selectTagsForAddWindow = new SelectTagsForAddWindow();
            //ЗАкидываем в него все теги, которые необходимо добавить
            selectTagsForAddWindow.SetTags(newTags);
            //Отображаем окно выбора тегов
            bool? result = selectTagsForAddWindow.ShowDialog();
            //Если результат из окна был получен успешно
            if (result.GetValueOrDefault(false))
            {
                //Получаем список тегов для добавления
                List<string> tagsToAdd = selectTagsForAddWindow.GetSelectedTags();
                //Добавляем их в общий список
                TagList.AddRange(tagsToAdd);
                //Сохраняем теги
                _tagsLoader.SaveTags(TagList);
                //Выводим сообщение
                MessageBox.Show("Теги были успешно сохранены. Все теги, которые не были добавлены в список будут удалены при сохранении изменений!");
            }
            //В противном случае
            else
                //Выводим соответствующее сообщение
                MessageBox.Show("Добавление тегов было отменено. Все теги, которые не были добавлены в список будут удалены при сохранении изменений!");
        }

        /// <summary>
        /// Удаляем теги у изображений, которых нет в списке
        /// </summary>
        /// <param name="images">Список загруженных изображений</param>
        private void RemoveMissingTags(List<TaggedImage> images) =>
            //Проходимся по всем изображениям
            images.ForEach(image =>
                //Удаляем у них теги, которых нет в общем списке тегов
                image.Tags.RemoveAll(tag => !TagList.Contains(tag)));


        /// <summary>
        /// Проверяем пару тегов на ошибки
        /// </summary>
        /// <param name="tagPair">Пара тегов для проверки</param>
        /// <returns>True - всё ок</returns>
        private bool CheckTagPair(KeyValuePair<string, string> tagPair)
        {
            //По дефолту вернём ошибку
            bool ex = false;
            //Если тегш слишком короткий
            if ((tagPair.Value == null) || (tagPair.Value.Length < 2))
                //Выводим соответствующее сообщение
                MessageBox.Show("Тег слишком короткий!");
            //Если тег не был изменён
            else if (tagPair.Value == tagPair.Key)
                //Выводим соответствующее сообщение
                MessageBox.Show("Тег не был изменён!");
            //Если тег уже был добавлен в список
            else if (TagList.Contains(tagPair.Value))
                //Выводим соответствующее сообщение
                MessageBox.Show("Такой тег уже есть в списке!");
            //В противном случае
            else
                //Всё ок
                ex = true;
            //Возвращаем результат
            return ex;
        }

        /// <summary>
        /// Выполняем замену тегов в переданном списке
        /// </summary>
        /// <param name="tags">Список тегов для замены</param>
        /// <param name="tagPair">Пара из заменяемого и заменяющего тегов</param>
        private void ReplaceTag(List<string> tags, KeyValuePair<string, string> tagPair)
        {
            //Удаляем старый тег из списка
            tags.Remove(tagPair.Key);
            //Добавляем новый тег в список
            tags.Add(tagPair.Value);
        }




        /// <summary>
        /// Получаем список уникальных тегов из списка изображений
        /// </summary>
        /// <param name="images">Список загруженных изображений</param>
        /// <returns>Список тегов</returns>
        public List<string> GetUnicalTags(List<TaggedImage> images)
        {
            //Инициализируем список для тегов
            List<string> ex = new List<string>();
            //Добавляем все теги из картинок в список
            images.ForEach(image => ex.AddRange(image.Tags));
            //Возвращаем только уникальные теги
            return ex.Distinct().ToList();
        }

        /// <summary>
        /// Выполняем обновленик коллекции тегов
        /// </summary>
        /// <param name="images">Список загруженных изображений</param>
        public void UpdateTags(List<TaggedImage> images)
        {
            //Получаем все уникальные теги из загруженных картинок
            List<string> tags = GetUnicalTags(images);
            //Получаем теги, которых нет в основном списке
            List<string> newTags = tags.Except(TagList).ToList();
            //Выполняем сохранение новых тегов
            SaveNewTags(newTags);
            //Удаляем теги у изображений, которых нет в списке
            RemoveMissingTags(images);
        }

        /// <summary>
        /// Выполняем добавление тега
        /// </summary>
        public void AddTag()
        {
            //Инициализируем окно добавления тега
            EditTagWindow editTagWindow = new EditTagWindow();
            //Получаем результат добавления тега
            bool? result = editTagWindow.ShowDialog();
            //Если всё ок
            if (result.GetValueOrDefault(false))
            {
                //Получаем результат добавления тега
                KeyValuePair<string, string> tagPair = editTagWindow.GetTagInfo();
                //Если пара тегов корректна                
                if (CheckTagPair(tagPair))
                {
                    //Добавляем новый тег в список
                    TagList.Add(tagPair.Value);
                    //Сохраняем теги
                    _tagsLoader.SaveTags(TagList);
                    //Выводим сообщение
                    MessageBox.Show($"Тег '{tagPair.Value}' был успешно добавлен!");
                }
            }
        }

        /// <summary>
        /// Выполняем редактирование тега
        /// </summary>
        /// <param name="images">Список изображений для обновления тега</param>
        /// <param name="tag">Текст тега для редактирвоания</param>
        public void EditTag(string tag, List<TaggedImage> images)
        {
            //Инициализируем окно добавления тега
            EditTagWindow editTagWindow = new EditTagWindow();
            //Проставляем в окно тег для редактирования
            editTagWindow.SetTag(tag);
            //Получаем результат добавления тега
            bool? result = editTagWindow.ShowDialog();
            //Если всё ок
            if (result.GetValueOrDefault(false))
            {
                //Получаем результат добавления тега
                KeyValuePair<string, string> tagPair = editTagWindow.GetTagInfo();
                //Если пара тегов корректна                
                if (CheckTagPair(tagPair))
                {
                    //Заменяем тег в основном списке
                    ReplaceTag(TagList, tagPair);
                    //Сохраняем теги
                    _tagsLoader.SaveTags(TagList);
                    //ЗАменяем теги в файлах
                    ReplaceTags(images, tagPair, false);
                    //Выводим сообщение
                    MessageBox.Show($"Тег '{tagPair.Key}' был успешно изменён на '{tagPair.Value}'!");
                }
            }
        }

        /// <summary>
        /// ЗАменяем теги в изображениях
        /// </summary>
        /// <param name="images">Список изображений для обновления тега</param>
        /// <param name="tagPair">Пара из заменяемого и заменяющего тегов</param>
        /// <param name="isNeedMessage">Флаг вывода сообщения о результате</param>
        public void ReplaceTags(List<TaggedImage> images, KeyValuePair<string, string> tagPair, bool isNeedMessage = true)
        {
            //Проходимся по картинкам, и заменяем им теги
            images.ForEach(image => ReplaceTag(image.Tags, tagPair));
            //Если нужно вывести сообщение
            if(isNeedMessage)
                //Выводим сообщение
                MessageBox.Show($"Тег '{tagPair.Key}' был успешно заменён на '{tagPair.Value}'!");
        }

        /// <summary>
        /// Удаляем тег из списка
        /// </summary>
        /// <param name="images">Список изображений для удаления тега</param>
        /// <param name="tag">Тег для удаления</param>
        public void DeleteTagFromCollection(List<TaggedImage> images, string tag)
        {
            //Удаляем тег из списка
            TagList.Remove(tag);
            //Сохраняем теги
            _tagsLoader.SaveTags(TagList);
            //Удаляем тег у изображений
            DeleteTagFromImage(images, tag);
            //Выводим сообщение
            MessageBox.Show($"Тег '{tag}' был успешно удалён!");
        }


        /// <summary>
        /// Удаляем тег из изображения
        /// </summary>
        /// <param name="image">Изображение для удаления тега</param>
        /// <param name="tag">Тег для удаления</param>
        public void DeleteTagFromImage(TaggedImage image, string tag) =>
            image.Tags.Remove(tag);

        /// <summary>
        /// Удаляем тег из изображений
        /// </summary>
        /// <param name="images">Список изображений для удаления тега</param>
        /// <param name="tag">Тег для удаления</param>
        public void DeleteTagFromImages(List<TaggedImage> images, string tag) =>
            //Проходимся по картинкам, и удаляем им этот тег
            images.ForEach(image => image.Tags.Remove(tag));

        /// <summary>
        /// Добавляем тег изображению
        /// </summary>
        /// <param name="image">Изображение для добавления тега</param>
        /// <param name="tag">Тег для добавления</param>
        public void AddTagToImage(TaggedImage image, string tag) =>
            image.Tags.Add(tag);

        /// <summary>
        /// Добавляем тег изображениям
        /// </summary>
        /// <param name="images">Изображения для добавления тега</param>
        /// <param name="tag">Тег для добавления</param>
        public void AddTagToImages(List<TaggedImage> images, string tag) =>
            //Проходимся по картинкам, и добавляем им этот тег
            images.ForEach(image => image.Tags.Add(tag));



    }
}
