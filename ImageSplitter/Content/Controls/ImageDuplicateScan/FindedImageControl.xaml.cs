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
    /// Логика взаимодействия для FindedImageControl.xaml
    /// </summary>
    public partial class FindedImageControl : UserControl
    {

        /// <summary>
        /// Событие обновления выделения для контроллов найденных изображений
        /// </summary>
        public event UpdateFindedImageControlSelectionEventHandler UpdateFindedImageControlSelection;



        /// <summary>
        /// Хеш дубликата
        /// </summary>
        public int DuplicateHash => _result.PathHash;
        /// <summary>
        /// Флаг выбора данного контролла
        /// </summary>
        public bool IsSelected => GetCheckBoxState();


        /// <summary>
        /// Информация о результате поиска
        /// </summary>
        private DuplicateResult _result;

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public FindedImageControl()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        private void Init()
        {
            //Проставляем дефолтные значения
            _result = null;
        }


        /// <summary>
        /// Обработчик события нажатия на панель 
        /// </summary>
        private void MainPanel_MouseDown(object sender, MouseButtonEventArgs e) =>
            //Вызываем внешний ивент
            UpdateFindedImageControlSelection?.Invoke(this);

        /// <summary>
        /// Получаем значение статуса чекбокса
        /// </summary>
        /// <returns>True - чекбокс нажат</returns>
        private bool GetCheckBoxState() =>
            SelectImageCheckBox.IsChecked.GetValueOrDefault(false);

      

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
        /// Обновляем значение статуса выделения
        /// </summary>
        /// <param name="state">Новое значение статуса выделения</param>
        public void SetSelectionState(bool state) =>
            //Проставляем цвет контролла
            MainPanel.Background = (state) ? Brushes.BlanchedAlmond : Brushes.White;

        /// <summary>
        /// Возвращаем целевое изображение с контролла
        /// </summary>
        /// <returns>Целевое изображение</returns>
        public ImageSource GetImage() =>
            FindedImageIcon.Source;

        /// <summary>
        /// Закрываем поток в памяти, связанный с изображением
        /// </summary>
        public void CloseImageSource()
        {
            //Если есть исходный поток в памяти
            if (FindedImageIcon.Source != null)
            {
                //Проучаем изображение
                BitmapImage source = (BitmapImage)FindedImageIcon.Source;
                //Очищаем поток
                source.StreamSource.Dispose();
                //Закрываем поток
                source.StreamSource.Close();
                //Сбрасываем источник
                FindedImageIcon.Source = null;
            }
        }

        /// <summary>
        /// Проставляем новое значение чекбоксу
        /// </summary>
        /// <param name="state">Новое значение чекбоксу</param>
        public void SetCheckBoxState(bool state) =>
            SelectImageCheckBox.IsChecked = state;

        /// <summary>
        /// Проставляем инфу в контролл
        /// </summary>
        /// <param name="info">Информация о контролле</param>
        public void SetControlInfo(DuplicateResult info)
        {
            //Запоминаем переданное значение
            _result = info;
            //Проставляем инфу в текстовые поля
            ImageParentFolderToolTip.Content = info.ParentPath;
            ImageParentFolderRun.Text = $"[{info.ParentName}]";
            ImageNameToolTip.Content = ImageNameRun.Text = info.Name;
            ImageSizeRun.Text = $"{info.Width}x{info.Height}";
        }



        /// <summary>
        /// Метод выполнения подгрузки изображения в контролл
        /// </summary>
        public void LoadImage()
        {
            //Закрываем поток в памяти, связанный с изображением
            CloseImageSource();
            //Грузим картинку в контролл
            FindedImageIcon.Source = LoadImageByPath(_result.Path);
        }
    }
}
