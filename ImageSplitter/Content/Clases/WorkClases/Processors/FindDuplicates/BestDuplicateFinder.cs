using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Duplicates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.FindDuplicates
{
    /// <summary>
    /// Класс поиска лучшей версии дубликата
    /// </summary>
    internal class BestDuplicateFinder
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public BestDuplicateFinder()
        {

        }


        /// <summary>
        /// Проставляем элементам флаг необходимости удаления
        /// </summary>
        /// <param name="duplicates">Список дубликатов</param>
        private void SetIsNeedRemoveFlag(List<DuplicateImageInfo> duplicates) =>
            //Берём список дубликатов
            duplicates
                //Сортируем по количеству пикселей
                .OrderBy(image => image.PixelsCount)
                //Пропускаем первый в списке элемент
                .Skip(1)
                //Возвращаем в виде списка
                .ToList()
                //Проставляем всем оставшимся флаг необходимости удаления
                .ForEach(image => image.IsNeedRemove = true);




        /// <summary>
        /// Обрабатываем дубликаты на предмет простановки флага необходимости удаления
        /// </summary>
        /// <param name="duplicates">Список дубликатов</param>
        public void ProcessDuplicatesIsNeedRemove(List<DuplicateImageInfo> duplicates)
        {
            DuplicateImageInfo buff;
            //Проходимся по списку дубликатов
            foreach (var duplicate in duplicates)
            {
                //Проставляем флаги всем дубликатам
                SetIsNeedRemoveFlag(duplicate.Duplicates);
                //Получаем дубликат без флага
                buff = duplicate.Duplicates.FirstOrDefault(image => !image.IsNeedRemove);
                //Если дубликат без флага найден
                if (buff != null)
                {
                    //Если у дубликата больше пикселей
                    if (buff.PixelsCount > duplicate.PixelsCount)
                        //Указываем что нужно удалить оригинал
                        duplicate.IsNeedRemove = true;
                    //Если у оригинала пикселей больше
                    else
                        //Указываем, что нужно удалить дубликат
                        buff.IsNeedRemove = true;
                }
            }
        }

    }
}
