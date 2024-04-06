using SplitterResources.Content.Clases.DataClases;
using SplitterSimpleUI.Content.Clases.WorkClases.Properties;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SplitterSimpleUI.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для SvgImageControl.xaml
    /// </summary>
    public partial class SvgImageControl : UserControl
    {
        /// <summary>
        /// Свойство зависимостей для отрисовываемой картинки
        /// </summary>
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(SvgImage), typeof(SvgImageControl),
                new UIPropertyMetadata(
                    null,
                    new PropertyChangedCallback(ControlPropertyCallbacks.ControlPropertyChangedNew)
                ));
        /// <summary>
        /// Отрисовываемая картинка
        /// </summary>
        public SvgImage Image
        {
            get => (SvgImage)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }


        /// <summary>
        /// Свойство зависимостей для цвета заливки
        /// </summary>
        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.Register("FillColor", typeof(SolidColorBrush), typeof(SvgImageControl),
                new UIPropertyMetadata(
                    null,
                    new PropertyChangedCallback(ControlPropertyCallbacks.ControlPropertyChangedNew)
                ));
        /// <summary>
        /// Цвет заливки
        /// </summary>
        public SolidColorBrush FillColor
        {
            get => (SolidColorBrush)GetValue(FillColorProperty);
            set => SetValue(FillColorProperty, value);
        }

        /// <summary>
        /// Свойство зависимостей для цвета рамки
        /// </summary>
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(SolidColorBrush), typeof(SvgImageControl),
                new UIPropertyMetadata(
                    null,
                    new PropertyChangedCallback(ControlPropertyCallbacks.ControlPropertyChangedNew)
                ));
        /// <summary>
        /// Цвет рамки
        /// </summary>
        public SolidColorBrush BorderColor
        {
            get => (SolidColorBrush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }


        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public SvgImageControl()
        {
            InitializeComponent();
        }




        /// <summary>
        /// Получаем карандаш по кисти
        /// </summary>
        /// <returns>Созданный класс карандаша</returns>
        public Pen GetPen(SolidColorBrush brush) =>
            new Pen(brush, 1);


        /// <summary>
        /// Метод обновления цветов
        /// </summary>
        /// <param name="image">Изображение для получения базовых цветов</param>
        /// <param name="borderColor">Цвет рамки</param>
        /// <param name="fillColor">Цвет заливки</param>
        /// <param name="borderPen">Карандаш для отрисовки рамки</param>
        private void UpdateColors(SvgImage image, SolidColorBrush borderColor, ref SolidColorBrush fillColor, out Pen borderPen)
        {
            //Берём или переданный цвет заливки (если он есть) или - цвет из класса
            fillColor = fillColor ?? image.FillColor;
            //Берём или переданный цвет рамки (если он есть) или - цвет из класса
            borderColor = borderColor ?? image.BorderColor;
            //Если цвет обводки есть - получаем карандаш
            borderPen = (borderColor == null) ? null : GetPen(borderColor);
        }


        /// <summary>
        /// Выполняем окрашивание иконки
        /// </summary>
        /// <param name="image">Изображение для окрашивания</param>
        /// <param name="borderColor">Цвет рамки</param>
        /// <param name="fillColor">Цвет заливки</param>
        /// <returns>Раскрашенная иконка</returns>
        private ImageSource Colorize(SvgImage image, SolidColorBrush fillColor, SolidColorBrush borderColor)
        {
            //Если иконки нет
            if (image != null)
            {
                //Получаем цвета, с учётом переданных
                UpdateColors(image, borderColor, ref fillColor, out Pen borderPen);
                //Инициализируем новую коллекцию частей
                DrawingCollection colorizedCollection = new DrawingCollection();
                //Проходимся по существующим частям
                foreach (Geometry path in image.PathList)
                    //Добавляем в раскрашенную коллекцию часть указав новый цвет заливки
                    colorizedCollection.Add(new GeometryDrawing(fillColor, borderPen, path));
                //Инициализируем новое изображение
                return new DrawingImage(new DrawingGroup() {
                    Children = colorizedCollection
                });
            }
            //В остальных случаях вернём null
            return null;
        }





        /// <summary>
        /// Выставление свойства при изменении свойства зависимостей FillColor
        /// </summary>
        /// <param name="fillColor">Цвет заливки</param>
        public void SetFillColor(SolidColorBrush fillColor) =>
            //Формируем картинку из фигур, красим (если нужно), и возвращаем.
            MainImage.Source = Colorize(Image, fillColor, BorderColor);

        /// <summary>
        /// Выставление свойства при изменении свойства зависимостей BorderColor
        /// </summary>
        /// <param name="borderColor">Цвет рамки</param>
        public void SetBorderColor(SolidColorBrush borderColor) =>
            //Формируем картинку из фигур, красим (если нужно), и возвращаем.
            MainImage.Source = Colorize(Image, FillColor, borderColor);

        /// <summary>
        /// Выставление свойства при изменении свойства зависимостей Image
        /// </summary>
        /// <param name="image">Новое изображение</param>
        public void SetImage(SvgImage image) =>
            //Формируем картинку из фигур, красим (если нужно), и возвращаем.
            MainImage.Source = Colorize(image, FillColor, BorderColor);
    }
}
