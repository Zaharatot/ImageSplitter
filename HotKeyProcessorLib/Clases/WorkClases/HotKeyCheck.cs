using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.WorkClases.KeyProcessor
{
    /// <summary>
    /// Класс проверки на то, что данное нажатие нельзя расценивать как хоткей
    /// </summary>
    internal class HotKeyCheck
    {

        /// <summary>
        /// Список запрещёных клавишь
        /// </summary>
        private Key[] _declinedKeys;
        /// <summary>
        /// Список запрещённых для обработки контроллов
        /// </summary>
        private Type[] _declinedSources;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public HotKeyCheck()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем массив запрещённых клавишь
            _declinedKeys = new Key[] { 
                Key.Tab, 
                Key.Enter, 
                Key.Escape,
                Key.CapsLock,
                Key.Home,
                Key.End,
                Key.PageUp,
                Key.PageDown
            };
            //Инициализируем массив запрещённых контроллов
            _declinedSources = new Type[] { 
                typeof(TextBox)
            };
        }

        /// <summary>
        /// Проверка на запрещённую клавишу
        /// </summary>
        /// <param name="key">Нажатая клавиша</param>
        /// <returns>True - данная клавиша запрещена</returns>
        private bool IsDeclinedKey(Key key) =>
            _declinedKeys.Contains(key);

        /// <summary>
        /// Проверка на запрещённый источник ивента
        /// </summary>
        /// <param name="source">Контролл-источник ивента</param>
        /// <returns></returns>
        private bool IsDeclinedSource(object source) =>
            _declinedSources.Contains(source.GetType());


        /// <summary>
        /// Проверка на то, что данное нажатие нельзя считать хоткеем
        /// </summary>
        /// <param name="e">Информация о нажатой кнопке</param>
        /// <returns>True - обрабатывать хоткей нельзя</returns>
        public bool IsNotHotkey(KeyEventArgs e) =>
            //ПРоверка по типу нажатой кнопки и контроллу-источнику
            IsDeclinedKey(e.Key) || IsDeclinedSource(e.OriginalSource);

    }
}
