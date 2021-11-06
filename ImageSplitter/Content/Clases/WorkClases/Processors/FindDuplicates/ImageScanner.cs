using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.FindDuplicates
{
    /// <summary>
    /// Класс, выполняющий поиск всех дочерних 
    /// изображений, и вычисление для них хешей
    /// </summary>
    internal class ImageScanner
    {

        /// <summary>
        /// Класс работы с ДКП-хешами
        /// </summary>
        private readonly DCTHash _dctHash;
        /// <summary>
        /// Массив поддерживаемых расширений для изображений
        /// </summary>
        private string[] _imageExtensions;



        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="dctHash">Ссылка на класс вычисления ДКП-хеша</param>
        public ImageScanner(DCTHash dctHash)
        {
            //Проставляем переданные значения
            _dctHash = dctHash;
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Формируем список поддерживаемых расширений
            _imageExtensions = GetImageExtensions();
        }


        /// <summary>
        /// Получаем список расширений для изображений
        /// </summary>
        /// <returns>Список расширений для изображений</returns>
        private string[] GetImageExtensions() =>
            new string[] {
                ".bmp", ".png", ".jpg", ".jpeg", ".gif"
            };

        /// <summary>
        /// Проверяем формат файла на допустимость
        /// </summary>
        /// <param name="file">Инфомрация о файле</param>
        /// <returns>True - файл является поддерживаемой картинкой</returns>
        private bool FileIsImage(FileInfo file) =>
            //Проверяем наличие расширения этого файла в списке допустимых
            _imageExtensions.Contains(file.Extension.ToLower());

        /// <summary>
        /// Получаем все изображения из папки
        /// </summary>
        /// <param name="parent">ИНформация о родительской папке</param>
        /// <returns>Список путей к дочерним изображениям</returns>
        private List<string> GetImagesFromDirectory(DirectoryInfo parent) =>
            //Из папки
            parent
                //Получаем все дочерние файлы
                .GetFiles()
                //Из них выбираем только изображения
                .Where(file => FileIsImage(file))
                //Получаем от них полный путь
                .Select(image => image.FullName)
                //Возвращаем в виде списка
                .ToList();

        /// <summary>
        /// Получаем список путей ко всем файлам
        /// </summary>
        /// <param name="parentPath">Путь к родительской папке</param>
        /// <param name="paths">Список найденных путей</param>
        private void GetAllFilePathsRecurse(DirectoryInfo parent, ref List<string> paths)
        {            
            //Добавляем все дочерние файлы текущей папки в список
            paths.AddRange(GetImagesFromDirectory(parent));
            //Проходимся по всем папкам
            foreach (var folder in parent.GetDirectories())
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
            //ПОлучаем родительскую директорию
            DirectoryInfo parent = new DirectoryInfo(path);
            //Получаем пути ко всем файлам в папке сканирования
            GetAllFilePathsRecurse(parent, ref pathList);
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
        /// Получаем выполненные
        /// </summary>
        /// <param name="path">Путь к папке сканирования</param>
        /// <returns></returns>
        public List<CreateHashTask> GetFiles(string path)
        {  
            //Запускаем выполнение рассчёта хешей
            List<Task<CreateHashTask>> tasks = StartHashCalculation(path);
            //Ожидаем завершения всех тасок
            WaitAllTasks(tasks);
            //Получаем результаты из тасок и возвращаем их
            return tasks.ConvertAll(task => task.Result).ToList();
        }
    }
}
