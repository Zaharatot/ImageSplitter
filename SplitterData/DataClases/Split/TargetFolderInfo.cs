using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SplitterDataLib.DataClases.Global.Split
{
    /// <summary>
    /// Информация о целевой папке
    /// </summary>
    public class TargetFolderInfo
    {
        /// <summary>
        /// Путь к этой папке
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Имя папки
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Целевая клавиша
        /// </summary>
        public Key TargetKey { get; set; }
        /// <summary>
        /// Флаг выбора папки
        /// </summary>
        public bool IsSelected { get; set; }



        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TargetFolderInfo()
        {
            //Проставляем дефолтные значения
            Path = Name = "";
            TargetKey = Key.Next;
            IsSelected = false;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="path">Путь к папке</param>
        /// <param name="name">Имя папки</param>
        /// <param name="targetKey">Ключ для папки</param>
        public TargetFolderInfo(string path, string name, Key targetKey)
        {
            //Проставляем переданные значения
            Path = path;
            Name = name;
            TargetKey = targetKey;
            //Проставляем дефолтное значение
            IsSelected = false;
        }
    }
}
