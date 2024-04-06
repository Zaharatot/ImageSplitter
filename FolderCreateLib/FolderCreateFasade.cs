using FolderCreateWindowLib.Content.Clases.WorkClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderCreateWindowLib
{
    /// <summary>
    /// Фасадный класс библиотеки работы с созданием папки
    /// </summary>
    public class FolderCreateFasade
    {
        /// <summary>
        /// Класс обработки создания папки
        /// </summary>
        private CreateFolderProcessor _createFolderProcessor;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FolderCreateFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _createFolderProcessor = new CreateFolderProcessor();
        }



        /// <summary>
        /// Метод получения имени папки для создания
        /// </summary>
        /// <returns>Строка нового имени папки, или NULL</returns>
        public string GetFolderName() =>
            //Вызываем внутренний метод
            _createFolderProcessor.GetFolderName();
    }
}
