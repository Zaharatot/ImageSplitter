using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanner.Clases.WorkClases.Hash
{

    /// <summary>
    /// Класс рассчёта Crc32 хеша
    /// </summary>
    internal class Crc32
    {
        /// <summary>
        /// Константа порождающего полинома
        /// </summary>
        private const uint POLY = 0xedb88320;
        /// <summary>
        /// Константа длинны таблицы полиномов
        /// </summary>
        private const int POLY_TABLE_LENGTH = 256;

        /// <summary>
        /// Экземпляр класса для паттерна синглтон
        /// </summary>
        private static Crc32 _instance;

        /// <summary>
        /// Таблица степеней полинома
        /// </summary>
        private uint[] _table;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public Crc32()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем таблицу степеней
            _table = CreatePolyTable();
        }

        /// <summary>
        /// Метод генерации таблицы полиномов
        /// </summary>
        /// <returns>Сформированная таблица полиномов</returns>
        private uint[] CreatePolyTable()
        {
            //Инициализируем выходной массив
            uint[] table = new uint[POLY_TABLE_LENGTH];
            //Проходимся по таблице степеней
            for (uint i = 0; i < table.Length; i++)
            {
                //Запоминаем значение индекса
                table[i] = i;
                //Проходимся от конца бита к началу
                for (int j = 8; j > 0; j--)
                    //Если бит равен единице
                    if ((table[i] & 1) == 1)
                        //Возвращаем значение степени
                        table[i] = (table[i] >> 1) ^ POLY;
                    //В противном случае
                    else
                        //Сдвигаем буфер
                        table[i] >>= 1;
            }
            //Возвращаем заполненную таблицу
            return table;
        }



        /// <summary>
        /// Метод рассчёта чексуммы по массиву байт
        /// </summary>
        /// <param name="bytes">МАссив байт для рассчёта</param>
        /// <returns>Значение чексуммы</returns>
        public uint ComputeChecksum(byte[] bytes)
        {
            byte id;
            //Инициализируем значение заполненное единицами
            uint crc = 0xffffffff;
            //Проходимся по байтам
            foreach (byte t in bytes)
            {
                //Получаем значение идентификатора
                id = (byte)((crc & 0xff) ^ t);
                //ПОлучаем значение для бита
                crc = (crc >> 8) ^ _table[id];
            }
            //Возвращаем результат
            return crc;
        }

        /// <summary>
        /// Метод рассчёта чексуммы по массиву байт
        /// </summary>
        /// <param name="data">Строка для рассчёта</param>
        /// <returns>Значение чексуммы</returns>
        public uint ComputeChecksum(string data)
        {
            byte id;
            //Инициализируем значение заполненное единицами
            uint crc = 0xffffffff;
            //Получаем байты из строки
            byte[] bytes = Encoding.Default.GetBytes(data);
            //Проходимся по байтам
            foreach (byte t in bytes)
            {
                //Получаем значение идентификатора
                id = (byte)((crc & 0xff) ^ t);
                //ПОлучаем значение для бита
                crc = (crc >> 8) ^ _table[id];
            }
            //Возвращаем результат
            return crc;
        }


        /// <summary>
        /// Метод получения экземпляра класса для паттерна синглтон
        /// </summary>
        /// <returns>Экземпляр класса</returns>
        public static Crc32 GetInstance()
        {
            //Если экземпляр не был создан ранее
            if (_instance == null)
                //ИНициализируем его
                _instance = new Crc32();
            //Возвращаем экземпляр класса
            return _instance;
        }

    }
}
