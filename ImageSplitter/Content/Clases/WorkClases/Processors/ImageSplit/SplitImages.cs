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
        /// Класс работы с путём сплита
        /// </summary>
        private SplitPathProcessor _splitPathProcessor;
        /// <summary>
        /// Класс работы с древом
        /// </summary>
        private TreeElementsProcessor _treeElementsProcessor;
        /// <summary>
        /// Класс выбора папок
        /// </summary>
        private SelectFoldersProcessor _selectFoldersProcessor;
        /// <summary>
        /// Класс указания имени папки
        /// </summary>
        private CreateFolderProcessor _createFolderProcessor;



        /// <summary>
        /// Список коллекций для обработки
        /// </summary>
        private List<CollectionInfo> _collections;

        /// <summary>
        /// Id текущей выбранной коллекции
        /// </summary>
        private int _currentCollectionId;

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
            _currentCollectionId = 0;
            //Инициализируем используемые классы
            _scanner = new CollectionsScanner();
            _mover = new Mover();
            _splitPathProcessor = new SplitPathProcessor();
            _treeElementsProcessor = new TreeElementsProcessor();
            _selectFoldersProcessor = new SelectFoldersProcessor();
            _createFolderProcessor = new CreateFolderProcessor();
        }

        /// <summary>
        /// Метод вызова ивента обновления основной инфы о сплите
        /// </summary>
        private void InvokeImageSplitInfoRequest() =>
            //Вызываем ивент, передав в него собранные данные
            GlobalEvents.InvokeUpdateImageSplitInfoRequest(
                GetImagesInfo(), _selectFoldersProcessor.Targets);

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
        private void ScanAndSelectFolders()
        {
            //Получаем список папок после сканирования
            List<TargetFolderInfo> foldersList = 
                _scanner.ScanFolders(_splitPathProcessor.SplitPath);
            //С окном работаем уже в ui-потоке
            App.Current.Dispatcher.Invoke(new Action(() => {
                //Выполняем выбор папок для работы
                _selectFoldersProcessor.SelectFolders(foldersList);
                //Запрашиваем обновление инфы о сплите
                InvokeImageSplitInfoRequest();
                //Вызываем ивент завершения сканирования
                GlobalEvents.InvokeScanComplete();
            }));
        }

        /// <summary>
        /// Метод добавления папки в список
        /// </summary>
        /// <param name="foldersPath">Путь к папке для перемещения</param>
        /// <param name="newFolderName">Имя новой папки</param>
        private void AddDirectory(string foldersPath, string newFolderName)
        {
            //Получаем полный путь к новой папке
            string path = $"{foldersPath}{newFolderName}\\";
            //Если папки не существует
            if (!Directory.Exists(path))
                //Создаём папку
                Directory.CreateDirectory(path);
            //Добавляем папку в список целей
            _selectFoldersProcessor.AddNewTarget(newFolderName, path);
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
        /// Откатываем перемещение коллекции
        /// </summary>
        public void UndoMove()
        {
            //Если коллекции вообще есть
            if (_collections.Count > 0)
            {
                //Получаем текущую выбранную картинку
                CollectionInfo image = GetCurrentImageInfo();
                //Откатываем перенос изображения
                _mover.UndoMove(image);
            }
        }

        /// <summary>
        /// Получаем папку для перемещения по нажатой кнопке
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>Папка для перемещеия</returns>
        public TargetFolderInfo GetMoveFolder(Key key) =>
            //Вызываем внутренний метод
            _selectFoldersProcessor.GetMoveFolder(key);

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
        public void StartScan()
        {
            //Запускаем эту работу в отдельном потоке
            new Thread(() => {
                //Сбрасываем id выбранной картинки
                _currentCollectionId = 0;
                //Сканим папку на предмет коллекций
                _collections = _scanner.ScanCollections(_splitPathProcessor.SplitPath);
                //Сканим целевые папки для добавления в программу
                ScanAndSelectFolders();
            }).Start();
        }



        /// <summary>
        /// Метод добавления новой папки
        /// </summary>
        public void AddNewFolder()
        {
            //Получаем путь к папке для перемещения
            string foldersPath = _splitPathProcessor.SplitPath.MovePath;
            //Если путь к родительской папке существует
            if (!string.IsNullOrEmpty(foldersPath))
            {                
                //Получаем результат ввода нового имени папки
                string newFolderName = _createFolderProcessor.GetFolderName();
                //Если было возвращено корректное имя папки
                if (!string.IsNullOrEmpty(newFolderName))
                {
                    //Добавляем папку
                    AddDirectory(foldersPath, newFolderName);
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
            //Удаляем папку по ключу
            _selectFoldersProcessor.RemoveTargetByKey(key);
            //Запрашиваем обновление инфы о сплите
            InvokeImageSplitInfoRequest();
        }



        /// <summary>
        /// Метод обновления пути сплита
        /// </summary>
        public void UpdateSplitPath()
        {
            //Получаем пути для сплита
            SplitPathsInfo info = _splitPathProcessor.UpdateSplitPath();
            //Вызываем ивент обновления инфы
            GlobalEvents.InvokeUpdateSplitPath(info);
        }

        /// <summary>
        /// Метод отображения древа
        /// </summary>
        public void ShowTree() =>
            //Вызываем внутренний метод, передав в него путь для перемещения
            _treeElementsProcessor.ShowTree(_splitPathProcessor.SplitPath.MovePath);
    }
}
