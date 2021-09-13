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
    /// Класс обработки нажатия на хоткей, для основного окна
    /// </summary>
    internal class MainKeys : IHotKeyProcessor
    {
        /// <summary>
        /// События запроса на переход ко вкладке
        /// </summary>
        public static event SendToTabRequestEventHandler SendToTabRequest;

        /// <summary>
        /// Идентификатор вкладки сплита изображений
        /// </summary>
        public int TabId => -1;


        /// <summary>
        /// Ссылка на основной рабочий класс
        /// </summary>
        private MainWork _mainWork;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="mainWork">Ссылка на основной рабочий класс</param>
        public MainKeys(MainWork mainWork)
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
                //Если было нажато сочетание "Ctrl+1"
                case Key.D1:
                    {
                        //Переход на вкладку сплита коллекций
                        SendToTabRequest?.Invoke(0);
                        break;
                    }
                //Если было нажато сочетание "Ctrl+2"
                case Key.D2:
                    {
                        //Переход на вкладку сплита файлов
                        SendToTabRequest?.Invoke(1);
                        break;
                    }
                //Если было нажато сочетание "Ctrl+3"
                case Key.D3:
                    {
                        //Переход на вкладку переименования файлов
                        SendToTabRequest?.Invoke(2);
                        break;
                    }
                //Если было нажато сочетание "Ctrl+4"
                case Key.D4:
                    {
                        //Переход на вкладку сканирования дубликатов
                        SendToTabRequest?.Invoke(3);
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
            //Никак не обрабатывается
            return false;
        }
    }
}
