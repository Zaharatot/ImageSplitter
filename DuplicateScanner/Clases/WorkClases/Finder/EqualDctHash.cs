using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanner.Clases.WorkClases.Finder
{
    /// <summary>
    /// Класс сравнения хешей
    /// </summary>
    internal class EqualDctHash
    {
        /// <summary>
        /// Чувствительность для сравнения изображений
        /// Дефолтом считается значение в 21, так что его и поставлю
        /// </summary>
        private const int DCT_HASH_EQUAL_SENSIVITY = 9;
        /// <summary>
        /// Чувствительность для сравнения лайновых изображений
        /// Её нужно ставить сильно меньше, чтобы было меньше ложных срабатываний
        /// </summary>
        private const int LINED_DCT_HASH_EQUAL_SENSIVITY = 9;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public EqualDctHash()
        {

        }



        /// <summary>
        /// Получаем схожесть хешей
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <returns>Значение схожести хешей</returns>
        private int GetHashSimilarity(ulong first, ulong second)
        {
            int ex = 0;
            //Проходимся по битам
            for (int i = 63; i >= 0; i--)
            {
                //Получаем биты и сравниваем
                if ((first >> i & 1) != (second >> i & 1))
                    //Если биты не равны - увеличиваем выход
                    ex++;
            }
            return ex;
        }

        /// <summary>
        /// Сравниваем хеши изображений
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <param name="sensivity">Значение чувствительности сравнения</param>
        /// <returns>True - картинки схожы</returns>
        public bool EqalHash(ulong? first, ulong? second, int sensivity) =>
            //Если оба хеша присутствуют
            (first.HasValue && second.HasValue) &&
            //И результат проверки ниже лимита
            (GetHashSimilarity(first.Value, second.Value) < sensivity);



        /// <summary>
        /// Сравниваем хеши изображений
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <returns>True - картинки схожы</returns>
        public bool EqalImageHash(ulong? first, ulong? second) => 
            //Выполняем сравнение с обычной чуствительностью
            EqalHash(first, second, DCT_HASH_EQUAL_SENSIVITY);

        /// <summary>
        /// Сравниваем хеши лайновых изображений
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <returns>True - картинки схожы</returns>
        public bool EqalLinedImageHash(ulong? first, ulong? second) =>
            //Выполняем сравнение с обычной чуствительностью для лайны
            EqalHash(first, second, LINED_DCT_HASH_EQUAL_SENSIVITY);

    }
}
