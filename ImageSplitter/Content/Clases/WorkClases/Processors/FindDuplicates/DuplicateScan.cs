using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Duplicates;
using ImageSplitter.Content.Clases.DataClases.Global;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.FindDuplicates
{
    /// <summary>
    /// Класс поиска дубликатов
    /// </summary>
    internal class DuplicateScan : IDisposable
    {
        /// <summary>
        /// Класс работы с ДКП-хешами
        /// </summary>
        private DCTHash _dctHash;
        /// <summary>
        /// Класс поиска изображений в указанной папке
        /// </summary>
        private ImageScanner _imageScanner;
        /// <summary>
        /// Класс конвертации информации об изображенияъ
        /// </summary>
        private ImageInfoConverter _imageInfoConverter;
        /// <summary>
        /// Класс поиска дубликатов
        /// </summary>
        private DuplicateFinder _duplicateFinder;
        /// <summary>
        /// Класс поиска лучшего дубликата
        /// </summary>
        private BestDuplicateFinder _bestDuplicateFinder;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicateScan()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем класс работы с ДКП-хешем
            _dctHash = new DCTHash();
            //Инициализируем класс поиска изображений
            _imageScanner = new ImageScanner(_dctHash);
            //Инициализируем класс конвертации информации об изображенияъ
            _imageInfoConverter = new ImageInfoConverter();
            //Инициализируем класс поиска дубликатов
            _duplicateFinder = new DuplicateFinder(_dctHash);
            ////Инициализируем класс поиска лучшего дубликата
            _bestDuplicateFinder = new BestDuplicateFinder();
        }


        /// <summary>
        /// Проверка отсутствия дубликатов
        /// </summary>
        /// <param name="duplicates">Список дубликатов для проверки</param>
        /// <returns>TRue - дубликатов не найдено</returns>
        private bool IsNotFoundDuplicates(List<DuplicateImageInfo> duplicates) =>
            duplicates.All(image => image.Duplicates.Count == 0);

        /// <summary>
        /// Ивент завершения поиска дубликатов
        /// </summary>
        /// <param name="duplicates">Список найденных дубликатов</param>
        private void InvokeCompleteFindDuplicatesEvent(List<DuplicateImageInfo> duplicates)
        {
            //Если дубликатов не найдено
            if (IsNotFoundDuplicates(duplicates))
                //Вызываем ивент отсутствия дубликатов по указанному пути
                GlobalEvents.InvokeDuplicateScanNotFound();
            //Если дубликаты всё-таки есть
            else
                //Вызываем ивент завершения сканирования на дубликаты
                GlobalEvents.InvokeDuplicateScanComplete(duplicates);
        }




        /// <summary>
        /// Запуск сканирования дубликатов
        /// </summary>
        /// <param name="path">Путь к папке сканирования</param>
        public void StartDuplicateScan(string path)
        {
            //Делаем всё это в отдельном потоке
            new Thread(() => {
                //Если он не оканчивается на слеш
                if (path.Last() != '\\')
                    //доабвляем его
                    path += "\\";
                //Получаем список файлов находящихся в указанной папке
                List<CreateHashTask> files = _imageScanner.GetFiles(path);
                // ВЫполняем конвертацию списка файлов в список классов информации о дубликатах
                List<DuplicateImageInfo> duplicates = 
                    _imageInfoConverter.ConvertTasksToDuplicateInfo(files);
                //Выполняем поиск дубликатов в списке
                duplicates = _duplicateFinder.FindDuplicates(duplicates);
                //Проставляем флаг необходимости удаления всем дубликатам
                _bestDuplicateFinder.ProcessDuplicatesIsNeedRemove(duplicates);
                //Вызываем ивент завершения поиска дубликатов
                InvokeCompleteFindDuplicatesEvent(duplicates);
            }).Start();
        }

        /// <summary>
        /// Метод запуска удаления дублиткатов
        /// </summary>
        /// <param name="duplicates">Список дубликатов для удаления</param>
        public void RemoveDuplicates(List<DuplicateImageInfo> duplicates)
        {
            //Делаем всё это в отдельном потоке
            new Thread(() => {
                //Проходимся по списку дубликатов
                foreach (var duplicate in duplicates)
                    //Для каждого из них удаляем привязанный файл
                    File.Delete(duplicate.Path);
                //Вызываем ивент завершения удаления дубликатов
                GlobalEvents.InvokeRemoveDuplicatesComplete();
            }).Start();
        }

        /// <summary>
        /// Метод очистки неуправляемых ресурсов класса
        /// </summary>
        public void Dispose()
        {
            //Завершаем работу с классом вычисления ДКП-хеша
            _dctHash?.Dispose();
        }
    }
}
