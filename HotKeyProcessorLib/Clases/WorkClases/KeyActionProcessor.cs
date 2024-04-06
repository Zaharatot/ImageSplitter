using ImageSplitter.Content.Clases.WorkClases.KeyProcessor.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageSplitter.Content.Clases.WorkClases.KeyProcessor
{
    /// <summary> 
    /// Класс обработки действий связанных с нажатиями клавишь
    /// </summary>
    internal class KeyActionProcessor
    {

        /// <summary>
        /// Класс проверки на то, что данное нажатие нельзя расценивать как хоткей
        /// </summary>
        private HotKeyCheck _notActionKeyCheck;
        /// <summary>
        /// Список обработчиков хоткеев
        /// </summary>
        private List<IHotKeyProcessor> _hotKeyProcessors;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public KeyActionProcessor()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _notActionKeyCheck = new HotKeyCheck();
            //Инициализируем список обработчиков кнопок
            _hotKeyProcessors = new List<IHotKeyProcessor>() { 
                new CollectionsSplitTab(_mainWork),
                new FindDuplicatesTab(_mainWork),
                new FilesRenameTab(_mainWork),
                new FilesSplitTab(_mainWork)
            };
        }



        /// <summary>
        /// Проверка нажатия кнопки "Ctrl" на клавиатуре
        /// </summary>
        /// <param name="e">Информация о нажатой кнопке</param>
        /// <returns>TRue - кнопка "Ctrl" была нажата</returns>
        private bool IsControlPressed(KeyEventArgs e) =>
            (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0;

        /// <summary>
        /// Получаем обработчик по идентификатору вкладки
        /// </summary>
        /// <param name="id">Идентификатор вкладки</param>
        /// <returns>Интерфейс обработчика</returns>
        private IHotKeyProcessor GetProcessorById(int id) =>
            _hotKeyProcessors.FirstOrDefault(processor => processor.TabId == id);


        /// <summary>
        /// Обрабатываем сочетания с клавишей Ctrl
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <param name="selectedPageId">Идентификатор выбранной вкладки</param>
        /// <returns>True - нажатие было обработано</returns>
        private bool ProcessControlKeys(Key key, int selectedPageId)
        {
            //Обрабатываем кнопку для основного окна
            bool ex = _mainKeysProcessor.ProcessControlKeys(key);
            //Если сочетание для всего окна не было найдено
            if (!ex)
            {
                //Получаем обработчик для выбранной вкладки
                IHotKeyProcessor processor = GetProcessorById(selectedPageId);
                //Если обработчик найден
                if (processor != null)
                    //Выполняем обработку для текущей вкладки
                    ex = processor.ProcessControlKeys(key);
            }
            //Возвращаемс результат
            return ex;
        }

        /// <summary>
        /// Обрабатываем обычные нажатия клавишь
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <param name="selectedPageId">Идентификатор выбранной вкладки</param>
        /// <returns>True - нажатие было обработано</returns>
        private bool ProcessKeys(Key key, int selectedPageId)
        {
            //Обрабатываем кнопку для основного окна
            bool ex = _mainKeysProcessor.ProcessKeys(key);
            //Если сочетание для всего окна не было найдено
            if (!ex)
            {
                //Получаем обработчик для выбранной вкладки
                IHotKeyProcessor processor = GetProcessorById(selectedPageId);
                //Если обработчик найден
                if (processor != null)
                    //Выполняем обработку для текущей вкладки
                    ex = processor.ProcessKeys(key);
            }
            //Возвращаемс результат
            return ex;
        }


        /// <summary>
        /// Обрабатываем нажатие на кнопку клавиатуры
        /// </summary>
        /// <param name="e">Информация о нажатой кнопке</param>
        /// <param name="selectedPageId">Идентификатор выбранной вкладки</param>
        public void ProcessKeyPress(KeyEventArgs e, int selectedPageId)
        {
            //Если данное нажатие можно обрабатывать как хоткей
            if (!_notActionKeyCheck.IsNotHotkey(e))
            {
                //Если была нажата кнопка "Ctrl"
                bool isKeyProcessed = IsControlPressed(e)
                    //Обрабатываем сочетания с клавишей Ctrl
                    ? ProcessControlKeys(e.Key, selectedPageId)
                    //В противном случае обрабатываем обычные нажатия клавишь
                    : ProcessKeys(e.Key, selectedPageId);
                //Если нажатие было обработано
                if (isKeyProcessed)
                    //Отменяем дальнейжую обработку нажатий
                    e.Handled = true;
            }
        }

    }
}
