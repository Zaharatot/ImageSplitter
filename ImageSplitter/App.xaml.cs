using ImageSplitter.Content.Clases.WorkClases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ImageSplitter
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Основной рабочий класс приложения
        /// </summary>
        private MainWork _mainWork;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события завершения работы приложения
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //Завершаем работу приложения
            Close();
        }


        /// <summary>
        /// Обработчик события завершения запуска приложения
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Инициализируем приложение
            Init();
        }


        /// <summary>
        /// Инициализатор приложения
        /// </summary>
        private void Init()
        {
            //Инициализируем основной рабочий класс приложения
            _mainWork = new MainWork();
            //Отображаем основное окно приложения
            _mainWork.ShowMainWindow();
        }

        /// <summary>
        /// Закрываем приложение
        /// </summary>
        private void Close()
        {
            //Завершаем работу основного рабочего класса приложения
            _mainWork?.Dispose();
        }
    }
}
