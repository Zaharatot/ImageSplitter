using FolderCreateWindowLib.Content.Windows;
using SplitterSimpleUI.Content.Clases.DataClases.HotKey;
using SplitterSimpleUI.Content.Clases.WorkClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FolderCreateWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс управления созданием папок
    /// </summary>
    internal class CreateFolderProcessor
    {

        /// <summary>
        /// Класс генерации имён добавляемых папок
        /// </summary>
        private FolderNameGenerator _folderNameGrnerator;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public CreateFolderProcessor()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор окна
        /// </summary>
        private void Init()
        {
            //Инициализируем класс генерации имён добавляемых папок
            _folderNameGrnerator = new FolderNameGenerator();
        }




        /// <summary>
        /// Метод получения имени папки для создания
        /// </summary>
        /// <returns>Строка нового имени папки, или NULL</returns>
        public string GetFolderName()
        {
            //По дефолту имя папки ставим в null
            string folderName = null;
            //Инициализируем окно добавления папки
            AddFolderWindow folderWindow = new AddFolderWindow();
            //Отображаем данное окно как диалоговое, и если всё ок
            if (folderWindow.ShowDialog().GetValueOrDefault(false))
                //Получаем имя папки из окна
                folderName = folderWindow.FolderName;
            //Возвращаем имя папки
            return folderName;
        }
    }
}
