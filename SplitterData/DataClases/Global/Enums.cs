using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitterDataLib.DataClases.Global
{
    /// <summary>
    /// Класс глобальных перечислений
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Перечисление ошибок сплиттера
        /// </summary>
        public enum SplitterErrorCodes
        {
            //Общие ошибки 0-99
            #region Common



            #endregion

            //Ошибки поиска дубликатов 100-199
            #region DuplicateScan

            #endregion

            //Ошибки сплита изображений 200-299
            #region SplitImages

            #endregion

            //Ошибки сплита файлов 300-399
            #region SplitFiles

            #endregion

            //Ошибки переименования файлов 400-499
            #region RenameFiles

            #endregion
        }

    }
}
