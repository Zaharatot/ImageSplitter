using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeViewWindowLib.Content.Clases.WorkClases;

namespace TreeViewWindowLib
{
    /// <summary>
    /// Фасадный класс библиотеки  отображения древа
    /// </summary>
    public class TreeViewFasade
    {
        /// <summary>
        /// Класс работы с древом
        /// </summary>
        private TreeElementsProcessor _treeElementsProcessor;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TreeViewFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _treeElementsProcessor = new TreeElementsProcessor();
        }





        /// <summary>
        /// Метод отображения древа
        /// </summary>
        /// <param name="path">Путь к древу</param>
        public async Task ShowTree(string path) =>
            //Вызываем внутренний мето
            await _treeElementsProcessor.ShowTree(path);
    }
}
