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

namespace ImageSplitter.Content.Clases.WorkClases.Processors
{
    /// <summary>
    /// Класс поиска дубликатов
    /// </summary>
    internal class DuplicateScan
    {
        /// <summary>
        /// Класс работы с ДКП-хешами
        /// </summary>
        private DCTHash _dctHash;



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
        }

        /// <summary>
        /// Получаем список путей ко всем файлам
        /// </summary>
        /// <param name="parentPath">Путь к родительской папке</param>
        /// <param name="paths">Список найденных путей</param>
        private void GetAllFilePathsRecurse(string parentPath, ref List<string> paths)
        {
            //Добавляем все дочерние файлы текущей папки в список
            paths.AddRange(Directory.GetFiles(parentPath));
            //Получаем список всех дочерних папок
            string[] folders = Directory.GetDirectories(parentPath);
            //Проходимся по всем папкам
            foreach (var folder in folders)
                //Для каждой из них вызываем рекурсивно этот метод
                GetAllFilePathsRecurse(folder, ref paths);
        }

        /// <summary>
        /// Запускаем выполнение рассчёта хешей
        /// </summary>
        /// <param name="path">Путь к папке сканирования</param>
        /// <returns>Список таско по нахождению хешей для каждого из целевых элементов</returns>
        private List<Task<CreateHashTask>> StartHashCalculation(string path)
        {
            //Инициализируем список путей к файлам
            List<string> pathList = new List<string>();
            //Получаем пути ко всем файлам в папке сканирования
            GetAllFilePathsRecurse(path, ref pathList);
            //Создаём задачи по нахождению хешей для всех дочерних элементов
            return _dctHash.AddTasksAsync(pathList);
        }

        /// <summary>
        /// Ожидаем завершения всех тасок
        /// </summary>
        /// <param name="tasks">Список тасок</param>
        private void WaitAllTasks(List<Task<CreateHashTask>> tasks)
        {
            int countCompleted;
            //Максимальное количество действий
            //Т.к. идёт сначала сканирование а
            //потом поиск дубликатов - просто удваиваем количество
            int maxCount = tasks.Count * 2;
            //Цикл идёт пока все таски не будут завершены
            do
            {
                //ПОлучаем количество завершённых задач
                countCompleted = tasks.Count(task => task.IsCompleted);
                //Вызываем ивент, передав в него текущий статус
                GlobalEvents.InvokeDuplicateScanProgress(countCompleted, maxCount);
                //Спим немного
                Thread.Sleep(500);
            //Цикл идёт пока все таски не будут завершены
            } while (countCompleted != tasks.Count);
        }

        /// <summary>
        /// ВЫполняем конвертацию списка тасок в список классов информации о дубликатах
        /// </summary>
        /// <param name="tasks">Список тасок</param>
        /// <returns>Список классов информации о дубикатах</returns>
        private List<DuplicateImageInfo> ConvertTasksToDuplicateInfo(List<Task<CreateHashTask>> tasks)
        {
            List<DuplicateImageInfo> ex = new List<DuplicateImageInfo>();
            //Проходимся по списку тасок
            for (int i = 0; i < tasks.Count; i++)
                //КОнвертируем результат таски в класс информации о
                //дубликате и добавляем в выходной список
                ex.Add(ConvertTaskResultToDuplicateInfo(tasks[i].Result, i));
            //Возвращаем результат
            return ex;
        }

        /// <summary>
        /// Получаем имя целоевой директории
        /// </summary>
        /// <param name="path">Путь к директории</param>
        /// <returns>Имя директории</returns>
        private string GetDirectoryName(string path) =>
            new DirectoryInfo(path).Name;


        /// <summary>
        /// Загружаем картинку по строке пути
        /// </summary>
        /// <param name="path">Путь к файлу картинки на диске</param>
        /// <returns>Класс картинки</returns>
        private BitmapImage LoadImageByPath(string path)
        {
            BitmapImage ex = new BitmapImage();
            ex.BeginInit();
            //Считываем байты файла в поток в памяти
            ex.StreamSource = new MemoryStream(File.ReadAllBytes(path));
            ex.EndInit();
            return ex;
        }

        /// <summary>
        /// Получаем разрешение изображения
        /// </summary>
        /// <param name="image">Изображение для получения разрешения</param>
        /// <returns>Строка с разрешением изображения</returns>
        private string GetImageResolution(BitmapImage image) =>
            $"[{image.PixelWidth}x{image.PixelHeight}]";

        /// <summary>
        /// Получаем количество пикселей изображения
        /// </summary>
        /// <param name="image">Изображение для получения разрешения</param>
        /// <returns>Количество пикселей изображения</returns>
        private uint GetImagePixelsCount(BitmapImage image) =>
            (uint)(image.PixelWidth * image.PixelHeight);


        /// <summary>
        /// ВЫполняем конвертацию задачи по поиску хеша в класс информации о дубликате
        /// </summary>
        /// <param name="taskResult">Результат выполнения таски</param>
        /// <param name="id">Идентификатор изображения в списке</param>
        /// <returns>Класс нформации о дубликате</returns>
        private DuplicateImageInfo ConvertTaskResultToDuplicateInfo(CreateHashTask taskResult, int id)
        {
            //Получаем полный путь к картинке
            string path = taskResult.GetFullPath();
            //Грузим целевое изображение
            BitmapImage image = LoadImageByPath(path);
            //Формируем итоговый класс
            DuplicateImageInfo imageInfo = new DuplicateImageInfo() {
                Id = (uint)id,
                Name = taskResult.FileName,
                Hash = taskResult.Hash.GetValueOrDefault(0),
                Path = path,
                ParentFolderName = GetDirectoryName(taskResult.Path),
                Resolution = GetImageResolution(image),
                PixelsCount = GetImagePixelsCount(image)
            };
            //Завершаем поток изображения
            image.StreamSource.Dispose();
            //Возвращаем результат
            return imageInfo;
        }


        /// <summary>
        /// Находим дубликаты среди файлов
        /// </summary>
        /// <param name="duplicates">Список дубликатов</param>
        /// <returns>Список найденных дубликатов</returns>
        private List<DuplicateImageInfo> FindDuplicates(List<DuplicateImageInfo> duplicates)
        {
            List<DuplicateImageInfo> ex = new List<DuplicateImageInfo>(); 
            List<DuplicateImageInfo> buff = new List<DuplicateImageInfo>();
            DuplicateImageInfo target;
            //Максимальное количество действий
            //Т.к. идёт сначала сканирование а
            //потом поиск дубликатов - просто удваиваем количество
            int maxCount = duplicates.Count * 2;
            int current, currentCount = duplicates.Count;
            //Пока есть ещё файлы для обработки
            while(duplicates.Count > 0)
            {
                //TODO: что-то я нахреначил не то в алгоритме поиска дубликатов


                //Получаем целевой элемент
                target = duplicates[0];
                //Удаляем целевой элемент из списка дубликатов
                duplicates.Remove(target);
                //Очищаем старый список дубликатов
                buff.Clear();
                //Получаем все дубликаты для целевого элемента
                GetDuplicatesRecurce(
                    new List<DuplicateImageInfo>() { target },
                    duplicates,
                    ref buff
                );
                //Проставляем дубликаты элементу
                target.Duplicates = buff;
                //Получаем оставшееся количество задач
                current = (currentCount - duplicates.Count) + currentCount;
                //Добавляем целевой элемент в выходной список
                ex.Add(target);
                //Вызываем ивент, передав в него текущий статус
                GlobalEvents.InvokeDuplicateScanProgress(current, maxCount);
            }
            //Возвращаем результат
            return ex;
        }

        /// <summary>
        /// Рукурсивный поиск дубликатов
        /// </summary>
        /// <param name="targets">Список целей поиска</param>
        /// <param name="duplicates">Список дубликатов для поиска</param>
        /// <param name="result">Список результатов</param>
        private void GetDuplicatesRecurce(List<DuplicateImageInfo> targets, List<DuplicateImageInfo> duplicates, ref List<DuplicateImageInfo> result)
        {
            List<DuplicateImageInfo> buff;
            //Проходимся по целям поиска
            foreach (DuplicateImageInfo target in targets)
            {
                //Получаем все дубликаты элемента
                buff = GetElementDuplicates(target, duplicates);
                //Удаляем найденные дубликаты из списка
                foreach (var duplicate in buff)
                    duplicates.Remove(duplicate);
                //Добавляем найденные дубликаты в список
                result.AddRange(buff);
                //Вызываем этот метод рекурсивно для найденных дубликатов
                GetDuplicatesRecurce(buff, duplicates, ref result);
            }
        }

        /// <summary>
        /// Получаем все дубликаты изображения
        /// </summary>
        /// <param name="target">Целевое изображение</param>
        /// <param name="duplicates">Список для проверки</param>
        /// <returns>Список дубликатов</returns>
        private List<DuplicateImageInfo> GetElementDuplicates(DuplicateImageInfo target, List<DuplicateImageInfo> duplicates) =>
            //В списке дубликатов
            duplicates
                //Выбираем только те, что входят в список дубликатов
                .Where(image => _dctHash.EqalImageHash(target.Hash, image.Hash))
                //Возвращаем их в виде списка
                .ToList();

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
        private void ProcessDuplicatesIsNeedRemove(List<DuplicateImageInfo> duplicates)
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
                //Запускаем выполнение рассчёта хешей
                List<Task<CreateHashTask>> tasks = StartHashCalculation(path);
                //Ожидаем завершения всех тасок
                WaitAllTasks(tasks);
                // ВЫполняем конвертацию списка тасок в список классов информации о дубликатах
                List<DuplicateImageInfo> duplicates = ConvertTasksToDuplicateInfo(tasks);
                //Выполняем поиск дубликатов в списке
                duplicates = FindDuplicates(duplicates);
                //Проставляем флаг необходимости удаления всем дубликатам
                ProcessDuplicatesIsNeedRemove(duplicates);
                //Вызываем ивент завершения сканирования на дубликаты
                GlobalEvents.InvokeDuplicateScanComplete(duplicates);
            }).Start();
        }
    }
}
