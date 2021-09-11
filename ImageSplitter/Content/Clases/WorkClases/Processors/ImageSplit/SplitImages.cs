using ImageSplitter.Content.Clases.DataClases;
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
using static ImageSplitter.Content.Clases.DataClases.Delegates;
using static ImageSplitter.Content.Clases.DataClases.Enums;

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
        private ImageScanner _scanner;
        /// <summary>
        /// Класс переноса изображений
        /// </summary>
        private Mover _mover;
        /// <summary>
        /// Класс поиска клавиш по id папки
        /// </summary>
        private KeyFinder _keyFinder;



        /// <summary>
        /// Список изображений для обработки
        /// </summary>
        private List<ImageInfo> _images;
        /// <summary>
        /// Список целевых папок
        /// </summary>
        private List<TargetFolderInfo> _targets;

        /// <summary>
        /// Id текущего выбранног оизображдения
        /// </summary>
        private int _currentImageId;
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
            _images = new List<ImageInfo>();
            _targets = new List<TargetFolderInfo>();
            _currentImageId = 0;
            _foldersPath = null;
            //Инициализируем используемые классы
            _scanner = new ImageScanner();
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
            int completed = _images.Count(img => (img.Status == ImageStatuses.Moved));
            //Выводим инфу об результатах
            return $"{_currentImageId + 1} / {completed} / {_images.Count}";
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
                foldersList = foldersWindow.GetFoldersToWork(foldersList);
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
        public void CheckImageMoveTarget(Key key)
        {
            //Получаем целевую папку, по нажатой кнопке
            TargetFolderInfo target = GetMoveFolder(key);
            //Если с этой кнопкой проассациирована папка
            if (target != null)
            {
                //Получаем текущую выбранную картинку
                ImageInfo image = GetCurrentImageInfo();
                //Переносим изображение
                _mover.MoveFile(target, image);
                //Вызываем ивент по завершеию переноса изображения
                GlobalEvents.InvokeMoveImageComplete();
            }
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
        public ImageInfo GetCurrentImageInfo() =>
            _images[_currentImageId];

        /// <summary>
        /// Переходим к указанной картинке
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns>Инфомрация о картинке</returns>
        public ImageInfo MoveToImage(int direction)
        {
            ImageInfo ex = null;
            //Если картинки вообще есть
            if (_images.Count > 0)
            {
                //Переходим по направлению
                _currentImageId += direction;
                //Засовываем значение идентификатора
                //обратно в рамки если оно зха них вышло
                if (_currentImageId >= _images.Count)
                    _currentImageId = _images.Count - 1;
                if (_currentImageId < 0)
                    _currentImageId = 0;
                //Возвращаем картинку
                ex = _images[_currentImageId];
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
        public void StartScan(string imagesPath, string foldersPath)
        {
            //Запускаем эту работу в отдельном потоке
            new Thread(() => {
                //Запоминаем имя папки с картинками, добавляя слеш в конец при необходимости
                _foldersPath = (foldersPath.Last() != '\\') ? $"{foldersPath}\\" : foldersPath;
                //Сбрасываем id выбранной картинки
                _currentImageId = 0;
                //Сканим картинки
                _images = _scanner.ScanImages(imagesPath);
                //Сканим папки
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
                    string path = $"{_foldersPath}{newFolderName}";
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
