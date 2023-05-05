using DuplicateScanner;
using DuplicateScanner.Clases.DataClases.File;
using DuplicateScanner.Clases.DataClases.Properties;
using ImageSplitter.Content.Clases.DataClases;
using ImageSplitter.Content.Clases.DataClases.Split;
using ImageSplitter.Content.Clases.WorkClases.Processors;
using ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit;
using ImageSplitter.Content.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.WorkClases
{
    /// <summary>
    /// Основной рабочий класс
    /// </summary>
    internal class MainWork : IDisposable
    {

        /// <summary>
        /// Класс сплита изображений
        /// </summary>
        private SplitImages _splitImages;
        /// <summary>
        /// Класс сплита файлов
        /// </summary>
        private FileSplitter _fileSplitter;
        /// <summary>
        /// Класс переименованяи файлов
        /// </summary>
        private FileRenamer _renameFiles;
        /// <summary>
        /// Класс поиска дубликатов
        /// </summary>
        private DuplicateScannerFasade _duplicateScan;



        /// <summary>
        /// Конструктор класса
        /// </summary>
        public MainWork()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем класс сплита изображений
            _splitImages = new SplitImages();
            //Инициализируем класс сплиттера
            _fileSplitter = new FileSplitter();
            //Инициализируем класс переименования
            _renameFiles = new FileRenamer();
            //Инициализируем класс поиска дубликатов
            _duplicateScan = new DuplicateScannerFasade();
        }



        /// <summary>
        /// Проверяем нажатую кнопку на тип кнопки переноса
        /// </summary>
        /// <param name="key">Код нажатой кнопки</param>
        /// <returns>True - нажатие было обработано</returns>
        public bool CheckImageMoveTarget(Key key) =>
            //Вызываем внутренний метод
            _splitImages.CheckImageMoveTarget(key);

        /// <summary>
        /// Запускаем сканирование
        /// </summary>
        /// <param name="imagesPath">Путь к папкам с картинками</param>
        /// <param name="foldersPath">Путь к целевым папкам</param>
        /// <param name="isFolder">Флаг поиска папок</param>
        public void StartScan(string imagesPath, string foldersPath, bool isFolder) =>
            //Вызываем внутренний метод
            _splitImages.StartScan(imagesPath, foldersPath, isFolder);

        /// <summary>
        /// Переходим к указанной картинке
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns>Инфомрация о картинке</returns>
        public CollectionInfo MoveToImage(int direction) =>
            //Вызываем внутренний метод
            _splitImages.MoveToImage(direction);

        /// <summary>
        /// Получаем папку для перемещения по нажатой кнопке
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>Папка для перемещеия</returns>
        public TargetFolderInfo GetMoveFolder(Key key) =>
            //Вызываем внутренний метод
            _splitImages.GetMoveFolder(key);

        /// <summary>
        /// Возвращаем текущую выбранную картинку
        /// </summary>
        /// <returns>ИНформация о выбранной картинке</returns>
        public CollectionInfo GetCurrentImageInfo() =>
            //Вызываем внутренний метод
            _splitImages.GetCurrentImageInfo();

        /// <summary>
        /// Метод добавления новой папки
        /// </summary>
        public void AddNewFolder() =>
            //Вызываем внутренний метод
            _splitImages.AddNewFolder();

        /// <summary>
        /// Метод удаления папки из списка
        /// </summary>
        /// <param name="key">Клавиша, к которой привязана папка</param>
        public void RemoveFolderFromList(Key key) =>
            //Вызываем внутренний метод
            _splitImages.RemoveFolderFromList(key);

        /// <summary>
        /// Запуск сплита 
        /// </summary>
        /// <param name="countFiles">Количество файлов для сплита</param>
        /// <param name="path">Путь для сплита</param>
        /// <param name="isChildSplit">Флаг сплита в дочерних папках</param>
        public void StartSplit(string path, int countFiles, bool isChildSplit) =>
            //Вызываем внутренний метод
            _fileSplitter.StartSplit(path, countFiles, isChildSplit);

        /// <summary>
        /// Возврат в родительскую папку контента из всех дочерних
        /// </summary>
        /// <param name="path">Путь к родительской папке</param>
        public void BackToParent(string path) =>
            //Вызываем внутренний метод
            _fileSplitter.BackToParent(path);

        /// <summary>
        /// Выполняем переименование файлов
        /// </summary>
        /// <param name="mask">Маска имени для переименования</param>
        /// <param name="path">Путь для переименования</param>
        public void RenameFiles(string path, string mask) =>
            //Вызываем внутренний метод
            _renameFiles.RenameFiles(path, mask);


        /// <summary>
        /// Запуск сканирования дубликатов
        /// </summary>
        /// <param name="properties">Параметры сканирования</param>
        public void StartDuplicateScan(ScanProperties properties) =>
            //Вызываем внутренний метод
            _duplicateScan.StartDuplicateScan(properties);

        /// <summary>
        /// Метод запуска удаления дублиткатов
        /// </summary>
        /// <param name="groups">Список запрещённых групп</param>
        /// <param name="toRemove">Группа хешей для удаления</param>
        public void RemoveDuplicates(HashesGroup toRemove, List<HashesGroup> groups) =>
            //Вызываем внутренний метод
            _duplicateScan.RemoveDuplicates(toRemove, groups);


        /// <summary>
        /// Метод удаления старых дубликатов из списка
        /// </summary>
        public void RemoveOldDuplicates() =>
            //Вызываем внутренний метод
            _duplicateScan.RemoveOldDuplicates();

        /// <summary>
        /// Метод очистки неуправляемых ресурсов класса
        /// </summary>
        public void Dispose()
        {
            //Завершаем работу с классом поиска дубликатов
            _duplicateScan?.Dispose();
        }
    }
}
