using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ImageSplitter.Content.Clases.DataClases.Controls
{
    /// <summary>
    /// Класс векторного изображения
    /// </summary>
    public class SvgImage
    {
        /// <summary>
        /// Список фигур иконки
        /// </summary>
        public List<Geometry> PathList { get; set; }
        /// <summary>
        /// Кисть для заливки
        /// </summary>
        public SolidColorBrush FillColor { get; set; }
        /// <summary>
        /// Карандаш для рамки
        /// </summary>
        public SolidColorBrush BorderColor { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SvgImage()
        {
            //Проставляем дефолтные значения
            PathList = new List<Geometry>();
            FillColor = BorderColor = null;
        }

    }
}
