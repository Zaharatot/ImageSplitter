using SplitterSimpleUI.Content.Clases.DataClases.HotKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SplitterSimpleUI.Content.Clases.WorkClases.HotKey
{
    /// <summary>
    /// Класс обработки горячих клавишь
    /// </summary>
    public class HotKeyProcessor
    {
        /// <summary>
        /// Словарь горячих клавишь
        /// </summary>
        private WindowHotKeys _hotKeys;
        /// <summary>
        /// Класс проверки на хоткей
        /// </summary>
        private HotKeyCheck _hotKeyCheck;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public HotKeyProcessor()
        {
            Init();
        }

        /// <summary>
        /// Инициализаптор класса
        /// </summary>
        private void Init()
        {
            //Ставим дефолтное значение для окна хоткеев
            _hotKeys = null;
            //Инициализируем класс проверки на хоткей
            _hotKeyCheck = new HotKeyCheck();
        }

        /// <summary>
        /// Предварительный обработчик события нажатия клавиши
        /// </summary>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Если нажатие выполнено именно на хоткей
            if(!_hotKeyCheck.IsNotHotkey(e))
                //Получаем список горячих клавишь, по целевому окну, и передаём в метод обработки
                ProcessWindowHotKeys(_hotKeys, e);
        }


        /// <summary>
        /// Проверка нажатия кнопки "Ctrl" на клавиатуре
        /// </summary>
        /// <param name="e">Информация о нажатой кнопке</param>
        /// <returns>TRue - кнопка "Ctrl" была нажата</returns>
        private bool IsControlPressed(KeyEventArgs e) =>
            (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0;

        /// <summary>
        /// Метод обработки хоткеев окна
        /// </summary>
        /// <param name="hotKeys">Список хоткеев окна</param>
        /// <param name="e">Информация о нажатой кнопке</param>
        private void ProcessWindowHotKeys(WindowHotKeys hotKeys, KeyEventArgs e)
        {           
            //Получаем флаг нажатия на клавишу Contrl
            bool isContrl = IsControlPressed(e);
            //Получаем информацию о хоткее, который соответствует нажатой клавише
            HotKeyInfo info = hotKeys.GetPressedHotKey(e.Key, isContrl);
            //Если подобный хоткей найден
            if (info != null)
            {
                //Вызываем метод обработки
                info.Method.Invoke();
                //Проставляем флаг обработки нажатия
                e.Handled = hotKeys.IsHandled;
            }
            //Если хоткея нет, но есть метод альтернативной обработки
            else if (hotKeys.IsContainOtherProcessMethod)
            {
                //Вызываем метод для альтернативной обработки
                hotKeys.IsOtherPressMethod.Invoke(e.Key);
                //Проставляем флаг обработки нажатия
                e.Handled = hotKeys.IsHandled;
            }
        }



        /// <summary>
        /// Метод добавления окна
        /// </summary>
        /// <param name="window">Окно для обработки</param>
        /// <param name="hotKeys">Список горячих клавишь для окна</param>
        public void AddWindow(Window window, WindowHotKeys hotKeys)
        {
            //Запоминаем переданные хоткеи
            _hotKeys = hotKeys;
            //Добавляем предварительный обработчик события нажатия клавиши
            window.PreviewKeyDown += Window_PreviewKeyDown;
        }

        /// <summary>
        /// Убираем обработку для окна
        /// </summary>
        /// <param name="window">Окно для обработки</param>
        public void RemoveWindow(Window window) =>
            //Удаляем предварительный обработчик события нажатия клавиши
            window.PreviewKeyDown -= Window_PreviewKeyDown;       

    }
}
