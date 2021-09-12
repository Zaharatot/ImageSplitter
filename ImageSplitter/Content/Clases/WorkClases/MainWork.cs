using ImageSplitter.Content.Clases.DataClases;
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
using static ImageSplitter.Content.Clases.DataClases.Delegates;
using static ImageSplitter.Content.Clases.DataClases.Enums;

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
        private DuplicateScan _duplicateScan;

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
            _duplicateScan = new DuplicateScan();
        }



        /// <summary>
        /// Проверяем нажатую кнопку на тип кнопки переноса
        /// </summary>
        /// <param name="key">Код нажатой кнопки</param>
        public void CheckImageMoveTarget(Key key) =>
            //Вызываем внутренний метод
            _splitImages.CheckImageMoveTarget(key);

        /// <summary>
        /// Запускаем сканирование
        /// </summary>
        /// <param name="imagesPath">Путь к папкам с картинками</param>
        /// <param name="foldersPath">Путь к целевым папкам</param>
        public void StartScan(string imagesPath, string foldersPath) =>
            //Вызываем внутренний метод
            _splitImages.StartScan(imagesPath, foldersPath);

        /// <summary>
        /// Переходим к указанной картинке
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns>Инфомрация о картинке</returns>
        public ImageInfo MoveToImage(int direction) =>
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
        public ImageInfo GetCurrentImageInfo() =>
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
        /// <param name="path">Путь к папке сканирования</param>
        public void StartDuplicateScan(string path) =>
            //Вызываем внутренний метод
            _duplicateScan.StartDuplicateScan(path);

        /// <summary>
        /// Метод запуска удаления дублиткатов
        /// </summary>
        /// <param name="duplicates">Список дубликатов для удаления</param>
        public void RemoveDuplicates(List<DuplicateImageInfo> duplicates) =>
            //Вызываем внутренний метод
            _duplicateScan.RemoveDuplicates(duplicates);

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
