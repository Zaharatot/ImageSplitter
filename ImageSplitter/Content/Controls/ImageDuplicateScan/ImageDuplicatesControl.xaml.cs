using DuplicateScanner.Clases.DataClases.File;
using DuplicateScanner.Clases.DataClases.Result;
using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Progress;
using ImageSplitter.Content.Clases.WorkClases.Resources;
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
using static DuplicateScanner.Clases.DataClases.Global.Enums;
using static ImageSplitter.Content.Clases.DataClases.Global.Delegates;

namespace ImageSplitter.Content.Controls.ImageDuplicateScan
{
    /// <summary>
    /// Логика взаимодействия для ImageDuplicatesControl.xaml
    /// </summary>
    public partial class ImageDuplicatesControl : UserControl
    {
        /// <summary>
        /// Событие запуска удаления дубликатов
        /// </summary>
        public event DuplicateRemoveEventHandler DuplicateRemove;




        /// <summary>
        /// Класс информации о прогрессе сканирования для панели
        /// </summary>
        private ProgressPanelInfo _scanProgressInfo;
        /// <summary>
        /// Класс информации о прогрессе удаления для панели
        /// </summary>
        private ProgressPanelInfo _removeProgressInfo;
        /// <summary>
        /// Класс форматирования времени
        /// </summary>
        private DateFormatter _dateFormatter;
        /// <summary>
        /// Текст сообыения для рассчёта времени
        /// </summary>
        private string _calculatiogTimeMessage;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public ImageDuplicatesControl()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем информацию о прогрессе
            _scanProgressInfo = CreateScanProgressInfo();
            _removeProgressInfo = CreateRemoveProgressInfo();
            //Грузим текст из ресурсов
            _calculatiogTimeMessage = ResourceLoader.LoadString("Text_DateLimitCalculation"); 
            //Инициализируем используемые классы
            _dateFormatter = new DateFormatter();
            //Добавляем обработчик события изменения текста поиска
            SearchStringTextBox.ChangeText += SearchStringTextBox_ChangeText;
        }


        /// <summary>
        /// Метод генерации информации о прогрессе удаления
        /// </summary>
        /// <returns>Класс информации о прогрессе</returns>
        private ProgressPanelInfo CreateRemoveProgressInfo() =>
            new ProgressPanelInfo() {
                IsViewProgress = true,
                HeaderText = ResourceLoader.LoadString("text_ProgressPanel_Remove"),
            };

        /// <summary>
        /// Метод генерации информации о прогрессе сканирования
        /// </summary>
        /// <returns>Класс информации о прогрессе</returns>
        private ProgressPanelInfo CreateScanProgressInfo() =>
            new ProgressPanelInfo() { 
                IsViewProgress = false,
                AddInfo = new List<ProgressAddInfo>() { 
                    new ProgressAddInfo(ResourceLoader.LoadString("text_DuplicateScanAddInfo_FilesFinded")),
                    new ProgressAddInfo(ResourceLoader.LoadString("text_DuplicateScanAddInfo_LoadedFiles")),
                    new ProgressAddInfo(ResourceLoader.LoadString("text_DuplicateScanAddInfo_FilesToProcess")),
                    new ProgressAddInfo(ResourceLoader.LoadString("text_DuplicateScanAddInfo_ErrorFilesCount")),
                    new ProgressAddInfo(ResourceLoader.LoadString("text_DuplicateScanAddInfo_ProcessedFiles")),
                    new ProgressAddInfo(ResourceLoader.LoadString("text_DuplicateScanAddInfo_ToCompleteTime")),                    
                }
            };




        /// <summary>
        /// Обработчик события изменения текста поиска
        /// </summary>
        /// <param name="text">Новый текст поиска</param>
        private void SearchStringTextBox_ChangeText(string text) =>
            //Вызываем метод обновления поиска
            IsChildElementSearchSucc(SearchStringTextBox.Text);
        
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
                //Если запрещено сохранение не выбранных
                if (!ScanProperties.IsSaveUnchecked)
                    //Просто очищаем их список
                    groups.Clear();
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
            //Закрываем поток в памяти, связанный с изображением
            CloseImageSource();
            //Грузим картинку в контролл
            TargetImage.Source = LoadImageByPath(control.DuplicateImagePath);
        }

        /// <summary>
        /// Обработчик события выбора контролла для удаления
        /// </summary>
        /// <param name="hash">Хеш элемента для простановки</param>
        /// <param name="parentName">Имя родительского элемента, откуда пришёл запрос блокировки</param>
        /// <param name="state">Статус для простановки</param>
        private void ImagesPanel_SetCheckToDuplicate(uint hash, string parentName, bool state) =>
            //Обновляем статусы блокировки дочерних элементов
            SetParentCheckBoxState(hash, parentName, state);

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
        /// Получаем название стадии по типу
        /// </summary>
        /// <param name="stage">Стадия прогресса</param>
        /// <returns>Текст стадии</returns>
        private string GetStageText(ScanStages stage) =>
            //Грузим описание стадии из ресурсов
            ResourceLoader.LoadString($"text_DuplicateScanStatus_{(int)stage}");


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
            List<uint> selectedHashes, notSelectedHashes;
            //Проходимся по всем контроллам панели
            foreach (FindedImagesPanel imagesPanel in MainPanel.Children)
            {
                //Получчаем из панели все выбранные и не выбранные хеши
                imagesPanel.GetHashesFromChilds(out selectedHashes, out notSelectedHashes);
                //Добавляем полученные значения в выходные классы
                toRemove.HashList.AddRange(selectedHashes);
                //Добавляем группу с этими хешами
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
        /// Метод проверки поиска для дочерних элементов
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        private void IsChildElementSearchSucc(string searchString)
        {
            //Проходимся по всем контроллам панели
            foreach (FindedImagesPanel imagesPanel in MainPanel.Children)
                //Если дочерняя панель соответствует критериям поиска
                imagesPanel.Visibility = (imagesPanel.IsSearchSucc(searchString)) 
                    //Отображаем её, а в противном случае - скрываем
                    ? Visibility.Visible : Visibility.Collapsed;
        }


        /// <summary>
        /// Метод простановки статуса для чекбокса, по значению из другой группы
        /// </summary>
        /// <param name="hash">Хеш элемента для простановки</param>
        /// <param name="parentName">Имя родительского элемента, откуда пришёл запрос блокировки</param>
        /// <param name="state">Статус для простановки</param>
        private void SetParentCheckBoxState(uint hash, string parentName, bool state)
        {
            //Список названий групп с последним заблокированным элементом
            StringBuilder sb = new StringBuilder();
            //Флаг группы с заблокированным последним элементом
            bool isLastBlocked;
            //Проходимся по всем контроллам панели
            foreach (FindedImagesPanel imagesPanel in MainPanel.Children)
            {
                //Передавая им значения для блокировки, и получаем флаг последнего заблокированного элемента
                isLastBlocked = imagesPanel.SetParentCheckBoxState(hash, parentName, state);
                //Если был заблокирован последний элемент
                if (isLastBlocked)
                    //Добавляем заголовок панели в список
                    sb.Append($"{imagesPanel.ElementHeader}, ");
            }
            //Если был заблокирован хоть один элемент
            if (sb.Length > 0)
            {
                //Берём строку без двух последних симводов
                string groups = sb.ToString().Substring(0, sb.Length - 2);
                //Выводим сообщение с предупреждением
                MessageBox.Show($"Данное действие выбрало для удаления последний не выбранный элемент в группах: {groups}");
            }
        }

        /// <summary>
        /// Создаём контролл панели дубликатов
        /// </summary>
        /// <param name="result">Класс результата поиска дубликатов</param>
        /// <param name="id">Идентификатор элемента</param>
        /// <returns>Созданный контролл</returns>
        private FindedImagesPanel CreateDuplicatesPanel(DuplicatePair result, int id)
        {
            //Создаём целевой контролл
            FindedImagesPanel imagesPanel = new FindedImagesPanel();
            //Добавляем обработчик события выделения контролла
            imagesPanel.UpdateFindedImageControlSelection += ImagesPanel_UpdateFindedImageControlSelection;
            //Добавляем обработчик события запроса на скрытие остальных панелей
            imagesPanel.HidePanelRequest += ImagesPanel_HidePanelRequest;
            //Добавляем обработчик события выбора контролла для удаления
            imagesPanel.SetCheckToDuplicate += ImagesPanel_SetCheckToDuplicate;
            //Проставляем контент в панель
            imagesPanel.SetImagesToControl(result, id);
            //Возвращаем созданный контролл
            return imagesPanel;
        }

        /// <summary>
        /// Закрываем поток в памяти, связанный с изображением
        /// </summary>
        private void CloseImageSource()
        {
            //Если есть исходный поток в памяти
            if (TargetImage.Source != null)
            {
                //Проучаем изображение
                BitmapImage source = (BitmapImage)TargetImage.Source;
                //Очищаем поток
                source.StreamSource.Dispose();
                //Закрываем поток
                source.StreamSource.Close();
                //Сбрасываем источник
                TargetImage.Source = null;
            }
        }

        /// <summary>
        /// Загружаем картинку по строке пути
        /// </summary>
        /// <param name="path">Путь к файлу картинки на диске</param>
        /// <returns>Класс картинки</returns>
        private BitmapImage LoadImageByPath(string path)
        {
            BitmapImage ex = new BitmapImage();
            ex.BeginInit();
            //Считываем байты файла в поток в памяти
            ex.StreamSource = new MemoryStream(File.ReadAllBytes(path));
            ex.EndInit();
            return ex;
        }

        /// <summary>
        /// Метод рассчёта оставшегося времени
        /// </summary>
        /// <param name="info">Информация о прогрессе сканирования</param>
        /// <returns>Строка с инфой о времени</returns>
        private string CalculateToCompleteTime(ScanProgressInfo info)
        {
            //Если время уже есть
            if (info.TimeLeft.HasValue)
                //ВОзвращаем время в виде строки
                return _dateFormatter.GetFormatDateLimit(info.TimeLeft.Value);
            //Если время пока не рассчитано
            else
                //Возвращаем соответствующее сообщение
                return _calculatiogTimeMessage;
        }






        /// <summary>
        /// Удаляем старые панели дубликатов с панели
        /// </summary>
        public void ClearOldPanels()
        {
            //Убираем выбранное изображение
            CloseImageSource();
            //Проходимся по всем контроллам панели
            foreach (FindedImagesPanel imagesPanel in MainPanel.Children)
            {
                //Удаляем обработчик события выделения контролла
                imagesPanel.UpdateFindedImageControlSelection -= ImagesPanel_UpdateFindedImageControlSelection;
                //Удаляем обработчик события запроса на скрытие остальных панелей
                imagesPanel.HidePanelRequest -= ImagesPanel_HidePanelRequest;
                //Удаляем обработчик события выбора контролла для удаления
                imagesPanel.SetCheckToDuplicate -= ImagesPanel_SetCheckToDuplicate;
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
        public void UpdateRemoveInfo(ProgressInfo info)
        {
            //Обновляем информацию в классе информации о прогрессе
            _removeProgressInfo.Maximum = info.MaxCount;
            _removeProgressInfo.Current = info.Processed;
            //Передаём значение в контролл
            MainProgressControl.UpdateProgress(_removeProgressInfo);
        }

        /// <summary>
        /// Выполняем обновление информации о прогрессе сканирования
        /// </summary>
        /// <param name="info">Информация о прогрессе сканирования</param>
        public void UpdateScanInfo(ScanProgressInfo info)
        {
            //Проставляем в поля доп. инфы значения
            _scanProgressInfo.AddInfo[0].Value = info.FilesFinded.ToString();
            _scanProgressInfo.AddInfo[1].Value = info.LoadedFiles.ToString();
            _scanProgressInfo.AddInfo[2].Value = info.FilesToProcess.ToString();
            _scanProgressInfo.AddInfo[3].Value = info.ErrorFilesCount.ToString();
            _scanProgressInfo.AddInfo[4].Value = info.ProcessedFiles.ToString();
            //РАссчитываем время до завершения работы
            _scanProgressInfo.AddInfo[5].Value = CalculateToCompleteTime(info);
            //Проставляем значения для прогрессбара
            _scanProgressInfo.Maximum = info.FilesToProcess;
            _scanProgressInfo.Current = info.ProcessedFiles;
            //Проставляем заголовок
            _scanProgressInfo.HeaderText = GetStageText(info.Stage);
            //Доп. инфу отображаем только на стадии генерации хешей
            _scanProgressInfo.IsViewAddInfo = (info.Stage == ScanStages.HashGeneration);
            //Отображаем значения прогресса только на стадии поиска дубликатов
            _scanProgressInfo.IsViewProgress = (info.Stage == ScanStages.DuplicateFind);
            //Бесконечный прогресс отображаем только на определённых стадиях
            _scanProgressInfo.IsIndeterminate = info.IsIndeterminate;
            //Передаём значение в контролл
            MainProgressControl.UpdateProgress(_scanProgressInfo);
        }


        /// <summary>
        /// Проставляем изображения в контролл
        /// </summary>
        /// <param name="results">Класс результатов поиска дубликатов</param>
        public void SetImages(List<DuplicatePair> results)
        {
            //Удаляем старые панели дубликатов с панели
            ClearOldPanels();
            //Проходимся по списку дубликатов
            for (int i = 0; i < results.Count; i++)
                //Создаём и добавляем на панель контролл панели дубликатов
                MainPanel.Children.Add(CreateDuplicatesPanel(results[i], i + 1));
        }
    }
}
