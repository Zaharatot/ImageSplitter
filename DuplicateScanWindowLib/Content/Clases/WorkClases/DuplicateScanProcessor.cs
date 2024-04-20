using DuplicateScannerLib;
using DuplicateScannerLib.Clases.DataClases.File;
using DuplicateScannerLib.Clases.DataClases.Result;
using DuplicateScanWindowLib.Content.Windows;
using MessagesWindowLib;
using DuplicateScannerLib.Clases.DataClases.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static MessagesWindowLib.Content.Clases.DataClases.Enums;

namespace DuplicateScanWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс обработки сканирования дубликатов
    /// </summary>
    internal class DuplicateScanProcessor : IDisposable
    {
        /// <summary>
        /// Окно сканера дубликатов
        /// </summary>
        private DuplicateScanWindow _duplicateScanWindow;

        /// <summary>
        /// Фаскдный класс библиотеки сканирования дубликатов
        /// </summary>
        private DuplicateScannerFasade _duplicateScannerFasade;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DuplicateScanProcessor()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            InitVariables();
            InitEvents();
        }

        /// <summary>
        /// Инициализируем значения переменных
        /// </summary>
        private void InitVariables()
        {
            //Инициализируем фасадный класс библиотеки сканирования дубликатов
            _duplicateScannerFasade = new DuplicateScannerFasade();
            //Инициализируем основное окно
            _duplicateScanWindow = new DuplicateScanWindow();
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void InitEvents()
        {
            //Добавляем обработчик события запуска удаления дубликатов
            _duplicateScanWindow.DuplicateRemove += _duplicateScanWindow_DuplicateRemove;
            //Добавляем обработчик события запроса удаления старых записей
            _duplicateScanWindow.RemoveOldRequest += _duplicateScanWindow_RemoveOldRequest;
            //Добавляем обработчик события запуска скнирования на дубликаты
            _duplicateScanWindow.StartDuplicateScan += _duplicateScanWindow_StartDuplicateScan;

            //Добавляем обработчик события обновления статуса сканирования на дубликаты
            DuplicateScannerFasade.UpdateScanInfo += DuplicateScannerFasade_UpdateScanInfo;
            //Добавляем обработчик события завершения сканирования на дубликаты
            DuplicateScannerFasade.CompleteScan += DuplicateScannerFasade_CompleteScan;
            //Добавляем обработчик события обновления статуса удаления выбранных дубликатов
            DuplicateScannerFasade.UpdateRemoveInfo += DuplicateScannerFasade_UpdateRemoveInfo;
            //Добавляем обработчик события завершения удаления выбранных дубликатов
            DuplicateScannerFasade.CompleteRemove += DuplicateScannerFasade_CompleteRemove;
            //Добавляем обработчик события завершения удаления устаревших дубликатов
            DuplicateScannerFasade.CompleteRemoveOldDuplicates += DuplicateScannerFasade_CompleteRemoveOldDuplicates;
        }




        /// <summary>
        /// Обработчик события обновления статуса удаления выбранных дубликатов
        /// </summary>
        /// <param name="info">ИНформация о статусе удаления дубликатов</param>
        private void DuplicateScannerFasade_UpdateRemoveInfo(ProgressInfo info) =>
            //Передаём информацию о прогрессе в окно
            _duplicateScanWindow.UpdateRemoveInfo(info);

        /// <summary>
        /// Обработчик события обновления статуса сканирования на дубликаты
        /// </summary>
        /// <param name="info">ИНформация о статусе сканирования</param>
        private void DuplicateScannerFasade_UpdateScanInfo(ScanProgressInfo info) =>
            //Передаём информацию о прогрессе в окно
            _duplicateScanWindow.UpdateScanInfo(info);

        /// <summary>
        /// Обработчик события завершения удаления выбранных дубликатов
        /// </summary>
        private void DuplicateScannerFasade_CompleteRemove()
        {
            //Выводим сообщение о результате
            MessagesBoxFasade.ShowMessageBoxDone(
                MessageBoxMessages.DuplicateRemoveComplete);
            //Вызываем метод окна
            _duplicateScanWindow.CompleteRemove();
        }

        /// <summary>
        /// Обработчик события завершения удаления устаревших записей о дубликатах
        /// </summary>
        private void DuplicateScannerFasade_CompleteRemoveOldDuplicates(int count)
        {
            //Выводим сообщение о результате
            MessagesBoxFasade.ShowMessageBoxDone(
                MessageBoxMessages.DuplicateRemoveOldElements, count.ToString());
            //Вызываем метод окна
            _duplicateScanWindow.CompleteRemoveOldDuplicates();
        }

        /// <summary>
        /// Обработчик события завершения сканирования на дубликаты
        /// </summary>
        /// <param name="result">Результат сканирования на дубликаты</param>
        private void DuplicateScannerFasade_CompleteScan(List<DuplicatePair> result)
        {
            //Если дубликаты не были найдены
            if (result.Count == 0)
                //Выводим сообщение о результате
                MessagesBoxFasade.ShowMessageBoxDone(
                    MessageBoxMessages.DuplicateScanNotFound);
            //Вызываем метод окна
            _duplicateScanWindow.CompleteScan(result);
        }



        /// <summary>
        /// Обработчик события запуска удаления дубликатов
        /// </summary>
        /// <param name="groups">Список запрещённых групп</param>
        /// <param name="toRemove">Группа хешей для удаления</param>
        private void _duplicateScanWindow_DuplicateRemove(HashesGroup toRemove, List<HashesGroup> groups) =>
            //Вызываем внутренний метод
            _duplicateScannerFasade.RemoveDuplicates(toRemove, groups);

        /// <summary>
        /// Обработчик событяи запуска сканирования
        /// </summary>
        /// <param name="properties">Параметры сканирования</param>
        private void _duplicateScanWindow_StartDuplicateScan(ScanProperties properties) =>
            //Вызываем внутренний метод
            _duplicateScannerFasade.StartDuplicateScan(properties);

        /// <summary>
        /// Обработчик события запроса на удаление старых элементов
        /// </summary>
        private void _duplicateScanWindow_RemoveOldRequest() =>
            //Вызываем внутренний метод
            _duplicateScannerFasade.RemoveOldDuplicates();





        /// <summary>
        /// Метод отображения окна дубликатов
        /// </summary>
        public void ShowDuplicatesWindow() =>
            //Вызываем отображение окна
            _duplicateScanWindow.Show();


        /// <summary>
        /// Обработчик события завершения работы с окном
        /// </summary>
        public void Dispose()
        {
            //Если окно дубликатов существует
            if(_duplicateScanWindow != null)
            {
                //Разрешаем закрыть окно дубликатов
                _duplicateScanWindow.IsAllowClose = true;
                //Закрываем его
                _duplicateScanWindow.Close();
            }
            //Завершаем работу сканера дубликатов
            _duplicateScannerFasade?.Dispose();
        }
    }
}
