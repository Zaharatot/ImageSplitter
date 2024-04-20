using FilesSplitWindowLib.Content.Clases.DataClases;
using MessagesWindowLib;
using SplitterDataLib.DataClases.Files;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static MessagesWindowLib.Content.Clases.DataClases.Enums;

namespace FilesSplitWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс возврата файлов
    /// </summary>
    internal class FilesReturn
    {

        /// <summary>
        /// Класс поиска имени файла
        /// </summary>
        private ElementNameChecker _elementNameChecker;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FilesReturn()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем класс поиска имени файла
            _elementNameChecker = new ElementNameChecker();
        }


        /// <summary>
        /// Переносим контент из дочерней папки в родительскую
        /// </summary>
        /// <param name="parentPath">Путь к родительской папке</param>
        /// <param name="child">Информация о дочерней папке</param>
        private void MoveChild(string parentPath, DirectoryInfo child)
        {
            string newName;
            //Проходимся по файлам дочерней папки
            foreach (FileInfo file in child.GetFiles())
            {
                //Получаем новое имя файла
                newName = _elementNameChecker.GetNewElementName(parentPath, file.Name, false);
                //Переносим их в родительскую
                file.MoveTo($"{parentPath}{newName}");
            }
            //Удаляем родительскую папку
            child.Delete();
        }




        /// <summary>
        /// Возврат в родительскую папку контента из всех дочерних
        /// </summary>
        /// <param name="info">Информация о перемещаемых файлах</param>
        public void StartReturn(MoveFilesInfo info) =>
            //Делаем всё это в отдельном потоке
            new Thread(() => {
                //Проходимся по всем папкам корневой
                foreach (DirectoryInfo dir in info.Parent.GetDirectories())
                    //Переносим их дочерние файлы в корневую
                    MoveChild(info.Parent.FullName, dir);
                //Выводим сообщение об успешном завершении операции
                MessagesBoxFasade.ShowMessageBoxDone(MessageBoxMessages.FileReturnComplete);
            }).Start();

    }
}
