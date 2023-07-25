using DuplicateScanner.Clases.DataClases.File;
using DuplicateScanner.Clases.DataClases.Properties;
using DuplicateScanner.Clases.WorkClases.Finder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DuplicateScanner.Clases.DataClases.Global.Delegates;
using static DuplicateScanner.Clases.DataClases.Global.Enums;

namespace DuplicateScanner.Clases.WorkClases.Finder
{
    /// <summary>
    /// Класс выполнения сравнения хешей
    /// </summary>
    internal class HashComparer
    {
        /// <summary>
        /// Делегат метода сравнения файлов
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы являются схожими</returns>
        internal delegate bool IsFileEqualDelegate(DuplicateInfo current, DuplicateInfo toCheck);



        /// <summary>
        /// Класс сравнения хешей
        /// </summary>
        private EqualDctHash _equalHash;
        /// <summary>
        /// Словарь методов сравнения файлов
        /// </summary>
        private Dictionary<ScanTypes, IsFileEqualDelegate> _equalsDict;

        /// <summary>
        /// Текущий выбранный метод проверки
        /// </summary>
        private IsFileEqualDelegate _currentMethod;
        /// <summary>
        /// Точность поиска
        /// </summary>
        public int ScanAccuracy { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public HashComparer()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _equalHash = new EqualDctHash();
            //ЗАполняем словарь методов сравнения
            _equalsDict = CreateEqualsDict();
            //ВЫбираем целевой метод проверки
            _currentMethod = _equalsDict[ScanTypes.Both];
            //Сохраняем значение точности сканирования
            ScanAccuracy = 9;
        }


        /// <summary>
        /// Метод инициализиации словаря методов сравнения
        /// </summary>
        /// <returns>Словарь методов сравнения</returns>
        private Dictionary<ScanTypes, IsFileEqualDelegate> CreateEqualsDict() =>
            new Dictionary<ScanTypes, IsFileEqualDelegate>() {
                { ScanTypes.DcpScan, IsFileEquals },
                { ScanTypes.LinedDcpScan, IsFileEqualsLined },
                { ScanTypes.Both, IsFileEqualsBoth },
            };

        /// <summary>
        /// Выполняем проверку на схожесть файлов
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы являются схожими</returns>
        private bool IsFileEqualsBoth(DuplicateInfo current, DuplicateInfo toCheck) =>
            //Сравниваем обычные хеши
            IsFileEquals(current, toCheck) ||
            //И лайновые с обычным
            IsFileEqualsLined(current, toCheck);

        /// <summary>
        /// Выполняем проверку на схожесть файлов
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы являются схожими</returns>
        private bool IsFileEqualsLined(DuplicateInfo current, DuplicateInfo toCheck) =>
            //Сравниваем ДКП-хеш оригинала с лайновым ДКП-хешем сравниваемого
            _equalHash.EqalHash(current.DcpHash, toCheck.LinedDcpHash, ScanAccuracy) ||
            //Сравниваем лайновый ДКП-хеш оригинала с ДКП-хешем сравниваемого
            _equalHash.EqalHash(current.LinedDcpHash, toCheck.DcpHash, ScanAccuracy);

        /// <summary>
        /// Выполняем проверку на схожесть файлов
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы являются схожими</returns>
        private bool IsFileEquals(DuplicateInfo current, DuplicateInfo toCheck) =>
            //Сравниваем ДКП хеши изображений
            _equalHash.EqalHash(current.DcpHash, toCheck.DcpHash, ScanAccuracy);


        /// <summary>
        /// Проверка на запрет сравнения хешей
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы не записаны как точные не дубликаты</returns>
        private bool IsDenyCheck(DuplicateInfo current, DuplicateInfo toCheck) =>
            //Сравниваемый находится в списке запрещённых у целевого
            current.ForbiddenHashes.Contains(toCheck.PathHash) ||
            //Или целевой находится в запрещённом списке у сравниваемого
            toCheck.ForbiddenHashes.Contains(current.PathHash);

        /// <summary>
        /// Метод проверки возможности сравнения изображений
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы можно сравнивать</returns>
        private bool IsAllowCheckDuplicates(DuplicateInfo current, DuplicateInfo toCheck) =>
            //Если сравниваемую картинку можно обрабатывать
            toCheck.IsAllowProcess &&
            //Если файлы не являются одним и тем же файлом
            //Проверка идёт по ссылке на класс, т.е. всё ок
            (current != toCheck) &&
            //Если нет запрета на сравнение 
            !IsDenyCheck(current, toCheck);



        /// <summary>
        /// Метод смены параметров проверки
        /// </summary>
        /// <param name="properties">Параметры сканирования</param>
        public void ChangeCheckProperties(ScanProperties properties)
        {
            //ВЫбираем целевой метод проверки
            _currentMethod = _equalsDict[properties.ScanType];
            //Сохраняем значение точности сканирования
            ScanAccuracy = properties.ScanAccuracy;
        }

        /// <summary>
        /// Выполняем проверку на дубликат двух файлов
        /// </summary>
        /// <param name="current">Текущий выбранный файл</param>
        /// <param name="toCheck">Файл для сравнения</param>
        /// <returns>True - файлы являются дубликатами</returns>
        public bool IsDuplicate(DuplicateInfo current, DuplicateInfo toCheck)
        {
            //Если файлы можно сравнивать
            if (IsAllowCheckDuplicates(current, toCheck))
                //Вызываем его и возвращаем результат проверки
                return _currentMethod.Invoke(current, toCheck);
            //Во всех остальных случаях - файлы не равны
            return false;
        }
    }
}
