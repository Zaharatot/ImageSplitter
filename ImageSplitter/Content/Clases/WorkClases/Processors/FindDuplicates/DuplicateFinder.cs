using DCTHashZ;
using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Duplicates;
using ImageSplitter.Content.Clases.DataClases.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.FindDuplicates
{
    /// <summary>
    /// Класс поиска дубликатов в списке 
    /// классов информации об изображениях
    /// </summary>
    internal class DuplicateFinder
    {
        /// <summary>
        /// Класс работы с ДКП-хешами
        /// </summary>
        private readonly DCTHash _dctHash;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="dctHash">Ссылка на класс вычисления ДКП-хеша</param>
        public DuplicateFinder(DCTHash dctHash)
        {
            //Проставляем переданные значения
            _dctHash = dctHash;
        }


        /// <summary>
        /// Рукурсивный поиск дубликатов
        /// </summary>
        /// <param name="targets">Список целей поиска</param>
        /// <param name="duplicates">Список дубликатов для поиска</param>
        /// <param name="result">Список результатов</param>
        private void GetDuplicatesRecurce(List<DuplicateImageInfo> targets, 
            ref List<DuplicateImageInfo> duplicates, ref List<DuplicateImageInfo> result)
        {
            List<DuplicateImageInfo> buff;
            //Проходимся по целям поиска
            foreach (DuplicateImageInfo target in targets)
            {
                //Получаем все дубликаты элемента
                buff = GetElementDuplicates(target, ref duplicates);
                //Удаляем найденные дубликаты из списка
                foreach (DuplicateImageInfo duplicate in buff)
                    duplicates.Remove(duplicate);
                //Добавляем найденные дубликаты в список
                result.AddRange(buff);
                //Вызываем этот метод рекурсивно для найденных дубликатов
                GetDuplicatesRecurce(buff, ref duplicates, ref result);
            }
        }

        /// <summary>
        /// Получаем все дубликаты изображения
        /// </summary>
        /// <param name="target">Целевое изображение</param>
        /// <param name="duplicates">Список для проверки</param>
        /// <returns>Список дубликатов</returns>
        private List<DuplicateImageInfo> GetElementDuplicates(DuplicateImageInfo target, 
            ref List<DuplicateImageInfo> duplicates) =>
            //В списке дубликатов
            duplicates
                //Выбираем только те, что входят в список дубликатов
                .Where(image => _dctHash.EqalImageHash(target.Hash, image.Hash))
                //Возвращаем их в виде списка
                .ToList();


        /// <summary>
        /// Находим дубликаты среди файлов
        /// </summary>
        /// <param name="duplicates">Список дубликатов</param>
        /// <returns>Список найденных дубликатов</returns>
        public List<DuplicateImageInfo> FindDuplicates(List<DuplicateImageInfo> duplicates)
        {
            List<DuplicateImageInfo> ex = new List<DuplicateImageInfo>();
            List<DuplicateImageInfo> buff;
            DuplicateImageInfo target;
            //Максимальное количество действий
            //Т.к. идёт сначала сканирование а
            //потом поиск дубликатов - просто удваиваем количество
            int maxCount = duplicates.Count * 2;
            int current;
            //Пока есть ещё файлы для обработки
            while (duplicates.Count > 0)
            {
                //Получаем целевой элемент
                target = duplicates[0];
                //Удаляем целевой элемент из списка дубликатов
                duplicates.Remove(target);
                //Очищаем старый список дубликатов
                buff = new List<DuplicateImageInfo>();
                //Получаем все дубликаты для целевого элемента
                GetDuplicatesRecurce(
                    new List<DuplicateImageInfo>() { target },
                    ref duplicates,
                    ref buff
                );
                //Проставляем дубликаты элементу
                target.Duplicates = buff;
                //Добавляем целевой элемент в выходной список
                ex.Add(target);
                //Получаем оставшееся количество задач
                current = maxCount - duplicates.Count;
                //Вызываем ивент, передав в него текущий статус
                GlobalEvents.InvokeDuplicateScanProgress(current, maxCount);
            }
            //Возвращаем результат
            return ex;
        }
    }
}
