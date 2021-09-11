using ImageSplitter.Content.Clases.DataClases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageSplitter.Content.Clases.DataClases.Enums;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit
{
    /// <summary>
    /// Класс переноса файлов
    /// </summary>
    internal class Mover
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public Mover()
        {

        }

        /// <summary>
        /// Перенос файла в целевую папку
        /// </summary>
        /// <param name="target">Информация о целевой папке</param>
        /// <param name="image">Информация о переносимом файле</param>
        public void MoveFile(TargetFolderInfo target, ImageInfo image)
        {
            //Проставляем старый путь
            string oldPath = image.GetCurrentPath();
            //Получаем новое имя файла
            image.NewFileName = GetNewFileName(target.Path, image.OriginalFileName);
            //Получаем нвоый путь
            image.NewPath = target.Path;
            //Проставляем новое имя папки
            image.NewFolderName = target.Name;
            //Обновляем сттус картинке
            image.Status = ImageStatuses.Moved;
            //Переносим файл
            File.Move(oldPath, $"{target.Path}\\{image.NewFileName}");
        }

        /// <summary>
        /// Подбираем новое имя для файла, которого нету в новой папке
        /// </summary>
        /// <param name="path">Путь для поиска</param>
        /// <param name="name">Старое имя файла</param>
        /// <returns>Уникальное имя файла</returns>
        private string GetNewFileName(string path, string name)
        {
            string ex = name;
            int id = 0;
            //Пока есть такой файл
            while (File.Exists($"{path}\\{ex}"))
                //Генерим новые имена
                ex = $"({id++}) {name}";
            return ex;
        }

    }
}
