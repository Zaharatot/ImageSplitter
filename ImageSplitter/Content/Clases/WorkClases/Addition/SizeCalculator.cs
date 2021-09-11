using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.WorkClases.Addition
{

    /// <summary>
    /// Класс, выполняющий формирволане строка для размера
    /// </summary>
    internal class SizeCalculator
    {
        /// <summary>
        /// Количество байт в килобайте (и т.д.)
        /// Константу добавил, чтобы в случае чего можно было 
        /// легко заменить 1024 на 1000.
        /// </summary>
        private const int SIZE = 1024;


        /// <summary>
        /// Массив имён размеров
        /// </summary>
        private string[] _sileNames;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SizeCalculator()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор коасса
        /// </summary>
        private void Init()
        {
            //Инициализируем массив размеров
            _sileNames = new string[] { "b", "Kb", "Mb", "Gb", "Tb" };
        }


        /// <summary>
        /// Получаем форматированную строку размера
        /// </summary>
        /// <param name="value">Числовое значение размера в байтах</param>
        /// <returns>Строка размера</returns>
        public string GetStringSize(double value)
        {
            //Дефолтное пустое значение
            string ex = "";
            //Id названия единицы измерения
            byte id = 0;

            try
            {
                //Если мы не дошли до предела измерений
                //И размер всё ещё больше лимита
                while ((id < 4) && (value > SIZE))
                {
                    //Делим на порядок
                    value /= SIZE;
                    //И переходим к следующему 
                    //названию единицы измерения
                    id++;
                }

                //Получаем название текущей единицы измерения
                ex = _sileNames[id];
                //Формируем строку из округлённого значения и названия порядка
                ex = $"{GetRoundedValue(value)} {ex}";
            }
            catch
            {
                ex = $"0 {_sileNames[0]}";
            }

            return ex;
        }

        /// <summary>
        /// Возвращаем округлённое значение
        /// </summary>
        /// <param name="value">Изначальное значение</param>
        /// <returns>Строка округлённого значения</returns>
        private string GetRoundedValue(double value) =>
            //Округляем до 2 знака после запятой и конвертим в строку
            Math.Round(value, 2).ToString();
    }
}
