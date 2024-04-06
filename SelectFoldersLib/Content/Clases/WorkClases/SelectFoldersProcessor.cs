using SelectFoldersWindowLib.Content.Windows;
using SplitterDataLib.DataClases.Global.Split;
using SplitterSimpleUI.Content.Clases.WorkClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SelectFoldersWindowLib.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс работы с окном выбора папок для сплита
    /// </summary>
    internal class SelectFoldersProcessor
    {


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SelectFoldersProcessor()
        {

        }



        /// <summary>
        /// Метод запуска выбора папок
        /// </summary>
        /// <param name="folders">Список папок для обработки</param>
        /// <returns>Список выбранных папок</returns>
        public List<TargetFolderInfo> SelectFolders(List<TargetFolderInfo> folders)
        {
            //Инициализируем окно выбора папок
            SelectFoldersWindow foldersWindow = new SelectFoldersWindow();
            //Обновляем список папок в окне
            foldersWindow.UpdateFoldersList(folders);
            //Отображаем окно как диалоговое, и если оно было успешно закрыто
            if (foldersWindow.ShowDialog().GetValueOrDefault(false))
            {
                //Обновляем выделение в папках по контроллу
                foldersWindow.GetSelectedFolders(ref folders);
                //Удаляем все не выделенные папки
                folders.RemoveAll(folder => !folder.IsSelected);
                //Возвращаем список выбранных папок
                return folders;
            }
            //В противном случае - возвращаем пустой список
            return new List<TargetFolderInfo>();
        }
    }
}
