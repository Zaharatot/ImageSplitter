using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Duplicates;
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

namespace ImageSplitter.Content.Controls.ImageDuplicateScan
{
    /// <summary>
    /// Логика взаимодействия для FindedImagesPanel.xaml
    /// </summary>
    public partial class FindedImagesPanel : UserControl
    {

        /// <summary>
        /// Событие обновления выделения для контроллов найденных изображений
        /// </summary>
        public event UpdateFindedImageControlSelectionEventHandler UpdateFindedImageControlSelection;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public FindedImagesPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события выбора контролла
        /// </summary>
        /// <param name="control">Выбранный контролл</param>
        private void ImageControl_UpdateFindedImageControlSelection(FindedImageControl control) =>
            //Вызываем внешний ивент
            UpdateFindedImageControlSelection?.Invoke(control);

        /// <summary>
        /// Обработчик события нажатия на кнопку сброса всхе выделений
        /// </summary>
        private void CleanAllCheckBoxesButton_Click(object sender, RoutedEventArgs e) =>
            //Сбрасываем все галочки на чекбоксах
            SetAllCheckBoxesState(false);

        /// <summary>
        /// Обработчик события нажатия на кнопку простановки всхе выделений
        /// </summary>
        private void SetAllCheckBoxesButton_Click(object sender, RoutedEventArgs e) =>
            //Проставляем все галочки на чекбоксах
            SetAllCheckBoxesState(true);


        /// <summary>
        /// Создаём контролл целевого изображения
        /// </summary>
        /// <param name="info">Информация о изображении</param>
        /// <returns>Контроллл с изображением</returns>
        private FindedImageControl CreateControl(DuplicateImageInfo info)
        {
            //Создаём новый контролл
            FindedImageControl imageControl = new FindedImageControl();
            //Добавляем обработчик события выделения контролла
            imageControl.UpdateFindedImageControlSelection += ImageControl_UpdateFindedImageControlSelection;
            //ПРоставляем в контролл информацию об иконке
            imageControl.SetControlInfo(info);
            //Возвращаем созданный контролл
            return imageControl;
        }

        /// <summary>
        /// Метод простановки нового статуса всем дочерним чекбоксам
        /// </summary>
        /// <param name="state">Новый статус</param>
        private void SetAllCheckBoxesState(bool state)
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
                //Для каждого из них сбрасываем галочку
                imageControl.SetCheckBoxState(state);
        }


        /// <summary>
        /// Проставляем изображения-дубликаты в контролл
        /// </summary>
        /// <param name="images">Общий список изображений</param>
        /// <param name="target">Изображение-оригинал</param>
        public void SetImagesToControl(List<DuplicateImageInfo> images, DuplicateImageInfo target)
        {
            //Удаляем старые изображения с панели
            ClearOldImages();
            //Создаём и добавляем на панель контролл оригинала
            MainPanel.Children.Add(CreateControl(target));
            //Проходимся по списку дубликатов
            foreach (var duplicate in target.Duplicates)
                //Создаём и добавляем на панель контролл изображения
                MainPanel.Children.Add(CreateControl(duplicate));
        }

        /// <summary>
        /// Сбрасыфваем выделение для всех дочерних контроллов
        /// </summary>
        public void UnselectAllChilds()
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
                //Для каждого из них сбрасываем выделение
                imageControl.SetSelectionState(false);
        }

        /// <summary>
        /// Удаляем старые изображения с панели
        /// </summary>
        public void ClearOldImages()
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
            {
                //Удаляем обработчик события выделения контролла
                imageControl.UpdateFindedImageControlSelection -= ImageControl_UpdateFindedImageControlSelection;
                //Закрываем поток работы с изображением
                imageControl.CloseImageSource();
            }
            //Очищаем панель от старых контроллов
            MainPanel.Children.Clear();
        }

        /// <summary>
        /// Получаем информацию об изображениях из дочерних контроллов
        /// </summary>
        /// <returns>Список дубликатов из дочерних контроллов, отмеченных галочками</returns>
        public List<DuplicateImageInfo> GetSelectedDulicateInfoFromChilds()
        {
            List<DuplicateImageInfo> ex = new List<DuplicateImageInfo>();
            DuplicateImageInfo buff;
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
            {
                //ПОлучаем инфу из контролла
                buff = imageControl.GetControlInfo();
                //Если это изображение нужно удалить
                if (buff.IsNeedRemove)
                    //Добавляем его в список возврата
                    ex.Add(buff);
            }
            //Возвращаем результат
            return ex;
        }

    }
}
