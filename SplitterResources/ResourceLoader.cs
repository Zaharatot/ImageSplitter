using SplitterResources.Content.Clases.DataClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SplitterResources
{
    /// <summary>
    /// Класс загрузки ресурса по идентификатору
    /// </summary>
    public static class ResourceLoader
    {
        /// <summary>
        /// Загружаем из ресурсов значение строки по id
        /// </summary>
        /// <param name="id">Id элемента</param>
        /// <param name="addText">Текст для вставки в сообщение</param>
        /// <returns>Искомое значение</returns>
        public static string LoadString(string id, string[] addText = null) =>
            //Получаем ресурс по Id
            addText != null
                //Получаем ресурс по Id
                ? string.Format((string)Application.Current.Resources[id], addText)
                : (string)Application.Current.Resources[id];

        /// <summary>
        /// Загружаем из ресурсов значение кисти по id
        /// </summary>
        /// <param name="id">Id элемента</param>
        /// <returns>Искомое значение</returns>
        public static SolidColorBrush LoadBrush(string id) =>
            //Получаем ресурс по Id
            (SolidColorBrush)Application.Current.Resources[id];


        /// <summary>
        /// Загружаем из ресурсов значение стиля по id
        /// </summary>
        /// <param name="id">Id элемента</param>
        /// <returns>Искомое значение</returns>
        public static Style LoadStyle(string id) =>
            //Получаем ресурс по Id
            (Style)Application.Current.Resources[id];

        /// <summary>
        /// Загружаем из ресурсов значение иконки по id
        /// </summary>
        /// <param name="id">Id элемента</param>
        /// <returns>Искомое значение</returns>
        public static SvgImage LoadIcon(string id) =>
            //Получаем ресурс по Id
            (SvgImage)Application.Current.Resources[id];
    }
}
