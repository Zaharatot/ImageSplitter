using SplitterResources;
using SplitterResources.Content.Clases.DataClases;
using SplitterSimpleUI.Content.Clases.DataClases.HotKey;
using SplitterSimpleUI.Content.Clases.WorkClases.HotKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MessagesWindowLib.Content.Clases.DataClases.Enums;
using static SplitterDataLib.DataClases.Global.Delegates;

namespace MessagesWindowLib.Content.Windows
{
    /// <summary>
    /// Логика взаимодействия для MessageBoxWindow.xaml
    /// </summary>
    public partial class MessageBoxWindow : Window
    {

        /// <summary>
        /// Класс обработки хоткеев
        /// </summary>
        private HotKeyProcessor _hotKeyProcessor;

        /// <summary>
        /// Сломарь методов обработки кнопок
        /// </summary>
        private Dictionary<MessageBoxTypes, EmptyEventHandler> _setButtonsMethodsDict;

        /// <summary>
        /// Текст кнопки согласия
        /// </summary>
        private string _yesButtonText;
        /// <summary>
        /// Текст кнопки принятия
        /// </summary>
        private string _okButtonText;


        /// <summary>
        /// Конструктор окна
        /// </summary>
        public MessageBoxWindow()
        {
            InitializeComponent();
            Init();
        }


        #region Initialization 

        /// <summary>
        /// Инициализатор окна
        /// </summary>
        private void Init()
        {
            InitVariables();
            InitHotkeys();
        }

        /// <summary>
        /// Инициализируем значения переменных
        /// </summary>
        private void InitVariables()
        {
            //Инициализируем словарь методов отображения кнопок
            _setButtonsMethodsDict = CreateButtonsDict();
            //Грузим из ресурсов названия кнопок
            _yesButtonText = ResourceLoader.LoadString("Text_MessageBoxWindow_YesButton_Content");
            _okButtonText = ResourceLoader.LoadString("Text_MessageBoxWindow_OkButton_Content");
        }

        /// <summary>
        /// Инициализируем хоткеи
        /// </summary>
        private void InitHotkeys()
        {
            //Получаем экземпляр класса обработки хоткеев
            _hotKeyProcessor = new HotKeyProcessor();
            //Добавляем хоткеи для текущего окна
            AddHotKeys();
        }

        /// <summary>
        /// Метод добавления хоткеев для окна
        /// </summary>
        private void AddHotKeys() =>
            _hotKeyProcessor.AddWindow(this, new WindowHotKeys(true) {
                //Добавляем список хоткеев
                HotKeys = new List<HotKeyInfo>() { 
                    //При нажатии на "Enter" - вызываем закрытие окна с подтверждением
                    new HotKeyInfo(Key.Enter, () => {  this.DialogResult = true; }),
                    //При нажатии на "Escape" - вызываем закрытие окна с отказом
                    new HotKeyInfo(Key.Escape, () => { this.DialogResult = false; }),
                }
            });


        /// <summary>
        /// Метод инициализации словаря методов отображения кнопок
        /// </summary>
        /// <returns>Словарь методов отображения кнопок</returns>
        private Dictionary<MessageBoxTypes, EmptyEventHandler> CreateButtonsDict() =>
            new Dictionary<MessageBoxTypes, EmptyEventHandler>() {
                { MessageBoxTypes.OkMessage, SetOkButtons },
                { MessageBoxTypes.YesNoMessage, SetYesNoButtons },
            };

        #endregion

        #region Events 

        /// <summary>
        /// Обработчик осбытия закрытия окна
        /// </summary>
        private void Window_Closed(object sender, EventArgs e) =>
            //Удаляем обюработку хоткеев для данного окна
            _hotKeyProcessor.RemoveWindow(this);


        /// <summary>
        /// Обработчик события нажатия на кнопку принятия выбора
        /// </summary>
        private void YesButton_Click(object sender, RoutedEventArgs e) =>
            //Вызываем закрытие окна с подтверждением
            this.DialogResult = true;

        /// <summary>
        /// Обработчик события нажатия на кнопку отклонения выбора
        /// </summary>
        private void NoButton_Click(object sender, RoutedEventArgs e) =>
            //Вызываем закрытие окна с отказом
            this.DialogResult = false;

        #endregion

        #region ButtonsUpdate 

        /// <summary>
        /// Метод простановки месседжбокса с запросом
        /// </summary>
        private void SetYesNoButtons()
        {
            //Отображаем обе кнопки
            YesButton.Visibility = NoButton.Visibility = Visibility.Visible;
            //Ставим соответствующую подпись
            YesButton.Content = _yesButtonText;
        }

        /// <summary>
        /// Метод простановки месседжбокса с уведомлением
        /// </summary>
        private void SetOkButtons()
        {
            //Отображаем только кнопку согласия
            YesButton.Visibility = Visibility.Visible;
            NoButton.Visibility = Visibility.Collapsed;
            //Ставим соответствующую подпись
            YesButton.Content = _okButtonText;
        }

        #endregion

        #region LoadInfo 

        /// <summary>
        /// Метод загрузки текста сообщения
        /// </summary>
        /// <param name="message">Тип сообщения</param>
        /// <returns>Строка текста сообщения</returns>
        private string LoadMessageText(MessageBoxMessages message) =>
            //Грузим текст из ресурсов
            ResourceLoader.LoadString($"Text_MessageBox_{message}");

        /// <summary>
        /// Метод загрузки заголовка сообщения
        /// </summary>
        /// <param name="level">Уровень сообщения</param>
        /// <returns>Строка заголовка сообщения</returns>
        private string LoadMessageHeader(MessageBoxLevels level) =>
            //Грузим текст из ресурсов
            ResourceLoader.LoadString($"Text_MessageBox_Header_{level}");

        /// <summary>
        /// Метод загрузки иконки сообщения
        /// </summary>
        /// <param name="level">Уровень сообщения</param>
        /// <returns>Иконка сообщения</returns>
        private SvgImage LoadMessageIcon(MessageBoxLevels level) =>
            //Грузим иконку из ресурсов
            ResourceLoader.LoadIcon($"Icon_{level}");

        #endregion


        /// <summary>
        /// Метод отображения информации о сообщении
        /// </summary>
        /// <param name="type">Тип сообщения</param>
        /// <param name="level">Уровень сообщения</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="addInfo">Дополнительная информация</param>
        public void SetMessageBoxInfo(MessageBoxTypes type, MessageBoxLevels level, MessageBoxMessages message, string addInfo)
        {
            //Вызываем метод обновления кнопок по типу сообщения
            _setButtonsMethodsDict[type].Invoke();
            //Грузим текст сообщения
            string messageText = LoadMessageText(message);
            //Добавляем в текст доп. инфу и выводим его в контролл
            MessageTextBlock.Text = string.Format(messageText, addInfo);
            //Грузим заголовок сообщения
            this.Title = LoadMessageHeader(level);
            //Грузим иконку сообщения
            StatusIcon.Image = LoadMessageIcon(level);
        }
    }
}
