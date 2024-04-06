using SplitterSimpleUI.Content.Clases.DataClases.HotKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SplitterSimpleUI.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс обработки горячих клавишь
    /// </summary>
    public class HotKeyProcessor
    {
        /// <summary>
        /// Экземпляр класса для синглтона
        /// </summary>
        private static HotKeyProcessor _instance;
        
        
        /// <summary>
        /// Словарь горячих клавишь
        /// </summary>
        private Dictionary<Window, WindowHotKeys> _hotKeysDict;


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
            //Инициализируем словарь горячих клавишь
            _hotKeysDict = new Dictionary<Window, WindowHotKeys>();
        }

        /// <summary>
        /// Предварительный обработчик события нажатия клавиши
        /// </summary>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e) =>
            //Получаем список горячих клавишь, по целевому окну, и передаём в метод обработки
            ProcessWindowHotKeys(_hotKeysDict[(Window)sender], e);



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
            //Добавляем хоткеи в словарь, проассоциировав их с целевым окном
            _hotKeysDict.Add(window, hotKeys);
            //Добавляем предварительный обработчик события нажатия клавиши
            window.PreviewKeyDown += Window_PreviewKeyDown;
        }

        /// <summary>
        /// Убираем обработку для окна
        /// </summary>
        /// <param name="window">Окно для обработки</param>
        public void RemoveWindow(Window window)
        {
            //Удаляем запись об окне из словаря
            _hotKeysDict.Remove(window);
            //Удаляем предварительный обработчик события нажатия клавиши
            window.PreviewKeyDown -= Window_PreviewKeyDown;
        }


        /// <summary>
        /// Метод получения экземпляра класса
        /// </summary>
        /// <returns>Экземпляр класса</returns>
        public static HotKeyProcessor GetInstance()
        {
            //Если экземпляр клаасса ещё не был проинициализирован
            if(_instance == null)
                //Создаём экземпляр класса
                _instance = new HotKeyProcessor();
            //Возвращаем его
            return _instance;
        }
    }
}
