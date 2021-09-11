using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows;

namespace ImageSplitter.Content.Clases.WorkClases.Processors
{
    /// <summary>
    /// Класс переименования файлов
    /// </summary>
    internal class FileRenamer
    {
        /// <summary>
        /// Константа имени выходной папки
        /// </summary>
        private const string OUTPUT_DIR_NAME = "_TEST_OUTPUT_\\";

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
        private string GetNewFileName(string mask, int id, string ext) =>
            string.Format(mask, id) + ext;

        /// <summary>
        /// СОздаём выходную папку
        /// </summary>
        /// <param name="rootPath">Путь к корневой папке</param>
        /// <returns>Строка пути к выходной папке</returns>
        private string CreateOutputDir(string rootPath)
        {
            //Добавляем имя выходной папки к пути к корневой папке
            string ex = $"{rootPath}{OUTPUT_DIR_NAME}";
            //Создаём папку, если её ещё не было
            Directory.CreateDirectory(ex);
            //Возвращаем путь
            return ex;
        }

        /// <summary>
        /// Выполняем переименовывание файлов
        /// </summary>
        /// <param name="root">ИНформация о корневой папке</param>
        /// <param name="outputDirPath">Путь к выходной папке</param>
        /// <param name="mask">Маска переименования</param>
        private void RenameFiles(DirectoryInfo root, string outputDirPath, string mask)
        {
            string newName;
            //Идентификатор файла
            int id = 0;
            //Проходимся по дочерним файлам
            foreach (FileInfo file in root.GetFiles())
            {
                //ПОлучаем новое имя файла
                newName = GetNewFileName(mask, id++, file.Extension);
                //Выполняем переименовывание файла с перемещением в выходную папку
                file.MoveTo($"{outputDirPath}{newName}");
            }
        }


        /// <summary>
        /// Перемещаем файлы обратно в корневую папку
        /// </summary>
        /// <param name="rootPath">Путь к корневой папке</param>
        /// <param name="ourputPath">Путь к выходной папке</param>
        private void MoveFilesToRoot(string rootPath, string ourputPath)
        {
            //ИНициализируем инфу о выходной папке
            DirectoryInfo outputFolder = new DirectoryInfo(ourputPath);
            //Если он не оканчивается на слеш
            if (rootPath.Last() != '\\')
                //доабвляем его
                rootPath += "\\";
            //ПРоходимся по всем файлам этой папки
            foreach (FileInfo file in outputFolder.GetFiles())
                //Переносим файл в корневую папку
                file.MoveTo($"{rootPath}{file.Name}");
            //Удаляем выходную папку
            outputFolder.Delete();
        }

        /// <summary>
        /// Выполняем переименовывание папки
        /// </summary>
        /// <param name="dir">Информация о папке</param>
        /// <param name="mask">Маска имени для переименования</param>
        private void RenameFolder(DirectoryInfo dir, string mask)
        {   
            //ПОлучаем путь к выходной папке
            string outputDirPath = CreateOutputDir(dir.FullName);
            //Если он не оканчивается на слеш
            if (outputDirPath.Last() != '\\')
                //доабвляем его
                outputDirPath += "\\";
            //Пенреименовываем файлы, перенося их в выходную папку
            RenameFiles(dir, outputDirPath, mask);
            //Переносим переименованные файлы в корень
            MoveFilesToRoot(dir.FullName, outputDirPath);
            //Проходимся по дочерним папкам
            foreach (DirectoryInfo child in dir.GetDirectories())
                //Переименовываем их содержимое
                RenameFolder(child, mask);
        }



        /// <summary>
        /// Выполняем переименование файлов
        /// </summary>
        /// <param name="mask">Маска имени для переименования</param>
        /// <param name="path">Путь для переименования</param>
        public void RenameFiles(string path, string mask)
        {
            //Делаем всё это в отдельном потоке
            new Thread(() => {
                //Если он не оканчивается на слеш
                if (path.Last() != '\\')
                    //доабвляем его
                    path += "\\";
                //Получаем инфу о текущей директории
                DirectoryInfo root = new DirectoryInfo(path);
                //Выполняем рекурсивное переиме5новывание папок
                RenameFolder(root, mask);
                //Выводим сообщение о завершении операции
                MessageBox.Show("Rename complete!");
            }).Start();
        }
    }
}
