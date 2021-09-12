using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.WorkClases.Addition
{
    /// <summary>
    /// Класс обработки действий связанных с нажатиями клавишь
    /// </summary>
    internal class KeyActionProcessor
    {
        /// <summary>
        /// Идентификатор вкладки сплита изображений
        /// </summary>
        private const int IMAGES_SPLIT_TAB_ID = 0;
        /// <summary>
        /// Идентификатор вкладки сплита файлов
        /// </summary>
        private const int FILES_SPLIT_TAB_ID = 1;
        /// <summary>
        /// Идентификатор вкладки переименования файлов
        /// </summary>
        private const int FILES_RENAME_TAB_ID = 2;
        /// <summary>
        /// Идентификатор вкладки поиска дубликатов изображений
        /// </summary>
        private const int IMAGE_DUPLICATES_TAB_ID = 3;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public KeyActionProcessor()
        {

        }



        /// <summary>
        /// Проверка нажатия кнопки "Ctrl" на клавиатуре
        /// </summary>
        /// <param name="e">Информация о нажатой кнопке</param>
        /// <returns>TRue - кнопка "Ctrl" была нажата</returns>
        private bool IsControlPressed(KeyEventArgs e) =>
            (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0;


        /// <summary>
        /// Обрабатываем сочетания с клавишей Ctrl
        /// </summary>
        /// <param name="e">Информация о нажатой кнопке</param>
        /// <param name="selectedPageId">Идентификатор выбранной вкладки</param>
        /// <returns>True - нажатие было обработано</returns>
        private bool ProcessControlKeys(KeyEventArgs e, int selectedPageId)
        {
            bool ex = false;
       /*     //Если было нажато сочетание "Ctrl+N"
            if (e.Key == Key.N)
                //Вызываем метод добавления папки
                _mainWork.AddNewFolder();*/
            //Возвращаем результат
            return ex;
        }

        /// <summary>
        /// Обрабатываем обычные нажатия клавишь
        /// </summary>
        /// <param name="e">Информация о нажатой кнопке</param>
        /// <param name="selectedPageId">Идентификатор выбранной вкладки</param>
        /// <returns>True - нажатие было обработано</returns>
        private bool ProcessKeys(KeyEventArgs e, int selectedPageId)
        {
            bool ex = false;
            //При нажатии кнопки "Влево"
        /*   if (e.Key == Key.Left)
                //Идём к предыдущей картинке
                MoveToCollection(-1);
            //При нажатии кнопки "Вправо"
            else if (e.Key == Key.Right)
                //Идём к следующей картинке
                MoveToCollection(1);
            //При нажатии кнопки "Вверх"
            else if (e.Key == Key.Up)
                //Идём к предыдущей картинке в коллекции
                SplitImages.MoveFolderImage(-1);
            //При нажатии кнопки "Вниз"
            else if (e.Key == Key.Down)
                //Идём к следующей картинке в коллекции
                SplitImages.MoveFolderImage(1);
            else
                //Проверяем нажатую кнопку на тип кнопки переноса
                _mainWork.CheckImageMoveTarget(e.Key);*/
            //Возвращаем результат
            return ex;
        }


        /// <summary>
        /// Обрабатываем нажатие на кнопку клавиатуры
        /// </summary>
        /// <param name="e">Информация о нажатой кнопке</param>
        /// <param name="selectedPageId">Идентификатор выбранной вкладки</param>
        public void ProcessKeyPress(KeyEventArgs e, int selectedPageId)
        {
            //Если была нажата кнопка "Ctrl"
            bool isKeyProcessed =  IsControlPressed(e)
                //Обрабатываем сочетания с клавишей Ctrl
                ? ProcessControlKeys(e, selectedPageId)
                //В противном случае обрабатываем обычные нажатия клавишь
                : ProcessKeys(e, selectedPageId);
            //Если нажатие было обработано
            if (isKeyProcessed)
                //Отменяем дальнейжую обработку нажатий
                e.Handled = true;
        }

    }
}
