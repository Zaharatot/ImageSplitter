using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ImageSplitter.Content.Clases.WorkClases.Processors
{
    /// <summary>
    /// Класс сплита файлов
    /// </summary>
    internal class FileSplitter
    {

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileSplitter()
        {

        }

        /// <summary>
        /// Сплитим файлы в директории
        /// </summary>
        /// <param name="countFiles">Количество файлов для сплита</param>
        /// <param name="directory">Информация о директории</param>
        private void Split(DirectoryInfo directory, int countFiles)
        { 
            //Если папка есть и количество файлов для сплита адекватное
            if (directory.Exists && (countFiles > 0))
                //Сплитим файлы
                SplitFolderContent(directory, countFiles);
        }


        /// <summary>
        /// Сплит файлов в папке
        /// </summary>
        /// <param name="countFiles">Количество файлов для сплита</param>
        /// <param name="di">Информация о сплитуемой папке</param>
        private void SplitFolderContent(DirectoryInfo di, int countFiles)
        {
            //Получаем все файлы в папке
            FileInfo[] files = di.GetFiles();
            int counter;
            int id = 0;
            int folderCounter = -1;
            //Путь загрузки
            string path;
            string name;
            //Если файлы в папке есть
            if (files.Length > 0)
            {
                do
                {
                    //Обновляем папку для перемещения
                    folderCounter++;
                    path = $"{di.FullName}\\{folderCounter}\\";
                    Directory.CreateDirectory(path);
                    //Перемещаем {countFiles} файлов
                    counter = 0;
                    do
                    {
                        //Если файл не скрытый системный или онли для чтения
                        if (!(files[id].Attributes.HasFlag(FileAttributes.Hidden) 
                            || files[id].Attributes.HasFlag(FileAttributes.System)
                            || files[id].Attributes.HasFlag(FileAttributes.ReadOnly)))
                        {
                            //Получаем имя файла
                            name = files[id].Name;
                            //Если имя файла больше 150 символов
                            if (name.Length > 150)
                                //Обрезаем его
                                name = name.Substring(0, 150);
                            files[id].MoveTo(path + name);
                        }
                        counter++;
                        id++;
                    } while ((id < files.Length) && (counter < countFiles));
                } while (id < files.Length);
            }
        }

        /// <summary>
        /// Переносим контент из дочерней папки в родительскую
        /// </summary>
        /// <param name="parentPath">Путь к родительской папке</param>
        /// <param name="child">Информация о дочерней папке</param>
        private void MoveChild(string parentPath, DirectoryInfo child)
        {
            //Проходимся по файлам дочерней папки
            foreach (FileInfo file in child.GetFiles())
                //Переносим их в родительскую
                file.MoveTo($"{parentPath}{file.Name}");
            //Удаляем родительскую папку
            child.Delete();
        }


        /// <summary>
        /// Возврат в родительскую папку контента из всех дочерних
        /// </summary>
        /// <param name="path">Путь к родительской папке</param>
        public void BackToParent(string path)
        {
            //Делаем всё это в отдельном потоке
            new Thread(() => {
                //Если он не оканчивается на слеш
                if (path.Last() != '\\')
                    //доабвляем его
                    path += "\\";
                //Получаем инфу о текущей директории
                DirectoryInfo root = new DirectoryInfo(path);
                //Проходимся по всем папкам корневой
                foreach (DirectoryInfo dir in root.GetDirectories())
                    //Переносим их дочерние файлы в корневую
                    MoveChild(path, dir);

                //Выводим сообщение о завершении операции
                MessageBox.Show("Back move complete!");
            }).Start();
        }

        /// <summary>
        /// Запуск сплита 
        /// </summary>
        /// <param name="countFiles">Количество файлов для сплита</param>
        /// <param name="path">Путь для сплита</param>
        /// <param name="isChildSplit">Флаг сплита в дочерних папках</param>
        public void StartSplit(string path, int countFiles, bool isChildSplit)
        {
            //Делаем всё это в отдельном потоке
            new Thread(() =>
            {
                //Получаем инфу о текущей директории
                DirectoryInfo root = new DirectoryInfo(path);
                //Если сплитим дочерние
                if (isChildSplit)
                {
                    //ПОлучаем дочерние папки
                    DirectoryInfo[] directories = root.GetDirectories();
                    //Проходимся по папкам
                    foreach (var directory in directories)
                        //Сплитим их
                        Split(directory, countFiles);
                }
                //Если сплитим только текущую
                else
                    Split(root, countFiles);


                MessageBox.Show("Split complete!");
            }).Start();
        }
    }
}
