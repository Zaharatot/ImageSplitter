﻿using ImageSplitter.Content.Clases.DataClases;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ImageDuplicatesControl.xaml
    /// </summary>
    public partial class ImageDuplicatesControl : UserControl
    {
        /// <summary>
        /// Событие запуска скнирования дубликатов
        /// </summary>
        public event StartDuplicateScanEventHandler StartDuplicateScan;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public ImageDuplicatesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку запуска сканирования
        /// </summary>
        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            //Если путь сканирования корректен
            if(CheckScanPath(ScanPathTextBox.Text))
                //ВЫзываем внешний ивент
                StartDuplicateScan?.Invoke(ScanPathTextBox.Text);
        }

        /// <summary>
        /// Обработчик события выбора контролла
        /// </summary>
        /// <param name="control">Выбранный контролл</param>
        private void ImagesPanel_UpdateFindedImageControlSelection(FindedImageControl control)
        {
            //Убираем выделение со всех дочерних контроллов
            ClearControlSelection();
            //Выделяем целевой контролл
            control.SetSelectionState(true);
            //Проставляем изображение с выделенного контролла в большую картинку
            TargetImage.Source = control.GetImage();
        }

        /// <summary>
        /// Убираем выделение со всех дочерних контроллов
        /// </summary>
        private void ClearControlSelection()
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImagesPanel imagesPanel in MainPanel.Children)
                //Сбрасываем выделение для их дочерних изображений
                imagesPanel.UnselectAllChilds();
        }

        /// <summary>
        /// Создаём контролл панели дубликатов
        /// </summary>
        /// <param name="images">Общий список изображений</param>
        /// <param name="target">Изображение-оригинал</param>
        /// <returns>Созданный контролл</returns>
        private FindedImagesPanel CreateDuplicatesPanel(List<DuplicateImageInfo> images, DuplicateImageInfo target)
        {
            //Создаём целевой контролл
            FindedImagesPanel imagesPanel = new FindedImagesPanel();
            //Добавляем обработчик события выделения контролла
            imagesPanel.UpdateFindedImageControlSelection += ImagesPanel_UpdateFindedImageControlSelection;
            //Проставляем контент в панель
            imagesPanel.SetImagesToControl(images, target);
            //Возвращаем созданный контролл
            return imagesPanel;
        }

        /// <summary>
        /// Удаляем старые панели дубликатов с панели
        /// </summary>
        private void ClearOldPanels()
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImagesPanel imagesPanel in MainPanel.Children)
                //Удаляем обработчик события выделения контролла
                imagesPanel.UpdateFindedImageControlSelection += ImagesPanel_UpdateFindedImageControlSelection;
            //Очищаем панель от старых контроллов
            MainPanel.Children.Clear();
        }

        /// <summary>
        /// Отображаем ошибку пути
        /// </summary>
        /// <param name="resultFlag">Флаг результата для простановки ошибки</param>
        /// <param name="errorMessage">Текст сообщения об ошибке</param>
        private void ShowPathError(ref bool resultFlag, string errorMessage)
        {
            //Проставляем флаг в ошибку
            resultFlag = false;
            //Выводим сообщение обю ошибке
            MessageBox.Show(errorMessage, "Ошибка!");
        }

        /// <summary>
        /// Проверка пути сканирования
        /// </summary>
        /// <param name="path">Путь сканирования</param>
        /// <returns>True - путь сканирования корректен</returns>
        private bool CheckScanPath(string path)
        {
            bool ex = true;
            //Если путь пустой
            if (string.IsNullOrEmpty(path))
                //Выводим ошибку пустого пути
                ShowPathError(ref ex, "Нужно указать путь для сканирования!");
            //Если папки не существует
            if (!Directory.Exists(path))
                //Выводим ошибку некорректного пути
                ShowPathError(ref ex, "Введённый путь некорректен!");
            //Возвращаем результат
            return ex;
        }


        /// <summary>
        /// Проставляем текущее значение прогресса сканирования
        /// </summary>
        /// <param name="current">Текущее значение</param>
        /// <param name="max">Максимальное значение</param>
        public void SetScanProgress(int current, int max)
        {
            ScanProgressBar.Value = current;
            ScanProgressBar.Maximum = max;
        }

        /// <summary>
        /// Проставляем изображения в контролл
        /// </summary>
        /// <param name="images">Список изображений-дубликатов</param>
        public void SetImages(List<DuplicateImageInfo> images)
        {
            //Проставляем бесконечный скролл
            ScanProgressBar.IsIndeterminate = true;
           //Удаляем старые панели дубликатов с панели
           ClearOldPanels();
            //Проходимся по списку дубликатов
            foreach (var image in images)
            {
                //Если дубликаты у данного изображения есть
                if(image.Duplicates.Count > 0)
                    //Создаём и добавляем на панель контролл панели дубликатов
                    MainPanel.Children.Add(CreateDuplicatesPanel(images, image));
            }
            //Ставим сразу максимальный прогресс
            SetScanProgress(1, 1);
            //Сбрвасываем бесконечный скролл
            ScanProgressBar.IsIndeterminate = false;
        }
    }
}
