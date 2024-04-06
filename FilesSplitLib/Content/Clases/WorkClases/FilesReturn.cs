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
    /// Класс возврата файлов
    /// </summary>
    internal class FilesReturn
    {

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FilesReturn()
        {

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
        /// <param name="info">Информация о перемещаемых файлах</param>
        public void StartReturn(MoveFilesInfo info) =>
            //Делаем всё это в отдельном потоке
            new Thread(() => {
                //Проходимся по всем папкам корневой
                foreach (DirectoryInfo dir in info.Parent.GetDirectories())
                    //Переносим их дочерние файлы в корневую
                    MoveChild(info.Parent.FullName, dir);

                //TODO: Заменить на нормальное сообщение
                //Выводим сообщение о завершении операции
                MessageBox.Show("Back move complete!");
            }).Start();

    }
}
