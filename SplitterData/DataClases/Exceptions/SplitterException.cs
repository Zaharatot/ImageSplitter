using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SplitterDataLib.DataClases.Global.Enums;

namespace SplitterDataLib.DataClases.Exceptions
{
    /// <summary>
    /// Класс, описывающий внутреннюю ошибку сплиттера
    /// </summary>
    public class SplitterException : Exception
    {

        /// <summary>
        /// Код ошибки
        /// </summary>
        public SplitterErrorCodes Code { get; private set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="code">Код ошибки</param>
        public SplitterException(SplitterErrorCodes code) : base()
        {
            //Проставляем переданные значения
            Code = code;
        }

    }
}
