using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanner.Clases.DataClases.File
{
    /// <summary>
    /// Класс группы хешей
    /// </summary>
    public class HashesGroup
    {
        /// <summary>
        /// Список хешей
        /// </summary>
        public List<uint> HashList { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="hashList">Список хешей</param>
        public HashesGroup(List<uint> hashList)
        {
            //Проставляем переданные значения
            HashList = hashList;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public HashesGroup()
        {
            //Проставляем дефолтные значения
            HashList = new List<uint>();
        }
    }
}
