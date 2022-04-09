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

/*
 
//Список клавишь, которые можно использовать

[Left = 23]
[Up = 24]
[Right = 25]
[Down = 26]


[D0 = 34]
[D1 = 35]
[D2 = 36]
[D3 = 37]
[D4 = 38]
[D5 = 39]
[D6 = 40]
[D7 = 41]
[D8 = 42]
[D9 = 43]
[A = 44]
[B = 45]
[C = 46]
[D = 47]
[E = 48]
[F = 49]
[G = 50]
[H = 51]
[I = 52]
[J = 53]
[K = 54]
[L = 55]
[M = 56]
[N = 57]
[O = 58]
[P = 59]
[Q = 60]
[R = 61]
[S = 62]
[T = 63]
[U = 64]
[V = 65]
[W = 66]
[X = 67]
[Y = 68]
[Z = 69]

[NumPad0 = 74]
[NumPad1 = 75]
[NumPad2 = 76]
[NumPad3 = 77]
[NumPad4 = 78]
[NumPad5 = 79]
[NumPad6 = 80]
[NumPad7 = 81]
[NumPad8 = 82]
[NumPad9 = 83]
[Multiply = 84]
[Add = 85]
[Separator = 86]
[Subtract = 87]

[Divide = 89]
[F1 = 90]
[F2 = 91]
[F3 = 92]
[F4 = 93]
[F5 = 94]
[F6 = 95]
[F7 = 96]
[F8 = 97]
[F9 = 98]
[F10 = 99]
[F11 = 100]
[F12 = 101]


 */