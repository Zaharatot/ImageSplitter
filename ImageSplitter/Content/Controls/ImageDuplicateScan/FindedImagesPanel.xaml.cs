using ImageSplitter.Content.Clases.DataClases;
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
using static ImageSplitter.Content.Clases.DataClases.Delegates;

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
        /// Удаляем старые изображения с панели
        /// </summary>
        private void ClearOldImages()
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImageControl imageControl in MainPanel.Children)
                //Удаляем обработчик события выделения контролла
                imageControl.UpdateFindedImageControlSelection -= ImageControl_UpdateFindedImageControlSelection;
            //Очищаем панель от старых контроллов
            MainPanel.Children.Clear();
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

    }
}
