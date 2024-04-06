using SplitterResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitterSimpleUI.Content.Clases.WorkClases.Data
{

    /// <summary>
    /// Класс форматирования даты/времени
    /// </summary>
    public class DateFormatter
    {
        /// <summary>
        /// Экземпляр класса форматирования даты для синглтона
        /// </summary>
        private static DateFormatter _instance;



        /// <summary>
        /// Текстовая подсказка для дней
        /// </summary>
        private string _daysText;
        /// <summary>
        /// Текстовая подсказка для часов
        /// </summary>
        private string _hoursText;
        /// <summary>
        /// Текстовая подсказка для минут
        /// </summary>
        private string _minutesText;
        /// <summary>
        /// Текстовая подсказка для секунд
        /// </summary>
        private string _secundsText;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DateFormatter()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Загружаем значения из ресурсов
            _daysText = ResourceLoader.LoadString("Text_DateLimitDay");
            _hoursText = ResourceLoader.LoadString("Text_DateLimitHour");
            _minutesText = ResourceLoader.LoadString("Text_DateLimitMinute");
            _secundsText = ResourceLoader.LoadString("Text_DateLimitSecund");
        }


        /// <summary>
        /// Форматируем значение секунд в строку
        /// </summary>
        /// <param name="secunds">Количество секунд для обработки</param>
        /// <returns>Форматированная строка лимита</returns>
        public string GetFormatDateLimit(double secunds)
        {
            //Если у нас больше минуты
            if (secunds > 60)
            {
                //Получаем минуты и секунды
                double minutes = secunds / 60;
                //Если у нас больше часа
                if (minutes > 60)
                {
                    //Получаем часы и минуты
                    double hours = minutes / 60;
                    //Если у нас больше дня
                    if (hours > 24)
                    {
                        //Получаем дни и часы
                        double days = hours / 24;
                        //Возвращаем результат
                        return $"{days.ToString("F0")} {_daysText}";
                    }
                    //В противном случае
                    else
                        //Возвращаем результат
                        return $"{hours.ToString("F0")} {_hoursText}";
                }
                //В противном случае
                else
                    //Возвращаем результат
                    return $"{minutes.ToString("F0")} {_minutesText}";
            }
            //В противном случае
            else
                //Возвращаем результат
                return $"{secunds.ToString("F0")} {_secundsText}";
        }



        /// <summary>
        /// Метод получения экземпляра класса
        /// </summary>
        /// <returns>Экземпляр класса</returns>
        public static DateFormatter GetInstance()
        {
            //Если экземпляра класса ещё нет
            if (_instance == null)
                //Инициализируем его
                _instance = new DateFormatter();
            //Возвращаем результат
            return _instance;
        }
    }
}
