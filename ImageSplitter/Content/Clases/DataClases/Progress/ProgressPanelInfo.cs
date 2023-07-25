using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.DataClases.Progress
{
    /// <summary>
    /// Класс информации о прогрессе для панели прогресса
    /// </summary>
    public class ProgressPanelInfo
    {
        /// <summary>
        /// Текст заголовка информации о прогрессе
        /// </summary>
        public string HeaderText { get; set; }
        /// <summary>
        /// Максимальное значение прогресса
        /// </summary>
        public int Maximum { get; set; }
        /// <summary>
        /// Текущее значение прогресса
        /// </summary>
        public int Current { get; set; }
        /// <summary>
        /// Флаг бесконечной прокрутки прогресса
        /// </summary>
        public bool IsIndeterminate { get; set; }
        /// <summary>
        /// Флаг отображения прогресса в заголовке
        /// </summary>
        public bool IsViewProgress { get; set; }
        /// <summary>
        /// Флаг отображения дополнительной инфы
        /// </summary>
        public bool IsViewAddInfo { get; set; }
        /// <summary>
        /// Блок дополнительной информации
        /// </summary>
        public List<ProgressAddInfo> AddInfo { get; set; }

        /// <summary>
        /// Метод получения заголовка
        /// </summary>
        public string GetHeader => 
            //Если нужно отобразить прогресс в заголовке - отображаем
            (IsViewProgress) ? $"[{HeaderText}] [{CurrentPercent}%]" : HeaderText;

        /// <summary>
        /// Получение текущего процента прогресса
        /// </summary>
        private int CurrentPercent => (Current * 100) / Maximum;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="headerText">Текст заголовка информации о прогрессе</param>
        /// <param name="maximum">Максимальное значение прогресса</param>
        /// <param name="current">Текущее значение прогресса</param>
        public ProgressPanelInfo(string headerText, int maximum, int current)
        {
            //Проставляем переданные значения
            HeaderText = headerText;
            Maximum = maximum;
            Current = current;
            //Проставляем дефолтные значения
            AddInfo = new List<ProgressAddInfo>();
            IsViewAddInfo = IsIndeterminate = false;
            IsViewProgress = true;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ProgressPanelInfo()
        {
            //Проставляем дефолтные значения
            AddInfo = new List<ProgressAddInfo>();
            IsViewAddInfo = IsIndeterminate = false;
            IsViewProgress = true;
            HeaderText = "";
            Maximum = Current = 0;
        }
    }
}
