using ImageSplitter.Content.Clases.DataClases.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.Tags
{
    /// <summary>
    /// Класс обновления тегов изображения
    /// </summary>
    internal class ImageTagUpdater
    {


        /// <summary>
        /// Класс работы с изображениями
        /// </summary>
        private ImagesWork _imagesWork;
        /// <summary>
        /// Класс работы с тегами
        /// </summary>
        private TagsWork _tagsWork;
        /// <summary>
        /// Список загруженных картинок
        /// </summary>
        private List<TaggedImage> _taggedImages;
        /// <summary>
        /// Id текущей страницы
        /// </summary>
        private int _currentPageId;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImageTagUpdater()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _imagesWork = new ImagesWork();
            _tagsWork = new TagsWork();
            //Инициализируем дефолтные значения
            _taggedImages = new List<TaggedImage>();
            _currentPageId = 0;
        }




        /// <summary>
        /// Получаем список тегированных изображений
        /// </summary>
        /// <returns>Список тегированных изображений</returns>
        public List<TaggedImage> GetImages() => 
            _taggedImages;

        /// <summary>
        /// Метод загрузки файлов из папки
        /// </summary>
        /// <param name="path">Путь к папке с файлами</param>
        public void LoadFiles(string path)
        {
            //Загружаем файлы изображений
            _taggedImages = _imagesWork.LoadFiles(path);
            //Обьновляем коллекцию тегов
            _tagsWork.UpdateTags(_taggedImages);
        }

        /// <summary>
        /// Выполняем сохранение тегов для изображений
        /// </summary>
        public void SaveChanges() =>
            //Вызываем внутренний метод
            _imagesWork.SaveTags(_taggedImages);

        /// <summary>
        /// Выполняем добавление тега
        /// </summary>
        public void AddTag() =>
            //Вызываем внутренний метод
            _tagsWork.AddTag();

        /// <summary>
        /// Выполняем редактирование тега
        /// </summary>
        /// <param name="tag">Текст тега для редактирвоания</param>
        public void EditTag(string tag) =>
            //Вызываем внутренний метод
            _tagsWork.EditTag(tag, _taggedImages);

        /// <summary>
        /// ЗАменяем теги в изображениях
        /// </summary>
        /// <param name="replacor">Тег для замены</param>
        /// <param name="source">Заменяемый тег</param>
        public void ReplaceTags(string source, string replacor) =>
            //Вызываем внутренний метод
            _tagsWork.ReplaceTags(_taggedImages, new KeyValuePair<string, string>(source, replacor));

        /// <summary>
        /// Удаляем тег из списка
        /// </summary>
        /// <param name="tag">Тег для удаления</param>
        public void DeleteTagFromCollection(string tag) =>
            //Вызываем внутренний метод
            _tagsWork.DeleteTagFromCollection(_taggedImages, tag);

        /// <summary>
        /// Удаляем тег из изображения
        /// </summary>
        /// <param name="image">Изображение для удаления тега</param>
        /// <param name="tag">Тег для удаления</param>
        public void DeleteTagFromImage(TaggedImage image, string tag) =>
            //Вызываем внутренний метод
            _tagsWork.DeleteTagFromImage(image, tag);

        /// <summary>
        /// Удаляем тег из изображений
        /// </summary>
        /// <param name="tag">Тег для удаления</param>
        public void DeleteTagFromImages(string tag) =>
            //Вызываем внутренний метод
            _tagsWork.DeleteTagFromImages(_taggedImages, tag);

        /// <summary>
        /// Добавляем тег изображению
        /// </summary>
        /// <param name="image">Изображение для добавления тега</param>
        /// <param name="tag">Тег для добавления</param>
        public void AddTagToImage(TaggedImage image, string tag) =>
            //Вызываем внутренний метод
            _tagsWork.AddTagToImage(image, tag);

        /// <summary>
        /// Добавляем тег изображениям
        /// </summary>
        /// <param name="tag">Тег для добавления</param>
        public void AddTagToImages(string tag) =>
            //Вызываем внутренний метод
            _tagsWork.DeleteTagFromImages(_taggedImages, tag);


        /// <summary>
        /// Переход к последней странице
        /// </summary>
        public void GoToLastPage() =>
            _currentPageId = _taggedImages.Count - 1;

        /// <summary>
        /// Переход к первой странице
        /// </summary>
        public void GoToFirstPage() =>
            _currentPageId = 0;

        /// <summary>
        /// Переход к странице
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        public void GoToPage(int direction) =>
            //ПОлучаем страницу, ограниченную нулём и максимумом
            //страниц, перемещённую в указанном направлении
            _currentPageId = Math.Max(0, 
                Math.Min(_taggedImages.Count, 
                    _currentPageId + direction));

        /// <summary>
        /// Возвращаем количество страниц
        /// </summary>
        /// <returns>Количество страниц</returns>
        public int GetCountPages() =>
            _taggedImages.Count;

        /// <summary>
        /// ВОзврат текущей страницы
        /// </summary>
        /// <returns>Текущая страница</returns>
        public TaggedImage GetCurrentPage() =>
            _taggedImages[_currentPageId];
    }
}