using ImageSplitter.Content.Clases.WorkClases.Addition;
using ImageSplitter.Content.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageSplitter.Content.Clases.WorkClases.Processors.ImageSplit
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
            //Инициализируем окно добавления папки
            AddFolderWindow folderWindow = new AddFolderWindow();
            //Отображаем данное окно как диалоговое, и если всё ок
            if (folderWindow.ShowDialog().GetValueOrDefault(false))
            {
                //Получаем имя папки из окна
                string folderName = folderWindow.FolderName;
                //Если имя для папки не пустое
                if (!string.IsNullOrEmpty(folderName)) 
                    //Возвращаем его
                    return folderName;
                //Выводим сообщение обю ошибке (если имя пустое)
                MessageBox.Show("Имя папки не должно быть пустым!", "Ошибка!");
            }
            //Возвращаем пустую строку
            return null;
        }
    }
}
