using DuplicateScanner.Clases.DataClases.File;
using DuplicateScanner.Clases.DataClases.Result;
using ImageSplitter.Content.Clases.DataClases;
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
using static ImageSplitter.Content.Clases.DataClases.Global.Delegates;

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
        /// Событие запуска удаления дубликатов
        /// </summary>
        public event DuplicateRemoveEventHandler DuplicateRemove;

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
        /// Обработчик события нажатия на кнопку удаления выбранных дубликатов
        /// </summary>
        private void RemoveDuplicatesButton_Click(object sender, RoutedEventArgs e)
        {
            //Если нужно действительно удалить файлы
            if (IsNeedRemoveFiles())
            {
                //Убираем выбранное изображение
                TargetImage.Source = null;
                //Получаем список хешей из всех контроллов выбора
                GetHashesFormSelectors(out HashesGroup toRemove, out List<HashesGroup> groups);
                //Вызываем ивент запроса дуаления, передавая в него списки хешей
                DuplicateRemove?.Invoke(toRemove, groups);
            }
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
        /// Метод выполнения запроса пользователю на 
        /// подтвержддение удаления выбранных файлов
        /// </summary>
        /// <returns>True - файлы действительно нужно удалить</returns>
        private bool IsNeedRemoveFiles() =>
            //Выводим месседжбокс с запросом
            (MessageBox.Show(
                "Вы действительно хотите удалить все отмеченные галочками файлы?",
                "Запрос подтверждения", MessageBoxButton.YesNo
            //И сверяем результат с подтверждением
            ) == MessageBoxResult.Yes);

        /// <summary>
        /// Получаем список хешей из всех контроллов выбора
        /// </summary>
        /// <param name="groups">Список запрещённых групп</param>
        /// <param name="toRemove">Группа хешей для удаления</param>
        private void GetHashesFormSelectors(out HashesGroup toRemove, out List<HashesGroup> groups)
        {
            //Инициализируем выходные классы
            toRemove = new HashesGroup();
            groups = new List<HashesGroup>();
            //Локальные массивы для получения значений
            List<int> selectedHashes, notSelectedHashes;
            //Проходимся по всем контроллам панели
            foreach (FindedImagesPanel imagesPanel in MainPanel.Children)
            {
                //Получчаем из панели все выбранные и не выбранные хеши
                imagesPanel.GetHashesFromChilds(out selectedHashes, out notSelectedHashes);
                //Добавляем полученные значения в выходные классы
                toRemove.HashList.AddRange(selectedHashes);
                groups.Add(new HashesGroup(notSelectedHashes));
            }
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
        /// <param name="result">Класс результата поиска дубликатов</param>
        /// <param name="id">Идентификатор элемента</param>
        /// <returns>Созданный контролл</returns>
        private FindedImagesPanel CreateDuplicatesPanel(FindResult result, int id)
        {
            //Создаём целевой контролл
            FindedImagesPanel imagesPanel = new FindedImagesPanel();
            //Добавляем обработчик события выделения контролла
            imagesPanel.UpdateFindedImageControlSelection += ImagesPanel_UpdateFindedImageControlSelection;
            //Добавляем обработчик события запроса на скрытие остальных панелей
            imagesPanel.HidePanelRequest += ImagesPanel_HidePanelRequest;
            //Проставляем контент в панель
            imagesPanel.SetImagesToControl(result, id);
            //Возвращаем созданный контролл
            return imagesPanel;
        }

        /// <summary>
        /// Jбработчик события запроса на скрытие остальных панелей
        /// </summary>
        /// <param name="showedPanelHeader">Pfujhkjdjr jnrhsditqcz gfytkb</param>
        private void ImagesPanel_HidePanelRequest(string showedPanelHeader)
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImagesPanel imagesPanel in MainPanel.Children)
            {
                //Если панель развёрнута и она не является только что открытой
                if (imagesPanel.IsExpanded && (imagesPanel.ElementHeader != showedPanelHeader))
                    //СВорачиваем её
                    imagesPanel.IsExpanded = false;
            }
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
        /// Удаляем старые панели дубликатов с панели
        /// </summary>
        public void ClearOldPanels()
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImagesPanel imagesPanel in MainPanel.Children)
            {
                //Удаляем обработчик события выделения контролла
                imagesPanel.UpdateFindedImageControlSelection -= ImagesPanel_UpdateFindedImageControlSelection;
                //Удаляем обработчик события запроса на скрытие остальных панелей
                imagesPanel.HidePanelRequest -= ImagesPanel_HidePanelRequest;
                //Удаляем всё со старой панели
                imagesPanel.ClearOldImages();
            }
            //Очищаем панель от старых контроллов
            MainPanel.Children.Clear();
        }

        /// <summary>
        /// Метод обновления видимости контролла прогресса
        /// </summary>
        /// <param name="isVisible">Новое значение видимости для контролла</param>
        public void SetProgressPanelVisiblity(bool isVisible) =>
            //Меняем видимость контролла прогресса
            MainProgressControl.Visibility = (isVisible) 
                ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Обновляем информацию об удалении
        /// </summary>
        /// <param name="info">Информация об удалении</param>
        public void UpdateRemoveInfo(ProgressInfo info) =>
            //Передаём значение в контролл
            MainProgressControl.UpdateRemoveInfo(info);


        /// <summary>
        /// Выполняем обновление информации о прогрессе сканирования
        /// </summary>
        /// <param name="info">Информация о прогрессе сканирования</param>
        public void UpdateScanInfo(ScanProgressInfo info) =>
            //Передаём значение в контролл
            MainProgressControl.UpdateScanInfo(info);


        /// <summary>
        /// Проставляем изображения в контролл
        /// </summary>
        /// <param name="results">Класс результатов поиска дубликатов</param>
        public void SetImages(List<FindResult> results)
        {
            //Отображем контролл прогресса
            MainProgressControl.Visibility = Visibility.Visible;
            //Удаляем старые панели дубликатов с панели
            ClearOldPanels();
            //Проходимся по списку дубликатов
            for (int i = 0; i < results.Count; i++)
            {
                //Отображаем прогресс визуализации
                MainProgressControl.UpdateVisualizeStage(i + 1, results.Count);
                //Создаём и добавляем на панель контролл панели дубликатов
                MainPanel.Children.Add(CreateDuplicatesPanel(results[i], i + 1));
            }
            //Отображаем прогресс визуализации
            MainProgressControl.UpdateVisualizeStage(results.Count, results.Count);
            //Скрываем контролл прогресса
            MainProgressControl.Visibility = Visibility.Collapsed;
        }

    }
}
