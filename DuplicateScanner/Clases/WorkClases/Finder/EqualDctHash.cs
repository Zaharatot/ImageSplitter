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
    }
}
