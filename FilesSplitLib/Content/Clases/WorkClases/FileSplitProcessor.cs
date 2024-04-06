using FilesSplitWindowLib.Content.Clases.DataClases;
using FilesSplitWindowLib.Content.Windows;
using SplitterDataLib.DataClases.Global.Split;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesSplitWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Основной класс выполнения сплита файлов
    /// </summary>
    internal class FileSplitProcessor
    {
        /// <summary>
        /// Класс выполнения сплита
        /// </summary>
        private FilesSplit _fileSplit;
        /// <summary>
        /// Класс выполнения возврата
        /// </summary>
        private FilesReturn _filesReturn;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileSplitProcessor()
        {
            Init();
        }

        private void Init()
        {
            //Инициализируем используемые классы
            _fileSplit = new FilesSplit();
            _filesReturn = new FilesReturn();
        }

        /// <summary>
        /// Метод создания информации о перемщении
        /// </summary>
        /// <param name="filesSplitWindow">Окно параметров перемщения</param>
        /// <param name="info">Информация о путях для сплита</param>
        /// <returns>Класс информации о перемщении</returns>
        private MoveFilesInfo CreateMoveInfo(FilesSplitWindow filesSplitWindow, SplitPathsInfo info) =>
            new MoveFilesInfo(
                filesSplitWindow.CountSplitFiles,
                new DirectoryInfo(info.ScanPath),
                filesSplitWindow.IsChildSplit
            );


        /// <summary>
        /// Метод выполнения сплита файлов
        /// </summary>
        /// <param name="info">Информация о путях для сплита</param>
        public void SplitFiles(SplitPathsInfo info)
        {
            //Инициализируем окно сплита файлов
            FilesSplitWindow filesSplitWindow = new FilesSplitWindow();
            //Если окно закрылось успехом
            if (filesSplitWindow.ShowDialog().GetValueOrDefault(false))
            {
                //Инициализируем класс информации о перемещении
                MoveFilesInfo moveInfo = CreateMoveInfo(filesSplitWindow, info);
                //Если был запрошен сплит
                if (filesSplitWindow.IsSplit)
                    //Вызываем метод сплита
                    _fileSplit.StartSplit(moveInfo);
                //Если был запрошен возврат
                else
                    //Вызываем метод возврата, передав путь
                    _filesReturn.StartReturn(moveInfo);
            }
        }
    }
}
