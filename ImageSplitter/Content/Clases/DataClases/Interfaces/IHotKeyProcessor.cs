using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.DataClases.Interfaces
{
    /// <summary>
    /// Интерфейс класса обработки нажатий на хоткей
    /// </summary>
    internal interface IHotKeyProcessor
    {
        /// <summary>
        /// ИДентификатор целевой вкладки
        /// </summary>
        int TabId { get; }


        /// <summary>
        /// Метод обработки нажатия на сочетание клавиши действия и Ctrl
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>True - нажатие было обработано</returns>
        bool ProcessControlKeys(Key key);

        /// <summary>
        /// Метод обработки нажатия на клавишу действия
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <returns>True - нажатие было обработано</returns>
        bool ProcessKeys(Key key);


    }
}
