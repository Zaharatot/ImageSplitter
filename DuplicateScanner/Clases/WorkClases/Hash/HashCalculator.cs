using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using DuplicateScannerLib.Clases.DataClases;
using DuplicateScannerLib.Clases.DataClases.File;
using DuplicateScannerLib.Clases.DataClases.Image;
using DuplicateScannerLib.Clases.WorkClases.Image;
using LiningLibZ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DCTHashZ.Clases.DataClases.Global.Enums;
using static DuplicateScannerLib.Clases.DataClases.Global.Enums;

namespace DuplicateScannerLib.Clases.WorkClases.Hash
{
    /// <summary>
    /// Класс выполнения вычисления хеша
    /// </summary>
    internal class HashCalculator : IDisposable
    {
        /// <summary>
        /// Константа размера области для лайнинга
        /// </summary>
        private const int LINING_AREA_SIZE = 10;
        /// <summary>
        /// Константа коэффициента лайнинга
        /// </summary>
        private const double LINING_COEFF = 0.7;


        /// <summary>
        /// Класс вычисления ДКП хеша
        /// </summary>
        private DCTHash _dctHash;
        /// <summary>
        /// Класс лайнинга изображений
        /// </summary>
        private LiningLibZFacade _liningLib;
        /// <summary>
        /// Класс загрузеи изображения
        /// </summary>
        private LoadImagePixels _loadImage;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public HashCalculator()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _liningLib = new LiningLibZFacade();
            _dctHash = new DCTHash();
            _loadImage = new LoadImagePixels();
        }


        /// <summary>
        /// Метод получения лайнового хеша
        /// </summary>
        /// <param name="image">Класс информации об изображении</param>
        /// <returns>Таска получения лайнового хеша изображения</returns>
        private Task<CreateHashTask> GetLinedHash(ByteImageInfo image)
        {
            //Выполняем лайнинг изображения, получая массив байт 
            byte[] linedPixels = _liningLib.LineImage(image.Pixels, image.ImageSize.Width,
                image.ImageSize.Height, LINING_AREA_SIZE, LINING_COEFF);
            //Инициализируем класс информации о лейновом изображении
            ByteImageInfo linedImage = new ByteImageInfo(image.ImageSize, linedPixels);
            //Получаем таску рассчёта хеша
            return _dctHash.AddTaskAsync(linedImage);
        }

        /// <summary>
        /// Метод получения хеша
        /// </summary>
        /// <param name="image">Класс информации об изображении</param>
        /// <returns>Таска получения хеша изображения</returns>
        private Task<CreateHashTask> GetHash(ByteImageInfo image) =>
            //Получаем таску рассчёта хеша, передав в него копию класса картинки
            _dctHash.AddTaskAsync(new ByteImageInfo(image));

        /// <summary>
        /// Метод получения хеша из таски
        /// </summary>
        /// <param name="task">Таска для получения хеша</param>
        /// <returns>Хеш или Null</returns>
        private ulong? GetHashFromTask(Task<CreateHashTask> task) =>
            //Если таска завершена и получен корректный результат
            (task.IsCompleted && task.Result.Status == CreateHashStatuses.Complete) 
                //Возвращаем значение хеша
                ? task.Result.Hash 
                //В противном случае - Null
                : null;

        /// <summary>
        /// Метод рассчёта хешей для изображения
        /// </summary>
        /// <param name="linedDcpHash">Дкп-хеш лайнового изображения</param>
        /// <param name="dcpHash">Дкп-хеш изображения</param>
        /// <param name="image">Изображение для получения хеша</param>
        private void ClaculateHashes(ByteImageInfo image, out ulong? dcpHash, out ulong? linedDcpHash)
        {
            try
            {
                //Инициализируем список тасок получения хешей
                Task<CreateHashTask>[] tasks = new Task<CreateHashTask>[] {
                     //Получаем хеш изображения
                     GetHash(image),
                     //Получаем лайновый хеш изображения
                     GetLinedHash(image)
                 };
                 //Ждём завершения выполнения этих задач
                 Task.WaitAll(tasks, 10000);
                 //Получаем значения хешей из тасок
                 dcpHash = GetHashFromTask(tasks[0]);
                 linedDcpHash = GetHashFromTask(tasks[1]);
            }
            //В случае ошибок
            catch 
            { 
                //Возвращаем пустые хеши
                dcpHash = linedDcpHash = null;
            }
        }

        /// <summary>
        /// Выполняем рассчёт хешей для изображения
        /// </summary>
        /// <param name="info">Класс информации о дубликате</param>
        public void CalculateHash(DuplicateInfo info)
        {
            //В районе 0.5 секунды время итерации - это вполне норм,
            //учитывая сохранение уже обработанных изображений.

            //Выполняем загрузку пикселей изображения
            ByteImageInfo image = _loadImage.LoadImage(info.Path);
            //Если картинка была успешно загружена
            if (image != null)
            {
                //Проставляем значения размеров картинки
                info.Width = image.OriginalSize.Width;
                info.Height = image.OriginalSize.Height;
                //Выполняем рассчёт хешей для изображения
                ClaculateHashes(image, out ulong? dcpHash, out ulong? linedDcpHash);
                //Проставляем полученные хеши в изображение
                info.DcpHash = dcpHash;
                info.LinedDcpHash = linedDcpHash;
                //Проставляем статус по наличию хешей изображения
                info.State = (dcpHash == null && linedDcpHash == null) 
                    ? DuplicateStates.CalculateHashesError : DuplicateStates.Calculated;
            }
            //В противном случае
            else
                //Ставим код ошибки
                info.State = DuplicateStates.ImageLoadError;
        }

        /// <summary>
        /// Метод очистки неуправляемых ресурсов класса
        /// </summary>
        public void Dispose()
        {
            //Завершаем работу с классом вычисления ДКП-хеша
            _dctHash?.Dispose();
        }
    }
}
