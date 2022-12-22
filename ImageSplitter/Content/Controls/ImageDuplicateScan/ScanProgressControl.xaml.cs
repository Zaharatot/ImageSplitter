using DuplicateScanner.Clases.DataClases.Result;
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

namespace ImageSplitter.Content.Controls.ImageDuplicateScan
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
        /// Выполняем обновление информации о прогрессе сканирования
        /// </summary>
        /// <param name="info">Информация о прогрессе сканирования</param>
        public void UpdateScanInfo(ScanProgressInfo info)
        {
            //ВЫбираем действия по стадиям
            switch (info.Stage)
            {
                case ScanStages.FindFiles:
                    {
                        //Проставляем текст стадии
                        StageNameRun.Text = $"{info.Stage}";
                        //Делаем прогрессбар бесконечным
                        ScanProgressBar.IsIndeterminate = true;
                        //Скрываем блок доп. инфы
                        AddInfoTextBlock.Visibility = Visibility.Collapsed;
                        break;
                    }
                case ScanStages.HashGeneration:
                    {
                        //Проставляем текст стадии, с доп. инфой о прогрессе
                        StageNameRun.Text = $"{info.Stage} [{info.ProcessedFiles} / {info.FilesToProcess}]";
                        //Проставляем значения в прогрессбар
                        ScanProgressBar.Maximum = info.FilesToProcess;
                        ScanProgressBar.Value = info.ProcessedFiles;
                        //Делаем прогрессбар обычным
                        ScanProgressBar.IsIndeterminate = false;
                        //Проставляем доп. инфу
                        FilesFindedRun.Text = info.FilesFinded.ToString();
                        ErrorFilesRun.Text = info.ErrorFilesCount.ToString();
                        FilesToProcessRun.Text = info.FilesToProcess.ToString();
                        LoadedFilesRun.Text = info.LoadedFiles.ToString();
                        ProcessedFilesRun.Text = info.ProcessedFiles.ToString();
                        //Отображаем блок доп. инфы
                        AddInfoTextBlock.Visibility = Visibility.Visible;
                        break;
                    }
                case ScanStages.SavingData:
                    {
                        //Проставляем текст стадии
                        StageNameRun.Text = $"{info.Stage}";
                        //Делаем прогрессбар бесконечным
                        ScanProgressBar.IsIndeterminate = true;
                        //Скрываем блок доп. инфы
                        AddInfoTextBlock.Visibility = Visibility.Collapsed;
                        break;
                    }
                case ScanStages.DuplicateFind:
                    {
                        //Проставляем текст стадии
                        StageNameRun.Text = $"{info.Stage} [{info.ProcessedFiles} / {info.FilesToProcess}]";
                        //Проставляем значения в прогрессбар
                        ScanProgressBar.Maximum = info.FilesToProcess;
                        ScanProgressBar.Value = info.ProcessedFiles;
                        //Делаем прогрессбар обычным
                        ScanProgressBar.IsIndeterminate = false;
                        //Скрываем блок доп. инфы
                        AddInfoTextBlock.Visibility = Visibility.Collapsed;
                        break;
                    }
            }
        }

        /// <summary>
        /// Обновляем информацию об удалении
        /// </summary>
        /// <param name="info">Информация об удалении</param>
        public void UpdateRemoveInfo(ProgressInfo info)
        {
            //Проставляем текст стадии
            StageNameRun.Text = $"Удаление файлов [{info.Processed} / {info.MaxCount}]";
            //Проставляем значения в прогрессбар
            ScanProgressBar.Maximum = info.MaxCount;
            ScanProgressBar.Value = info.Processed;
            //Делаем прогрессбар обычным
            ScanProgressBar.IsIndeterminate = false;
            //Скрываем блок доп. инфы
            AddInfoTextBlock.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// Обновляем информацию о визуализации
        /// </summary>
        /// <param name="max">Общее количество изображений</param>
        /// <param name="processed">Количество обработанных изображений</param>
        public void UpdateVisualizeStage(int processed, int max)
        {
            //Проставляем текст стадии
            StageNameRun.Text = $"Отображение результатов [{processed} / {max}]";
            //Проставляем значения в прогрессбар
            ScanProgressBar.Maximum = max;
            ScanProgressBar.Value = processed;
            //Делаем прогрессбар обычным
            ScanProgressBar.IsIndeterminate = false;
            //Скрываем блок доп. инфы
            AddInfoTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
