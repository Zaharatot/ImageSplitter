using DuplicateScanner.Clases.DataClases.Properties;
using ImageSplitter.Content.Clases.DataClases.Global;
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

namespace ImageSplitter.Content.Controls.ImageDuplicateScan
{
    /// <summary>
    /// Логика взаимодействия для ScanPropertiesControl.xaml
    /// </summary>
    public partial class ScanPropertiesControl : UserControl
    {
        /// <summary>
        /// Флаг сохранения не выбранных элементов, как запрещённых
        /// </summary>
        public bool IsSaveUnchecked => 
            SaveUncheckedCheckBox.IsChecked.GetValueOrDefault(false);


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ScanPropertiesControl()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Заполняем комбобокс
            FillComboBox();
        }

        /// <summary>
        /// Метод заполнения комбобокса
        /// </summary>
        private void FillComboBox()
        {
            //Очищаем список элементов
            ScanTypeComboBox.Items.Clear();
            //Тут индекс взял от последнего элемента enum-а
            for (int i = 0; i <= (int)ScanTypes.LinedDcpScan; i++)
                //Добавляем тип сканирования в список
                ScanTypeComboBox.Items.Add(GetScanTypeName(i));
            //Выбираем первый элемент в списке
            ScanTypeComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Кнопка запуска сканирования
        /// </summary>
        private void StartScanButton_Click(object sender, RoutedEventArgs e)
        {
            //Если путь сканирования корректен
            if (CheckScanPath(ScanPathTextBox.Text))
                //Вызываем глобальный ивент, передавая в него параметры с панели
                GlobalEvents.InvokeStartDuplicateScan(LoadScanProperties());
        }

        /// <summary>
        /// Обработчик события изменения значения слайдера
        /// </summary>
        private void AccuracySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) =>
            //Проставляем в текстовый блок значение из слайдера
            AccuracyTextBlock.Text = ((int)AccuracySlider.Value).ToString();


        /// <summary>
        /// Загружаем из ресурсов строку по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор типа enum-а</param>
        /// <returns>Строка описания типа</returns>
        private string GetScanTypeName(int id) =>
            ResourceLoader.LoadString($"Text_DuplicateScanType_{id}");

        /// <summary>
        /// Метод загрузки параметров сканирвоания с панели
        /// </summary>
        /// <returns>Параметры сканирования</returns>
        private ScanProperties LoadScanProperties() =>
            new ScanProperties() { 
                ScanPath = ScanPathTextBox.Text,
                ScanAccuracy = (int)AccuracySlider.Value,
                ScanType = GetScanType()
            };

        /// <summary>
        /// Метод получения выбранного типа сканирвоания
        /// </summary>
        /// <returns>ВЫбранный тип скнирования</returns>
        private ScanTypes GetScanType() =>
            (ScanTypes)ScanTypeComboBox.SelectedIndex;


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

    }
}
