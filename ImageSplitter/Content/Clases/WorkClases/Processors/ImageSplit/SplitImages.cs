using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Global;
using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases.Addition;
using ImageSplitter.Content.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static ImageSplitter.Content.Clases.DataClases.Global.Enums;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit
{
    /// <summary>
    /// Класс выполнения сплита изображений
    /// </summary>
    internal class SplitImages
    {
        /// <summary>
        /// Класс сканиварованяи изображений
        /// </summary>
        private CollectionsScanner _scanner;
        /// <summary>
        /// Класс переноса изображений
        /// </summary>
        private Mover _mover;
        /// <summary>
        /// Класс поиска клавиш по id папки
        /// </summary>
        private KeyFinder _keyFinder;



        /// <summary>
        /// Список коллекций для обработки
        /// </summary>
        private List<CollectionInfo> _collections;
        /// <summary>
        /// Список целевых папок
        /// </summary>
        private List<TargetFolderInfo> _targets;

        /// <summary>
        /// Id текущей выбранной коллекции
        /// </summary>
        private int _currentCollectionId;
        /// <summary>
        /// Путь к списку папок
        /// </summary>
        private string _foldersPath;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SplitImages()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем дефолтные значения
            _collections = new List<CollectionInfo>();
            _targets = new List<TargetFolderInfo>();
            _currentCollectionId = 0;
            _foldersPath = null;
            //Инициализируем используемые классы
            _scanner = new CollectionsScanner();
            _mover = new Mover();
            _keyFinder = new KeyFinder();
        }

        /// <summary>
        /// Метод вызова ивента обновления основной инфы о сплите
        /// </summary>
        private void InvokeImageSplitInfoRequest() =>
            //Вызываем ивент, передав в него собранные данные
            GlobalEvents.InvokeUpdateImageSplitInfoRequest(GetImagesInfo(), _targets);

        /// <summary>
        /// Получаем инфу об изображениях
        /// </summary>
        /// <returns>Статус обработки изображений</returns>
        private string GetImagesInfo()
        {
            //Получаем количество обработанных изображений
            int moved = _collections.Count(img => img.IsMoved);
            //Выводим инфу об результатах
            return $"{_currentCollectionId + 1} / {moved} / {_collections.Count}";
        }


        /// <summary>
        /// Сканируем и выбираем папки
        /// </summary>
        /// <param name="foldersPath">Путь к целевым папкам</param>
        private void ScanAndSelectFolders(string foldersPath)
        {
            //Получаем список папок после сканирования
            List<TargetFolderInfo> foldersList = _scanner.ScanFolders(foldersPath);
            //С окном работаем уже в ui-потоке
            App.Current.Dispatcher.Invoke(new Action(() => {
                //Инициализируем окно выбора папок
                SelectFoldersWindow foldersWindow = new SelectFoldersWindow();
                //Получаем список папок для работы
                foldersList = foldersWindow.GetFoldersToWork(foldersList, _targets);
                //Проставляем список папок
                _targets = foldersList;
                //Запрашиваем обновление инфы о сплите
                InvokeImageSplitInfoRequest();
                //Вызываем ивент завершения сканирования
                GlobalEvents.InvokeScanComplete();
            }));
        }

        /// <summary>
        /// Обновляем кнопки для целей
        /// </summary>
        private void UpdateTargetsKeys()
        {
            //Проходимся по списку целей
            for(int i = 0; i < _targets.Count; i++)
                //Проставляем новый ключ для папки
                _targets[i].TargetKey = _keyFinder.GetKeyByNumber(i);
        }



        /// <summary>
        /// Проверяем нажатую кнопку на тип кнопки переноса
        /// </summary>
        /// <param name="key">Код нажатой кнопки</param>
        /// <returns>True - нажатие было обработано</returns>
        public bool CheckImageMoveTarget(Key key)
        {
            bool ex = false;
            //Получаем целевую папку, по нажатой кнопке
            TargetFolderInfo target = GetMoveFolder(key);
            //Если с этой кнопкой проассациирована папка
            if (target != null)
            {
                //Получаем текущую выбранную картинку
                CollectionInfo image = GetCurrentImageInfo();
                //Переносим изображение
                _mover.MoveCollection(target, image);
                //Указываем, что нажатие было обработано
                ex = true;
                //Вызываем ивент по завершеию переноса изображения
                GlobalEvents.InvokeMoveImageComplete();
            }
            //ВОзвращаем результат
            return ex;
        }

        /// <summary>
        /// Получаем папку для перемещения по нажатой кнопке
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>Папка для перемещеия</returns>
        public TargetFolderInfo GetMoveFolder(Key key) =>
            _targets.FirstOrDefault(trg => (trg.TargetKey == key));

        /// <summary>
        /// Возвращаем текущую выбранную картинку
        /// </summary>
        /// <returns>ИНформация о выбранной картинке</returns>
        public CollectionInfo GetCurrentImageInfo() =>
            _collections[_currentCollectionId];

        /// <summary>
        /// Переходим к указанной картинке
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns>Инфомрация о картинке</returns>
        public CollectionInfo MoveToImage(int direction)
        {
            CollectionInfo ex = null;
            //Если картинки вообще есть
            if (_collections.Count > 0)
            {
                //Переходим по направлению
                _currentCollectionId += direction;
                //Засовываем значение идентификатора
                //обратно в рамки если оно зха них вышло
                if (_currentCollectionId >= _collections.Count)
                    _currentCollectionId = _collections.Count - 1;
                if (_currentCollectionId < 0)
                    _currentCollectionId = 0;
                //Возвращаем картинку
                ex = _collections[_currentCollectionId];
                //Запрашиваем обновление инфы о сплите
                InvokeImageSplitInfoRequest();
            }
            return ex;
        }

        /// <summary>
        /// Запускаем сканирование
        /// </summary>
        /// <param name="imagesPath">Путь к папкам с картинками</param>
        /// <param name="foldersPath">Путь к целевым папкам</param>
        /// <param name="isFolder">Флаг поиска папок</param>
        public void StartScan(string imagesPath, string foldersPath, bool isFolder)
        {
            //Запускаем эту работу в отдельном потоке
            new Thread(() => {
                //Запоминаем имя папки с картинками, добавляя слеш в конец при необходимости
                _foldersPath = (foldersPath.Last() != '\\') ? $"{foldersPath}\\" : foldersPath;
                //Сбрасываем id выбранной картинки
                _currentCollectionId = 0;
                //Сканим папку на предмет коллекций
                _collections = _scanner.ScanCollections(imagesPath, isFolder);
                //Сканим целевые папки для добавления в программу
                ScanAndSelectFolders(_foldersPath);
            }).Start();
        }


        /// <summary>
        /// Метод добавления новой папки
        /// </summary>
        public void AddNewFolder()
        {
            //Если путь к папке существует
            if (!string.IsNullOrEmpty(_foldersPath))
            {
                //Инициализируем окно добавления папки
                AddFolderWindow folderWindow = new AddFolderWindow();
                //Получаем результат ввода нового имени папки
                var newFolderName = folderWindow.GetNewFolderName(_foldersPath);
                //Если было возвращено корректное имя папки
                if (!string.IsNullOrEmpty(newFolderName))
                {
                    //Получаем полный путь к новой папке
                    string path = $"{_foldersPath}{newFolderName}\\";
                    //Если папки не существует
                    if(!Directory.Exists(path))
                        //Создаём папку
                        Directory.CreateDirectory(path);
                    //Добавляем папку в список целей
                    _targets.Add(new TargetFolderInfo() {
                        Name = newFolderName,
                        Path = path,
                        TargetKey = _keyFinder.GetKeyByNumber(_targets.Count)
                    });
                    //Запрашиваем обновление инфы о сплите
                    InvokeImageSplitInfoRequest();
                }
            }
        }


        /// <summary>
        /// Метод удаления папки из списка
        /// </summary>
        /// <param name="key">Клавиша, к которой привязана папка</param>
        public void RemoveFolderFromList(Key key)
        {
            //Удаляем все папки привязанные к указанной клавише
            _targets.RemoveAll(folder => folder.TargetKey == key);
            //Обновляем клавиши для папок
            UpdateTargetsKeys();
            //Запрашиваем обновление инфы о сплите
            InvokeImageSplitInfoRequest();
        }
    }
}
