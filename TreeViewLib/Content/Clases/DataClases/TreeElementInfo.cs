using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TreeViewWindowLib.Content.Clases.DataClases.Global.Enums;

namespace TreeViewWindowLib.Content.Clases.DataClases
{
    /// <summary>
    /// Класс информации об элементе древа
    /// </summary>
    public class TreeElementInfo
    {

        /// <summary>
        /// Имя элемента древа
        /// </summary>
        public string Name => _directory.Name;

        /// <summary>
        /// Список дочерних элементов
        /// </summary>
        public List<TreeElementInfo> Childs { get; private set; }
        /// <summary>
        /// Флаг наличия дочерних элементоа
        /// </summary>
        public int CountChildFiles { get; private set; }
        /// <summary>
        /// Флаг наличия дочерних директорий
        /// </summary>
        public DirectoryStatuses Status { get; private set; }
        /// <summary>
        /// Количество нерасспличенных дочерних элементов
        /// </summary>
        public int NotSplittedChilds { get; private set; }


        /// <summary>
        /// Флаг не расспличенной папки
        /// </summary>
        internal bool IsNotSplitted { get; private set; }
        /// <summary>
        /// Флаг не расспличенных дочерних
        /// </summary>
        internal bool IsChildsNotSplitted { get; private set; }


        /// <summary>
        /// Информация о директории
        /// </summary>
        private DirectoryInfo _directory;



        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="directory">Класс информации о папке</param>
        public TreeElementInfo(DirectoryInfo directory)
        {
            //Проставляем переданные значения
            _directory = directory;
            //Проставляем дефолтные значения
            IsChildsNotSplitted = IsNotSplitted = false;
            Status = DirectoryStatuses.Splitted;
            CountChildFiles = 0;
            //Инициализируем список дочерних элементов
            Childs = new List<TreeElementInfo>();
        }



        /// <summary>
        /// Проверка на доступный для работы файл
        /// </summary>
        /// <param name="file">Файл для проверки</param>
        /// <returns>True - файл не скрытый</returns>
        private bool IsNotHiddenFile(FileInfo file) =>
            //Проверяем флаги у целевого файла
            !file.Attributes.HasFlag(FileAttributes.System | FileAttributes.Hidden);

        /// <summary>
        /// Метод получения количества дочерних файлов в папке
        /// </summary>
        /// <returns>Количетсво файлов</returns>
        private int GetCountChildFiles() =>
            //Получаем из папки 
            _directory
                //Все дочерние файлы
                .GetFiles()
                //Отсекаем скрытые и системные
                .Where(IsNotHiddenFile)
                //Возвращаем их количество
                .Count();

        /// <summary>
        /// Метод получения статуса папки
        /// </summary>
        /// <returns>Значение статуса папки</returns>
        private DirectoryStatuses GetDirectoryStatus() =>
            //Если текущая папка не расспличена
            (IsNotSplitted)
                //Возвращаем соответствующий статус
                ? DirectoryStatuses.NotSplitted
                //Если не расспличены дочерние
                : (IsChildsNotSplitted)
                    //Возвращаем соответствующий статус
                    ? DirectoryStatuses.ChildsNotSplitted
                    //В полтивном случае - папка расспличена
                    : DirectoryStatuses.Splitted;
        
        /// <summary>
        /// Метод обновления статуса папки
        /// </summary>
        private void UpdateStatus()
        {
            //Если в папке есть дочерние файлы и папки - она не расспличена
            IsNotSplitted = ((CountChildFiles > 0) && (Childs.Count > 0));
            //Проверяем на сплит дочерние и дочерние дочерних
            IsChildsNotSplitted = Childs.Any(child => child.IsNotSplitted || child.IsChildsNotSplitted);
            //Получаем статус папки
            Status = GetDirectoryStatus();
        }



        /// <summary>
        /// Метод запроса обновления информации об элементе
        /// </summary>
        public void UpdateInfo()
        {
            //Получаем количество дочерних файлов
            CountChildFiles = GetCountChildFiles();
            //Выполняем обновление информации о дочерних
            Childs.ForEach(child => child.UpdateInfo());
            //ВАЖНО! Эти вызовы должны быть только после обновления дочерних!
            //Обновляем статус папки
            UpdateStatus();
        }

        /// <summary>
        /// Метод сортировки дочерних элементов по имени
        /// </summary>
        public void OrderChildsByName() =>
            //Сортируем элементы по имени
            Childs = Childs.OrderBy(elem => elem.Name).ToList();
    }
}
