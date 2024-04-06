using DuplicateScanWindowLib.Content.Clases.WorkClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateScanWindowLib
{
    /// <summary>
    /// Фасадный класс работы с дубликатами
    /// </summary>
    public class DuplicateScanFasade : IDisposable
    {
        /// <summary>
        /// Класс обработки работы с дубликатами
        /// </summary>
        private DuplicateScanProcessor _duplicateScanProcessor;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicateScanFasade()
        {
            Init();
        }

        private void Init()
        {
            //Инициализируем класс рабоыт с дубликатами
            _duplicateScanProcessor = new DuplicateScanProcessor();
        }





        /// <summary>
        /// Метод отображения окна дубликатов
        /// </summary>
        public void ShowDuplicatesWindow() =>
            //Вызываем отображение окна
            _duplicateScanProcessor.ShowDuplicatesWindow();


        /// <summary>
        /// Обработчик события завершения работы с окном
        /// </summary>
        public void Dispose() =>
            //Вызываем завершение работы основного класса
            _duplicateScanProcessor?.Dispose();

    }
}
