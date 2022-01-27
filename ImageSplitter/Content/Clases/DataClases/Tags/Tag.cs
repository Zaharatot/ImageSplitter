using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.DataClases.Tags
{
    /// <summary>
    /// Класс, описывающий тег
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Идентификатор тега
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// Строка имени тега
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Код клавиши, к которой привязан тег
        /// </summary>
        public Key KeyCode { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public Tag()
        {
            //Инициализируем дефолтные значения
            Id = 0;
            KeyCode = Key.A;
            Name = "Новый тег";
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="id">Id тега</param>
        /// <param name="name">Имя тега</param>
        /// <param name="keyCode">Код привязанной клавиши</param>
        public Tag(string name, Key keyCode)
        {
            //Инициализируем дефолтные значения
            Id = 0;
            //Проставляем переданные значения
            Name = name;
            KeyCode = keyCode;
        }

        /// <summary>
        /// Получаем строку с текстом тега
        /// </summary>
        /// <returns>Строка текста тега</returns>
        public string GetTagText() => $"[{KeyCode}] {Name}";

        /// <summary>
        /// Получаем строку с кодом клавиши тега
        /// </summary>
        /// <returns>Строка с кодом клавиши тега</returns>
        public string GetTagLetter() => $"[{KeyCode}]";
    }
}
