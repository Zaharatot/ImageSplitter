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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MessagesWindowLib.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для PopupMessagePanel.xaml
    /// </summary>
    public partial class PopupMessagePanel : UserControl
    {
        /// <summary>
        /// Анимация отображения контролла
        /// </summary>
        private DoubleAnimation _showPopupAnimation;
        /// <summary>
        /// Анимация скрытия контролла
        /// </summary>
        private DoubleAnimation _hidePopupAnimation;


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public PopupMessagePanel()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор контролла
        /// </summary>
        private void Init()
        {
            InitAnimations();
            InitEvents();
        }

        /// <summary>
        /// Метод инициализации анимация
        /// </summary>
        private void InitAnimations()
        {
            _showPopupAnimation = CreateShowAnimation();
            _hidePopupAnimation = CreateHideAnimation();
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void InitEvents()
        {
            //Добавляем обработчик события завершения анимации отображения контролла
            _showPopupAnimation.Completed += _showPopupAnimation_Completed;
            //Добавляем обработчик события завершения анимации скрытия контролла
            _hidePopupAnimation.Completed += _hidePopupAnimation_Completed;
        }

        /// <summary>
        /// Обработчик события завершения анимации скрытия контролла
        /// </summary>
        private void _hidePopupAnimation_Completed(object sender, EventArgs e) =>
            //Скрываем всю панель полностью
            MessageBorder.Visibility = Visibility.Collapsed;

        /// <summary>
        /// Обработчик события завершения анимации отображения контролла
        /// </summary>
        private async void _showPopupAnimation_Completed(object sender, EventArgs e) =>
            await this.Dispatcher.InvokeAsync(async () => {
                //Ждём 3 секунды
                await Task.Delay(3000);
                //Запускаем анимацию скрытия контролла
                MessageBorder.BeginAnimation(OpacityProperty, _hidePopupAnimation);
            });


        /// <summary>
        /// Метод инициализации анимации отображения контролла
        /// </summary>
        /// <returns>Созданный класс анимации</returns>
        private DoubleAnimation CreateShowAnimation() =>
            new DoubleAnimation() {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.4),
            };

        /// <summary>
        /// Метод инициализации анимации скрытия контролла
        /// </summary>
        /// <returns>Созданный класс анимации</returns>
        private DoubleAnimation CreateHideAnimation() =>
            new DoubleAnimation() {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.4)
            };



        /// <summary>
        /// Метод отображения сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void ShowMessage(string message)
        {
            //Вставляем текст в панель
            MessageTextBlock.Text = message;
            //Отображаем панель
            MessageBorder.Visibility = Visibility.Visible;
            //Запускаем анимацию отображения контролла
            MessageBorder.BeginAnimation(OpacityProperty, _showPopupAnimation);
        }
    }
}
