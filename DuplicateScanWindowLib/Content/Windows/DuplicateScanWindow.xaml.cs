using DuplicateScannerLib.Clases.DataClases.File;
using DuplicateScannerLib.Clases.DataClases.Result;
using DuplicateScanWindowLib.Content.Controls;
using SplitterDataLib.DataClases.Global.DuplicateScan;
using SplitterResources;
using SplitterSimpleUI.Content.Clases.DataClases.HotKey;
using SplitterSimpleUI.Content.Clases.DataClases.Progress;
using SplitterSimpleUI.Content.Clases.WorkClases.Controls;
using SplitterSimpleUI.Content.Clases.WorkClases.Data;
using SplitterSimpleUI.Content.Clases.WorkClases.HotKey;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using static DuplicateScannerLib.Clases.DataClases.Global.Enums;
using static SplitterDataLib.DataClases.Global.Delegates;

namespace DuplicateScanWindowLib.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для DuplicateScanWindow.xaml
    /// </summary>
    public partial class DuplicateScanWindow : Window
    {
        /// <summary>
        /// Событие запуска скнирования на дубликаты
        /// </summary>
        public event StartDuplicateScanEventHandler StartDuplicateScan;
        /// <summary>
        /// Событие запроса удаления старых записей
        /// </summary>
        public event EmptyEventHandler RemoveOldRequest;
        /// <summary>
        /// Событие запуска удаления дубликатов
        /// </summary>
        public event DuplicateRemoveEventHandler DuplicateRemove;

        /// <summary>
        /// Флаг разрешения закрытия окна
        /// </summary>
        public bool IsAllowClose { get; set; }



        /// <summary>
        /// Класс обработки хоткеев
        /// </summary>
        private HotKeyProcessor _hotKeyProcessor;


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
        /// Класс загрузки изображения
        /// </summary>
        private ImageSourceLoader _imageSourceLoader;
        /// <summary>
        /// Текст сообыения для рассчёта времени
        /// </summary>
        private string _calculatiogTimeMessage;

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public DuplicateScanWindow()
        {
            InitializeComponent();
            Init();
        }

        #region Initialization

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        private void Init()
        {
            //Инициалдизируем значения переменных
            InitVariables();
            //Инициализируем обработчики событий
            InitEvents();
            //Инициализируем хоткеи
            InitHotkeys();
        }

        /// <summary>
        /// Инициалдизируем значения переменных
        /// </summary>
        private void InitVariables()
        {
            //Инициализируем флаг разрешения закрытия окна
            IsAllowClose = false;
            //Инициализируем информацию о прогрессе
            _scanProgressInfo = CreateScanProgressInfo();
            _removeProgressInfo = CreateRemoveProgressInfo();
            //Грузим текст из ресурсов
            _calculatiogTimeMessage = ResourceLoader.LoadString("Text_DateLimitCalculation");
            //Инициализируем используемые классы
            _dateFormatter = DateFormatter.GetInstance();
            //Инициализируем класс загрузки изображения
            _imageSourceLoader = new ImageSourceLoader();
            //Добавляем обработчик события изменения текста поиска
            SearchStringTextBox.ChangeText += SearchStringTextBox_ChangeText;
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void InitEvents()
        {
            //Добавляем обработчик события запроса удаления старых записей
            ScanProperties.RemoveOldRequest += ScanProperties_RemoveOldRequest;
            //Добавляем обработчик события запуска скнирования на дубликаты
            ScanProperties.StartDuplicateScan += ScanProperties_StartDuplicateScan;
        }


        #endregion

        #region InitProgressPanel

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

        #endregion

        #region InitHotKeys

        /// <summary>
        /// Инициализируем хоткеи
        /// </summary>
        private void InitHotkeys()
        {
            //Получаем экземпляр класса обработки хоткеев
            _hotKeyProcessor = HotKeyProcessor.GetInstance();
            //Добавляем хоткеи для текущего окна
            AddHotKeys();
        }

        /// <summary>
        /// Метод добавления хоткеев для окна
        /// </summary>
        private void AddHotKeys() =>
            //При получении хоткея, дальше нажатие не пройдёт
            _hotKeyProcessor.AddWindow(this, new WindowHotKeys(true) {
                //Добавляем список хоткеев
                HotKeys = new List<HotKeyInfo>() { 
                    ////При нажатии на "Ctrl + R" - выполняем обновление древа 
                    //new HotKeyInfo(Key.R, () => { StartUpdateTree(); }, true),
                    ////При нажатии на "Escape" - вызываем закрытие окна
                    //new HotKeyInfo(Key.Escape, () => { this.Close(); }),
                }
            });

        #endregion


        #region Events



        /// <summary>
        /// Обработчик осбытия обработки закрытия окна
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //Если окно закрывать не разрешено
            if (!IsAllowClose)
            {
                //Скрываем данное окно
                this.Visibility = Visibility.Hidden;
                //Отменяем закрытие
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Обработчик осбытия закрытия окна
        /// </summary>
        private void Window_Closed(object sender, EventArgs e) =>
            //Удаляем обюработку хоткеев для данного окна
            _hotKeyProcessor.RemoveWindow(this);

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
                //Выключаем доступность окна
                this.IsEnabled = false;
                //Убираем выбранное изображение
                TargetImage.Source = null;
                //Получаем список хешей из всех контроллов выбора
                GetHashesFormSelectors(out HashesGroup toRemove, out List<HashesGroup> groups);
                //Если запрещено сохранение не выбранных
                if (!ScanProperties.IsSaveUnchecked)
                    //Просто очищаем их список
                    groups.Clear();
                //Отображаем панель прогресса
                SetProgressPanelVisiblity(true);
                //Очищаем панель дубликатов
                ClearOldPanels();
                //Вызываем ивент запроса дуаления, передавая в него списки хешей
                DuplicateRemove?.Invoke(toRemove, groups);
            }
        }

        /// <summary>
        /// Обработчик события выбора контролла
        /// </summary>
        /// <param name="control">Выбранный контролл</param>
        private async void ImagesPanel_UpdateFindedImageControlSelection(object control)
        {
            //Типизируем полученный контролл, и если всё ок
            if (control is FindedImageControl findedImageControl)
            {
                //Убираем выделение со всех дочерних контроллов
                ClearControlSelection();
                //Выделяем целевой контролл
                findedImageControl.SetSelectionState(true);
                //Грузим картинку в контролл
                await _imageSourceLoader.LoadImageByPath(
                    TargetImage, findedImageControl.DuplicateImagePath);
            }
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
        /// <param name="showedPanelHeader">Заголовок открывшейся панели</param>
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
        /// Обработчик события запуска скнирования на дубликаты
        /// </summary>
        /// <param name="properties">Параметры сканирования</param>
        private void ScanProperties_StartDuplicateScan(ScanProperties properties)
        {
            //Выключаем доступность окна
            this.IsEnabled = false;
            //Отображаем панель прогресса
            SetProgressPanelVisiblity(true);
            //Вызываем внешний ивент
            StartDuplicateScan?.Invoke(properties);
        }

        /// <summary>
        /// Обработчик события запроса удаления старых записей
        /// </summary>
        private void ScanProperties_RemoveOldRequest()
        {
            //Выключаем доступность окна
            this.IsEnabled = false;
            //Отображаем панель прогресса
            SetProgressPanelVisiblity(true);
            //Вызываем внешний ивент
            RemoveOldRequest?.Invoke();
        }

        #endregion


        #region InternalMethods

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
        private void ClearOldPanels()
        {
            //Убираем выбранное изображение
            _imageSourceLoader.CloseImageSource(TargetImage);
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
        private void SetProgressPanelVisiblity(bool isVisible) =>
            //Меняем видимость контролла прогресса
            MainProgressControl.Visibility =
                (isVisible) ? Visibility.Visible : Visibility.Collapsed;


        /// <summary>
        /// Проставляем изображения в контролл
        /// </summary>
        /// <param name="results">Класс результатов поиска дубликатов</param>
        private void SetImages(List<DuplicatePair> results)
        {
            //Удаляем старые панели дубликатов с панели
            ClearOldPanels();
            //Проходимся по списку дубликатов
            for (int i = 0; i < results.Count; i++)
                //Создаём и добавляем на панель контролл панели дубликатов
                MainPanel.Children.Add(CreateDuplicatesPanel(results[i], i + 1));
        }

        #endregion


        #region PublicMethods



        /// <summary>
        /// Обновляем информацию об удалении
        /// </summary>
        /// <param name="info">Информация об удалении</param>
        public void UpdateRemoveInfo(ProgressInfo info) =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Обновляем информацию в классе информации о прогрессе
                _removeProgressInfo.Maximum = info.MaxCount;
                _removeProgressInfo.Current = info.Processed;
                //Передаём значение в контролл
                MainProgressControl.UpdateProgress(_removeProgressInfo);
            });

        /// <summary>
        /// Выполняем обновление информации о прогрессе сканирования
        /// </summary>
        /// <param name="info">Информация о прогрессе сканирования</param>
        public void UpdateScanInfo(ScanProgressInfo info) =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
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
            });


        /// <summary>
        /// Метод завершения удаления
        /// </summary>
        public void CompleteRemove() =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Скрываем панель прогресса
                SetProgressPanelVisiblity(false);
                //Возвращаем доступность окна
                this.IsEnabled = true;
            });

        /// <summary>
        /// Метод завершения удаления старых дубликатов
        /// </summary>
        public void CompleteRemoveOldDuplicates() =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Скрываем панель прогресса
                SetProgressPanelVisiblity(false);
                //Возвращаем доступность окна
                this.IsEnabled = true;
            });


        /// <summary>
        /// Метод завершения сканирования
        /// </summary>
        /// <param name="result">Результат сканирования на дубликаты</param>
        public void CompleteScan(List<DuplicatePair> result) =>
            //Вызываем в UI-потоке
            this.Dispatcher.Invoke(() => {
                //Скрываем панель прогресса
                SetProgressPanelVisiblity(false);
                //Если дубликаты не были найдены
                if (result.Count == 0)
                    //Удаляем старые панели дубликатов, чтобы не было путанницы
                    ClearOldPanels();
                //Если результаты есть
                else
                    //Втыкаем результаты поиска в контролл
                    SetImages(result);
                //Возвращаем доступность окна
                this.IsEnabled = true;
            });

        #endregion

    }
}