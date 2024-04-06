using DuplicateScannerLib.Clases.DataClases.Result;
using DuplicateScannerLib.Clases.WorkClases.Hash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static DuplicateScannerLib.Clases.DataClases.Global.Enums;

namespace DuplicateScannerLib.Clases.DataClases.File
{
    /// <summary>
    /// Класс информации о дубликате
    /// </summary>
    [Serializable]
    public class DuplicateInfo
    {      
        
        
        /// <summary>
        /// Хеш пути к изображению
        /// </summary>
        public uint PathHash { get; set; }
        /// <summary>
        /// ДКП-хеш изображения
        /// </summary>
        public ulong? DcpHash { get; set; }
        /// <summary>
        /// ДКП-хеш лайновой версии изображения
        /// </summary>
        public ulong? LinedDcpHash { get; set; }
        /// <summary>
        /// Путь к родительской папке
        /// </summary>
        public string ParentPath { get; set; }
        /// <summary>
        /// Название родительской папки
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// Имя файла изображения
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ширина файла
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Высота файла
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Статус обработки изображения
        /// </summary>
        public DuplicateStates State { get; set; }

        /// <summary>
        /// Список хешей, которые не являются дубликатами
        /// </summary>
        public List<uint> ForbiddenHashes { get; set; }

        /// <summary>
        /// Строка полного пути к изображению
        /// </summary>
        [XmlIgnore]
        public string Path => ParentPath + Name;
        /// <summary>
        /// Разрешение файла
        /// </summary>
        [XmlIgnore]
        public double Resolution => Width * Height;
        /// <summary>
        /// Флаг возможности обработки изображения
        /// </summary>
        [XmlIgnore]
        public bool IsAllowProcess => State == DuplicateStates.Calculated;
        /// <summary>
        /// Флаг ошибки при обработке файла
        /// </summary>
        [XmlIgnore]
        public bool IsErrorFile => 
            //Если у нас ошибка рассчёта хешей
            (State == DuplicateStates.CalculateHashesError) || 
            //ОШибка загрузки изображения
            (State == DuplicateStates.ImageLoadError) ||
            //Или один из хешей пустой (стейт "CalculateHashesError"
            //указывает на то, что оба хеша с ошибками)
            ((DcpHash == null) || (LinedDcpHash == null));

        /// <summary>
        /// Класс генерации CRC32 хешей
        /// </summary>
        [XmlIgnore]
        private readonly Crc32 _crc;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicateInfo()
        {
            //Проставляем дефолтные значения
            ForbiddenHashes = new List<uint>();
            PathHash = 0;
            Width = Height = 0;
            DcpHash = LinedDcpHash = 0;
            ParentPath = Name = ParentName = "";
            State = DuplicateStates.Added;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="file">Класс информации о файле</param>
        public DuplicateInfo(FileInfo file)
        {
            //Получаем экземпляр класса генерации хешей
            _crc = Crc32.GetInstance();
            //Проставляем переданные значения
            ParentPath = file.DirectoryName + "\\"; 
            Name = file.Name;
            ParentName = file.Directory.Name;
            PathHash = _crc.ComputeChecksum(file.FullName);
            //Проставляем дефолтные значения
            ForbiddenHashes = new List<uint>();
            DcpHash = LinedDcpHash = 0;
            State = DuplicateStates.Added;
        }



        /// <summary>
        /// Метод проверки возможности добавления хеша в запрещённые
        /// </summary>
        /// <param name="hash">Хеш для проверки</param>
        /// <returns>True - можно добавлять</returns>
        public bool IsAllowAddToForbidden(uint hash) =>
            //Хеш не совпадает с хешем текущего элемента
            (PathHash != hash) &&
            //Хеш отсутствует в списке запрещённых
            !ForbiddenHashes.Contains(hash);

        /// <summary>
        /// Метод удаления указанного хеша из списка запрещённых
        /// </summary>
        /// <param name="hash">Хеш для удаления</param>
        public void RemoveForbiddenHash(uint hash) =>
            //Удаляем из списка запрещённых хешей указанный
            ForbiddenHashes.Remove(hash);

        /// <summary>
        /// Метод получения класса результата
        /// </summary>
        /// <returns>Класс результата поиска</returns>
        public DuplicateResult GetResult() =>
            new DuplicateResult() { 
                Height = Height, 
                Width = Width,
                Name = Name,
                Path = Path,
                PathHash = PathHash,
                ParentName = ParentName,
                ParentPath = ParentPath
            };

        /// <summary>
        /// Метод сравнения элементов
        /// </summary>
        /// <param name="elem">Элемент для сравнения</param>
        /// <returns>True - элементы равны</returns>
        public override bool Equals(object elem) =>
            //Если переданный элемент имеет нужный тип
            (elem is DuplicateInfo duplicate) 
                //Сравниваем по хешу, и игнорируем пустой хеш
                ? (duplicate.PathHash == PathHash) && (PathHash != 0)
                //В противном случае они точно не равны
                : false;

        /// <summary>
        /// Метод получения хеша элемента
        /// </summary>
        /// <returns>Хеш пути к файлу</returns>
        public override int GetHashCode() => (int)PathHash;
    }
}
