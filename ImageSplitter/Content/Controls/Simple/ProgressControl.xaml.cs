using DuplicateScanner.Clases.DataClases.Result;
using ImageSplitter.Content.Clases.DataClases.Progress;
using ImageSplitter.Content.Clases.WorkClases.Resources;
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
using static DuplicateScanner.Clases.DataClases.Global.Enums;

namespace ImageSplitter.Content.Controls.Simple
{
    /// <summary>
    /// Логика взаимодействия для ScanProgressControl.xaml
    /// </summary>
    public partial class ScanProgressControl : UserControl
    {

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public ScanProgressControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Метод обновления количества контроллов в панели
        /// </summary>
        /// <param name="panel">Панель для обновления</param>
        /// <param name="count">Количество контроллов в панели</param>
        private void UpdatePanelControlsCount(StackPanel panel, int count)
        {
            //Получаем количество контроллов для обновления
            int toUpdate = panel.Children.Count - count;
            //Если контроллов больше чем нужно
            if (toUpdate > 0)
                //Удаляем лишние
                panel.Children.RemoveRange(0, toUpdate);
            //Если контроллов меньше чем нужно
            else if (toUpdate < 0)
                //Проходимся в цикле
                for(int i = 0; i < -toUpdate; i++)
                    //И добавляем новые текстовые блоки
                    panel.Children.Add(new TextBlock());
        }

        /// <summary>
        /// Обновляем контент для элементов панелей доп. информации
        /// </summary>
        /// <param name="panel">Панель для обновления</param>
        /// <param name="addInfos">Список блоков информации для панели</param>
        private void UpdateAddInfoPanelsContent(StackPanel panel, List<ProgressAddInfo> addInfos)
        {
            //ПРоходимся по информации для панели
            for(int i = 0; i < addInfos.Count; i++)
                //Проставляем в контроллы панели контент
                (panel.Children[i] as TextBlock).Text = addInfos[i].GetText;
        }

        /// <summary>
        /// Обновляем дополнительную информацию
        /// </summary>
        /// <param name="addInfos">Список классов информации для панели</param>
        private void UpdateAddInfo(List<ProgressAddInfo> addInfos)
        {
            //Получаем количество элементов в каждой из панелей
            int rightCount = addInfos.Count / 2;
            //Если останется лишний - он пойдёт в левую панель
            int leftCount = rightCount + addInfos.Count % 2;
            //Обновляем количество контроллов в панелях
            UpdatePanelControlsCount(LeftInfoPanel, leftCount);
            UpdatePanelControlsCount(RightInfoPanel, rightCount);
            //Обновляем контент в контроллах панелей
            UpdateAddInfoPanelsContent(LeftInfoPanel, addInfos.Take(leftCount).ToList());
            UpdateAddInfoPanelsContent(RightInfoPanel, addInfos.Skip(leftCount).ToList());
        }

        /// <summary>
        /// Метод обновления панели прогресса
        /// </summary>
        /// <param name="info">Информация о прогрессе</param>
        public void UpdateProgress(ProgressPanelInfo info)
        {
            //Проставляем текст заголовка прогресса
            ProgressHeaderTextBlock.Text = info.GetHeader;
            //Проставляем значения в прогрессбар
            ScanProgressBar.Maximum = info.Maximum;
            ScanProgressBar.Value = info.Current;
            //Делаем прогрессбар обычным
            ScanProgressBar.IsIndeterminate = info.IsIndeterminate;
            //Обновляем видимость панелей доп. информации
            LeftInfoPanel.Visibility = RightInfoPanel.Visibility =
                info.IsViewAddInfo ? Visibility.Visible : Visibility.Collapsed;
            //Обновляем дополнительную информацию
            UpdateAddInfo(info.AddInfo);
        }
    }
}
