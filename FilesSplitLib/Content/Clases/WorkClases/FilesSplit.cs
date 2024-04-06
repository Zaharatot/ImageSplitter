using FilesSplitWindowLib.Content.Clases.DataClases;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FilesSplitWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс сплита файлов
    /// </summary>
    internal class FilesSplit
    {

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FilesSplit()
        {

        }

        /// <summary>
        /// Метод проверки аттрибутов файла на запрет работы с ним
        /// </summary>
        /// <param name="attributes">Список аттрибутов файла</param>
        /// <returns>True - файл запрещён к работе</returns>
        private bool IsForbiddenFile(FileAttributes attributes) =>
            attributes.HasFlag(FileAttributes.Hidden) ||
            attributes.HasFlag(FileAttributes.System) ||
            attributes.HasFlag(FileAttributes.ReadOnly);

        /// <summary>
        /// Метод перемещения файла
        /// </summary>
        /// <param name="file">Информация о файле</param>
        /// <param name="path">Путь для перемещения</param>
        private void MoveFile(FileInfo file, string path)
        {
            string name;
            //Если файл не скрытый системный или онли для чтения
            if (!IsForbiddenFile(file.Attributes))
            {
                //Получаем имя файла
                name = file.Name;
                //Если имя файла больше 150 символов
                if (name.Length > 150)
                    //Обрезаем его
                    name = name.Substring(0, 150);
                //Перемещаем файл
                file.MoveTo(path + name);
            }
        }

        /// <summary>
        /// Метод получения файлов для перемщения
        /// </summary>
        /// <param name="parent">Родительская папка для перемещения</param>
        /// <returns>Список файлов для перемещения</returns>
        private List<FileInfo> GetFilesToMove(DirectoryInfo parent) =>
            //Из родительской папки
            parent
                //Получаем файлы
                .GetFiles()
                //Отсекаем из их списка запрещённые к обработке
                .Where(file => !IsForbiddenFile(file.Attributes))
                //Возвращаем в виде списка
                .ToList();

        /// <summary>
        /// Метод получения количества папок
        /// </summary>
        /// <param name="countFiles">Общее количество файлов</param>
        /// <param name="countFilesInFolder">Количество файлов в папке</param>
        /// <returns>Количество папок для раскладки</returns>
        private int GetCountFolders(int countFiles, int countFilesInFolder)
        {
            //Получаем количество папок без учёта остатка
            int count = countFiles / countFilesInFolder;
            //Если файлов меньше необходимого, или файлы делятся нацело
            if ((count == 0) || (countFiles % countFilesInFolder == 0))
                //Возвращаем текущее количество
                return count;
            //Во всех остальных случаях - будет дополнительная папка
            return count + 1;
        }

        /// <summary>
        /// Метод перемещения файлов в папку
        /// </summary>
        /// <param name="files">Список файлов для перемещения</param>
        /// <param name="info">Информация о перемещаемых файлах</param>
        private void MoveFilesToFolder(List<FileInfo> files, MoveFilesInfo info)
        {
            //Получаем количество файлов для обработки
            int countFiles = Math.Min(files.Count, info.CountFilesInFolder);
            //Создаём директорию в случае, если её не существовало
            Directory.CreateDirectory(info.CurrentParentPath);
            //Проходимся по файлам для обработки
            for(int i = 0; i < countFiles; i++)
            {
                //Выполняем перемещение файла
                MoveFile(files[0], info.CurrentParentPath);
                //Удаляем файл из списка на перенос
                files.RemoveAt(0);
            }
        }

        /// <summary>
        /// Сплит файлов в папке
        /// </summary>
        /// <param name="info">Информация о перемещаемых файлах</param>
        private void SplitFolderContent(MoveFilesInfo info)
        {
            //Если данные для сплита корректны
            if (info.IsCorrectData)
            {
                //Получаем файлы для обработки
                List<FileInfo> files = GetFilesToMove(info.Parent);
                //Получаем количество папок для обработки
                int countFolders = GetCountFolders(files.Count, info.CountFilesInFolder);
                //Проходимся по папкам
                for (int i = 0; i < countFolders; i++)
                {
                    //Выполняем перемещение файлов в папку
                    MoveFilesToFolder(files, info);
                    //Переходим к следующей папке
                    info.MoveToNextFolder();
                }
            }
        }

        /// <summary>
        /// Метод запуска сплита для дочерних
        /// </summary>
        /// <param name="info">Информация о перемещаемых файлах</param>
        private void SplitChilds(MoveFilesInfo info)
        {
            //Проходимся по дочерним папкам от целевой
            foreach (DirectoryInfo child in info.Parent.GetDirectories())
            {
                //Обновляем родительскую папку
                info.Parent = child;
                //Выполняем сплит для контента папки
                SplitFolderContent(info);
            }
        }




        /// <summary>
        /// Запуск сплита 
        /// </summary>
        /// <param name="info">Информация о перемещаемых файлах</param>
        public void StartSplit(MoveFilesInfo info) =>
            //Делаем всё это в отдельном потоке
            new Thread(() => {
                //Если сплитим дочерние
                if (info.IsChildSplit)
                    //Вызываем обработку дочерних
                    SplitChilds(info);
                //Если сплитим только текущую
                else
                    //Выполняем сплит для контента папки
                    SplitFolderContent(info);

                //TODO: Заменить на нормальное сообщение
                MessageBox.Show("Split complete!");
            }).Start();
    }
}
