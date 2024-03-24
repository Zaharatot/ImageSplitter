using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases.Addition;
using ImageSplitter.Content.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit
{
    /// <summary>
    /// Класс работы с окном выбора папок для сплита
    /// </summary>
    internal class SelectFoldersProcessor
    {
        /// <summary>
        /// Список целевых папок
        /// </summary>
        public List<TargetFolderInfo> Targets => _targets;



        /// <summary>
        /// Класс поиска клавиш по id папки
        /// </summary>
        private KeyFinder _keyFinder;
        /// <summary>
        /// Список целевых папок
        /// </summary>
        private List<TargetFolderInfo> _targets;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SelectFoldersProcessor()
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
            _targets = new List<TargetFolderInfo>();
        }

        /// <summary>
        /// Проставляем выделение для папок из списка
        /// </summary>
        /// <param name="folders">Список папок для обработки</param>
        private void UpdateFoldersSelection(ref List<TargetFolderInfo> folders) =>
            //Для каждой из свежепереданных папок
            folders.ForEach(folder =>
                //проставляем флаг выбора, в случае, если она была в предыдущем списке
                folder.IsSelected = _targets.Any(target => target.Name.Equals(folder.Name)));

        /// <summary>
        /// Обновляем список папок после выделения
        /// </summary>
        /// <param name="folders">Список папок для обработки</param>
        private void UpdateFoldersAfterSelection(ref List<TargetFolderInfo> folders)
        {
            //Удаляем все не выделенные папки
            folders.RemoveAll(folder => !folder.IsSelected);
            //Проходимся по папкам
            for(int i = 0; i < folders.Count; i++)
                //Для каждой добавляем клавишу
                folders[i].TargetKey = _keyFinder.GetKeyByNumber(i);
        }






        /// <summary>
        /// Получаем папку для перемещения по нажатой кнопке
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>Папка для перемещеия</returns>
        public TargetFolderInfo GetMoveFolder(Key key) =>
            _targets.FirstOrDefault(trg => (trg.TargetKey == key));

        /// <summary>
        /// Метод удаления папки по ключу
        /// </summary>
        /// <param name="key">Ключ папки для удаления</param>
        public void RemoveTargetByKey(Key key)
        {
            //Удаляем все папки привязанные к указанной клавише
            _targets.RemoveAll(folder => folder.TargetKey == key);
            //Обновляем ключи для папок
            UpdateTargetsKeys();
        }

        /// <summary>
        /// Метод добавления новой папки в список целей
        /// </summary>
        /// <param name="name">Имя для новой папки</param>
        /// <param name="path">Путь к новой папке</param>
        public void AddNewTarget(string name, string path) =>
            //Добавляем папку в список целей
            _targets.Add(new TargetFolderInfo(path, name, 
                _keyFinder.GetKeyByNumber(_targets.Count)));

        /// <summary>
        /// Обновляем кнопки для целей
        /// </summary>
        public void UpdateTargetsKeys()
        {
            //Проходимся по списку целей
            for (int i = 0; i < _targets.Count; i++)
                //Проставляем новый ключ для папки
                _targets[i].TargetKey = _keyFinder.GetKeyByNumber(i);
        }



        /// <summary>
        /// Метод запуска выбора папок
        /// </summary>
        /// <param name="folders">Список папок для обработки</param>
        public void SelectFolders(List<TargetFolderInfo> folders)
        {
            //Инициализируем окно выбора папок
            SelectFoldersWindow foldersWindow = new SelectFoldersWindow();
            //Обновляем флаги выбора для папок по старому списку
            UpdateFoldersSelection(ref folders);
            //Обновляем список папок в окне
            foldersWindow.UpdateFoldersList(folders);
            //Отображаем окно как диалоговое, и если оно было успешно закрыто
            if (foldersWindow.ShowDialog().GetValueOrDefault(false))
            {
                //Обновляем выделение в папках по контроллу
                foldersWindow.GetSelectedFolders(ref folders);
                //Обновляем список папок после выделения
                UpdateFoldersAfterSelection(ref folders);
                //Обновляем текущий список папок
                _targets = folders;
            }
            //В противном случае
            else
                //Чистим список выбранных папок
                folders.Clear();
        }
    }
}
