using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows;
using SplitterDataLib.DataClases.Global.Split;
using MessagesWindowLib;
using static MessagesWindowLib.Content.Clases.DataClases.Enums;

namespace FilesRenameWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс переименования файлов
    /// </summary>
    internal class FileRenamer
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileRenamer()
        {

        }

        /// <summary>
        /// Формируем новое имя файла
        /// </summary>
        /// <param name="mask">Маска имени файла</param>
        /// <param name="id">Id файла</param>
        /// <param name="ext">Расширение файла</param>
        /// <returns>Строка нового имени файла</returns>
        private string GetNewFileName(string mask, string ext, ref int id)
        {
            //Имя файла
            string fileName;

            do
                //Получаем имя файла с итератором, и увеличиваем итератор
                fileName = string.Format(mask, id++) + ext;
            //Повторяем процесс до тех пор, пока не получим не
            //занятое имя файла. Это нужно для случая, если в
            //папке уже есть файлы подходящие под маску имени
            while (File.Exists(fileName));
            //Возвращаем имя файла
            return fileName;
        }

        /// <summary>
        /// Выполняем переименовывание файлов
        /// </summary>
        /// <param name="root">ИНформация о корневой папке</param>
        /// <param name="mask">Маска переименования</param>
        private void RenameFiles(DirectoryInfo root, string mask)
        {
            string newName;
            //Инициализируем идентификатор файла
            int id = 0;
            //Проходимся по дочерним файлам
            foreach (FileInfo file in root.GetFiles())
            {
                //Получаем новое имя файла
                newName = GetNewFileName(mask, file.Extension, ref id);
                //Выполняем переименовывание файла
                file.MoveTo($"{root.FullName}{newName}");
            }
        }


        /// <summary>
        /// Выполняем переименование файлов
        /// </summary>
        /// <param name="mask">Маска имени для переименования</param>
        /// <param name="info">Информация о выбранных путях</param>
        public void RenameFiles(SplitPathsInfo info, string mask) =>
            //Делаем всё это в отдельном потоке
            new Thread(() => {
                //Выполняем рекурсивное переименовывание папок
                RenameFiles(new DirectoryInfo(info.ScanPath), mask);
                //Выводим сообщение об успешном завершении операции
                MessagesBoxFasade.ShowMessageBoxDone(MessageBoxMessages.FileRenameComplete);
            }).Start();
    }
}
