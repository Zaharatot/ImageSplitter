using ImageSplitter.Content.Clases.DataClases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static ImageSplitter.Content.Clases.DataClases.Global.Delegates;

namespace ImageSplitter.Content.Clases.WorkClases.KeyProcessor.Processors
{
    /// <summary>
    /// Класс обработки нажатия на хоткей, для вкладки сплита коллекций
    /// </summary>
    internal class CollectionsSplitTab : IHotKeyProcessor
    {
        /// <summary>
        /// Событие запроса на переход к изображению
        /// </summary>
        public static event MoveToImageEventHandler MoveToCollectionRequest;
        /// <summary>
        /// Событие запроса на переход к изображению
        /// </summary>
        public static event MoveToImageEventHandler MoveToImageRequest;

        /// <summary>
        /// Идентификатор вкладки сплита изображений
        /// </summary>
        public int TabId => 0;

        /// <summary>
        /// Ссылка на основной рабочий класс
        /// </summary>
        private MainWork _mainWork;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="mainWork">Ссылка на основной рабочий класс</param>
        public CollectionsSplitTab(MainWork mainWork)
        {
            //Проставляем переданное значение
            _mainWork = mainWork;
        }

        /// <summary>
        /// Метод обработки нажатия на сочетание клавиши действия и Ctrl
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>True - нажатие было обработано</returns>
        public bool ProcessControlKeys(Key key)
        {
            bool ex = true;
            //Выбираем действие по кнопке
            switch (key)
            {
                //Если было нажато сочетание "Ctrl+N"
                case Key.N:
                    {
                        //Вызываем метод добавления папки
                        _mainWork.AddNewFolder();
                        break;
                    }
                //Во всех остальных случаях игнорируем нажатие
                default:
                    {
                        ex = false;
                        break;
                    }
            }
            //Возвращаем результат
            return ex;
        }

        /// <summary>
        /// Метод обработки нажатия на клавишу действия
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>True - нажатие было обработано</returns>
        public bool ProcessKeys(Key key)
        {
            bool ex = true;
            //Выбираем действие по кнопке
            switch (key)
            {
                //Если было нажата кнопка Left
                case Key.Left:
                    {
                        //Идём к предыдущей картинке
                        MoveToCollectionRequest?.Invoke(-1);
                        break;
                    }
                //Если было нажата кнопка Right
                case Key.Right:
                    {
                        //Идём к следующей картинке
                        MoveToCollectionRequest?.Invoke(1);
                        break;
                    }
                //Если было нажата кнопка Up
                case Key.Up:
                    {
                        //Идём к предыдущей картинке в коллекции
                        MoveToImageRequest?.Invoke(-1);
                        break;
                    }
                //Если было нажата кнопка Down
                case Key.Down:
                    {
                        //Идём к следующей картинке в коллекции
                        MoveToImageRequest?.Invoke(1);
                        break;
                    }
                //Во всех остальных случаях игнорируем нажатие
                default:
                    {
                        //Проверяем нажатую кнопку на тип кнопки переноса
                        ex = _mainWork.CheckImageMoveTarget(key);
                        break;
                    }
            }
            //Возвращаем результат
            return ex;
        }
    }
}
