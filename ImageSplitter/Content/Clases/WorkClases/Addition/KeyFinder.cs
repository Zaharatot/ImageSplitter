using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.WorkClases.Addition
{
    /// <summary>
    /// Класс поиска кнопок по номерам папок
    /// </summary>
    internal class KeyFinder
    {
        /// <summary>
        /// Список запрещёных клавишь
        /// </summary>
        private int[] _declinedKeys;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public KeyFinder()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем список запрещённых кнопок
            _declinedKeys = new int[] {
                //Спецкнопки на клавиатуре
                (int)Key.Separator,
                (int)Key.LWin,
                (int)Key.RWin,
                (int)Key.Apps,
                (int)Key.Sleep,
                (int)Key.PrintScreen,
                (int)Key.NumLock,
                (int)Key.Scroll,
                //Эта кнопка, по какой-то причине, не отрабатывает
                (int)Key.F10,
                //F-кнопки сверх стандартных
                (int)Key.F13,
                (int)Key.F14,
                (int)Key.F15,
                (int)Key.F16,
                (int)Key.F17,
                (int)Key.F18,
                (int)Key.F19,
                (int)Key.F20,
                (int)Key.F21,
                (int)Key.F22,
                (int)Key.F23,
                (int)Key.F24,
            };
        }



        /// <summary>
        /// Получаем клавишу для папки по id папки
        /// </summary>
        /// <param name="id">Id папки</param>
        /// <returns>Код клавиши</returns>
        public Key GetKeyByNumber(int id)
        {
            //Получаем код клавиши
            int keyId = (int)Key.NumPad0;
            //Цикл идёт до тех пор, пока не
            //дойдём до нужного id клавиши
            while (id > 0)
            {
                //Переходим к следующей клавише
                keyId++;
                //Если данная клавиша не запрещена
                if (!_declinedKeys.Contains(keyId))
                    //Уменьшаем ID
                    id--;
            }
            //Возвращаем кнопку
            return (Key)keyId;
        }
    }
}
