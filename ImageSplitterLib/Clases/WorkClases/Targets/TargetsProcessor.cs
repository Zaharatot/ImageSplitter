using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitterLib.Clases.WorkClases.Targets
{
    /// <summary>
    /// Класс работы с целевыми папками
    /// </summary>
    internal class TargetsProcessor
    {
        /// <summary>
        /// Объект класса для синглтона
        /// </summary>
        private static TargetsProcessor _instance;

        /// <summary>
        /// Список целевых папок
        /// </summary>
        public List<TargetFolderInfo> Targets { get; set; }


        /// <summary>
        /// Класс поиска клавиш по id папки
        /// </summary>
        private KeyFinder _keyFinder;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TargetsProcessor()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор окна
        /// </summary>
        private void Init()
        {
            //Инициализируем класс поиска клавиш
            _keyFinder = new KeyFinder();
            //Инициализируем дефолтные значения
            Targets = new List<TargetFolderInfo>();
        }


        /// <summary>
        /// Обновляем кнопки для целей
        /// </summary>
        private void UpdateTargetsKeys()
        {
            //Проходимся по списку целей
            for (int i = 0; i < Targets.Count; i++)
                //Проставляем новый ключ для папки
                Targets[i].TargetKey = _keyFinder.GetKeyByNumber(i);
        }



        /// <summary>
        /// Метод добавления новой папки в список целей
        /// </summary>
        /// <param name="name">Имя для новой папки</param>
        /// <param name="path">Путь к новой папке</param>
        internal void AddNewTarget(string name, string path) =>
            //Добавляем папку в список целей
            Targets.Add(new TargetFolderInfo(path, name,
                _keyFinder.GetKeyByNumber(Targets.Count)));

        /// <summary>
        /// Метод удаления папки по ключу
        /// </summary>
        /// <param name="key">Ключ папки для удаления</param>
        internal void RemoveTargetByKey(Key key)
        {
            //Удаляем все папки привязанные к указанной клавише
            Targets.RemoveAll(folder => folder.TargetKey == key);
            //Обновляем ключи для папок
            UpdateTargetsKeys();
        }

        /// <summary>
        /// Получаем папку для перемещения по нажатой кнопке
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>Папка для перемещеия</returns>
        public TargetFolderInfo GetMoveFolder(Key key) =>
            Targets.FirstOrDefault(trg => (trg.TargetKey == key));


        /// <summary>
        /// Обновляем список целевых папок
        /// </summary>
        /// <param name="folders">Список папок для простановки</param>
        public void SetNewTargets(List<TargetFolderInfo> folders)
        {
            //Проходимся по папкам
            for (int i = 0; i < folders.Count; i++)
                //Для каждой добавляем клавишу
                folders[i].TargetKey = _keyFinder.GetKeyByNumber(i);
            //Обновляем список целевых папок
            Targets = folders;
        }


        /// <summary>
        /// Проставляем выделение для папок по предыдущим выбранным
        /// </summary>
        /// <param name="folders">Список папок для обработки</param>
        public void UpdateFoldersSelectionFromOldList(ref List<TargetFolderInfo> folders) =>
            //Для каждой из свежепереданных папок
            folders.ForEach(folder =>
                //проставляем флаг выбора, в случае, если она была в предыдущем списке
                folder.IsSelected = Targets.Any(target => target.Name.Equals(folder.Name)));



        /// <summary>
        /// Метод получения экземпляра класса
        /// </summary>
        /// <returns>Экземпляр класса</returns>
        public static TargetsProcessor GetInstance()
        {
            //Если екземпляр ещё не был создан
            if (_instance == null)
                //Инициализируем его
                _instance = new TargetsProcessor();
            //Возвращаем результат
            return _instance;
        }
    }
}
