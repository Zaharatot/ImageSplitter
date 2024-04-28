using ImageSplitterLib.Clases.DataClases;
using ImageSplitterLib.Clases.WorkClases.Collection;
using ImageSplitterLib.Clases.WorkClases.Targets;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitterLib.Clases.WorkClases
{
    /// <summary>
    /// Класс выполнения сплита изображений
    /// </summary>
    internal class SplitImages
    {


        /// <summary>
        /// Текущая выбранная картинка
        /// </summary>
        public CollectionInfo CurrentImageInfo => 
            //Если коллекция есть
            (_collections != null && _collections.Count > 0) 
                //Вернём её, в противном случае - вернём null
                ? _collections[_currentCollectionId] : null;


        /// <summary>
        /// Класс сканиварованяи изображений
        /// </summary>
        private CollectionsScanner _collectionScanner;
        /// <summary>
        /// Класс переноса изображений
        /// </summary>
        private CollectionMover _collectionMover;
        /// <summary>
        /// Класс работы с целевыми папками
        /// </summary>
        private TargetsProcessor _targetsProcessor;

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
            _collectionScanner = new CollectionsScanner();
            _collectionMover = new CollectionMover();
            //Получаем экземпляр класса работы с целями
            _targetsProcessor = TargetsProcessor.GetInstance();
        }


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
            _targetsProcessor.AddNewTarget(newFolderName, path);
        }

        /// <summary>
        /// Метод вызова ивента обновления основной инфы о сплите
        /// </summary>
        private void InvokeImageSplitInfoRequest() =>
            //Вызываем ивент, передав в него собранные данные
            ImageSplitterFasade.InvokeUpdateImageSplitInfoRequest(
                GetImagesInfo(), _targetsProcessor.Targets);






        /// <summary>
        /// Метод завершения выбора папок
        /// </summary>
        /// <param name="folders">Список папок для простановки</param>
        public void CompleteSelectFolders(List<TargetFolderInfo> folders)
        {
            //Вызываем простановку нового списка целей
            _targetsProcessor.SetNewTargets(folders);
            //Отображаем в окно информацию о найденных элементах
            InvokeImageSplitInfoRequest();
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
            TargetFolderInfo target = _targetsProcessor.GetMoveFolder(key);
            //Если с этой кнопкой проассациирована папка
            if (target != null)
            {
                //Получаем текущую выбранную картинку, и переносим её
                _collectionMover.MoveCollection(target, CurrentImageInfo);
                //Указываем, что нажатие было обработано
                ex = true;
                //Вызываем ивент по завершеию переноса изображения
                ImageSplitterFasade.InvokeMoveImageComplete();
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
                //Откатываем перенос текущего изображения
                _collectionMover.UndoMove(CurrentImageInfo);
        }

        /// <summary>
        /// Переходим к указанной картинке
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns>Инфомрация о картинке</returns>
        public CollectionInfo MoveToCollection(int direction)
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
        /// Запускаем поиск коллекций
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public void StartScanCollections(SplitPathsInfo info) =>
            //Запускаем эту работу в отдельном потоке
            new Thread(() => {
                //Сбрасываем id выбранной картинки
                _currentCollectionId = 0;
                //Сканим папку на предмет коллекций
                _collections = _collectionScanner.ScanCollections(info);
                //Вызываем событие завершения поиска коллекций
                ImageSplitterFasade.InvokeScanCollectionsComplete();
            }).Start();

        /// <summary>
        /// Выполняем поиск папок
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public List<TargetFolderInfo> ScanFolders(SplitPathsInfo info)
        {
            //Получаем список выбранных папок
            List<TargetFolderInfo> folders = _collectionScanner.ScanFolders(info);
            //Обновляем выделение для списка папок
            _targetsProcessor.UpdateFoldersSelectionFromOldList(ref folders);
            //Возвращаем выделенные папки
            return folders;
        }


        /// <summary>
        /// Метод добавления новой папки
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        /// <param name="newFolderName">Имя создаваемой папки</param>
        public void AddNewFolder(SplitPathsInfo info, string newFolderName)
        {
            //Получаем путь к папке для перемещения
            string foldersPath = info.MovePath;
            //Если путь к родительской папке существует
            if (!string.IsNullOrEmpty(foldersPath))
            {
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
            _targetsProcessor.RemoveTargetByKey(key);
            //Запрашиваем обновление инфы о сплите
            InvokeImageSplitInfoRequest();
        }


        /// <summary>
        /// Перемещаемся к изображению внутри папки
        /// </summary>
        /// <param name="direction">Направление перемещения</param>
        public void MoveToImage(int direction)
        {
            //Если есть активная коллекция, и она содержит папки
            if ((CurrentImageInfo != null) && (CurrentImageInfo.IsFolder))
                //Перемещаемся к новому изображению внутри коллекции
                CurrentImageInfo.MoveToImage(direction);
        }
    }
}
