using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SplitImagesWindowLib.Content.Clases.DataClases
{
    /// <summary>
    /// Класс глобальных делегатов событий
    /// </summary>
    public class Delegates
    {

        /// <summary>
        /// Делегат события перехода к изображению
        /// </summary>
        /// <param name="direction">Направление перехода</param>
        public delegate void MoveToImageEventHandler(int direction);

        /// <summary>
        /// Делегат события запроса на удаление целевой папки из списка
        /// </summary>
        /// <param name="key">Клавиша, к которой привязана папка</param>
        /// <param name="folderName">Имя папки</param>
        public delegate void RemoveFolderRequestEventHandler(Key key, string folderName);
    }
}
