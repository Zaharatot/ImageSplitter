using DuplicateScannerLib.Clases.DataClases.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScannerLib.Clases.WorkClases.Comparers
{
    /// <summary>
    /// Класс сравнения элементов
    /// </summary>
    internal class FilePairComparer : IEqualityComparer<DuplicatePair>
    {
        /// <summary>
        /// Метод сравнения пары элементов
        /// </summary>
        /// <param name="elem1">Первый элемент для сравнения</param>
        /// <param name="elem2">Второй элемент для сравнения</param>
        /// <returns>True - элементы равны</returns>
        public bool Equals(DuplicatePair elem1, DuplicatePair elem2) =>
            //Чекаем прямое соответствие файлов пары
            ((elem1.Original.PathHash == elem2.Original.PathHash) && (elem1.Copy.PathHash == elem2.Copy.PathHash)) ||
            //Чекаем обратное соответствие файлов пары
            ((elem1.Original.PathHash == elem2.Copy.PathHash) && (elem1.Copy.PathHash == elem2.Original.PathHash));



        /// <summary>
        /// Метод получения хеша
        /// </summary>
        public int GetHashCode(DuplicatePair elem) => 0;
        //Этот хеш игнорируем, проверка идёт по внутренним хешам

    }
}
