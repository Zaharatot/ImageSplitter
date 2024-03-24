﻿using ImageSplitter.Content.Clases.WorkClases.Resources;
using ImageSplitter.Content.Controls.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ImageSplitter.Content.Clases.WorkClases.Addition
{
    /// <summary>
    /// Класс обработки выделения иконок
    /// </summary>
    internal class IconsSelectionProcessor
    {
        /// <summary>
        /// Экземпляр класса для синглтона
        /// </summary>
        private static IconsSelectionProcessor _instance;


        /// <summary>
        /// Дефолтный цвет иконки
        /// </summary>
        private SolidColorBrush _defaultColor;
        /// <summary>
        /// Выделенный цвет иконки
        /// </summary>
        private SolidColorBrush _selectedColor;
        /// <summary>
        /// Отключенный цвет иконки
        /// </summary>
        private SolidColorBrush _disabledColor;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public IconsSelectionProcessor()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Грузим цвета для иконок
            _defaultColor = ResourceLoader.LoadBrush("Brush_ForegroundColor");
            _selectedColor = ResourceLoader.LoadBrush("Brush_ActiveColor");
            _disabledColor = ResourceLoader.LoadBrush("Brush_ForegroundDisabledColor");
        }


        /// <summary>
        /// Обработчик события ухода курсора с иконки
        /// </summary>
        private void Icon_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) 
        {
            //Получаем иконку
            SvgImageControl icon = sender as SvgImageControl;
            //Сбрасываем цвет иконки на дефолтный, или ставим отключенный
            icon.FillColor = (icon.IsEnabled) ? _defaultColor : _disabledColor;
        }

        /// <summary>
        /// Обработчик события ухода курсора с иконки
        /// </summary>
        private void Icon_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //Получаем иконку
            SvgImageControl icon = sender as SvgImageControl;
            //Ставим выделенный цвет иконки, или ставим отключенный
            icon.FillColor = (icon.IsEnabled) ? _selectedColor : _disabledColor;
        }

        /// <summary>
        /// Обработчик события включения/выключения иконки
        /// </summary>
        private void Icon_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Получаем иконку
            SvgImageControl icon = sender as SvgImageControl;
            //Сбрасываем цвет иконки на дефолтный, или ставим отключенный
            icon.FillColor = (icon.IsEnabled) ? _defaultColor : _disabledColor;
        }



        /// <summary>
        /// Метод добавления иконок для обработки
        /// </summary>
        /// <param name="icons">Список иконок для обработки</param>
        public void AddIcons(List<SvgImageControl> icons) 
        { 
            //Проходимся по переданных иконкам
            foreach(SvgImageControl icon in icons)
            {
                //Добавляем обработчик события наведения курсора на иконку
                icon.MouseEnter += Icon_MouseEnter;
                //Добавляем обработчик события ухода курсора с иконки
                icon.MouseLeave += Icon_MouseLeave;
                //Добавляем обработчик события включения/выключения иконки
                icon.IsEnabledChanged += Icon_IsEnabledChanged;
                //Сбрасываем цвет иконки на дефолтный, или ставим отключенный
                icon.FillColor = (icon.IsEnabled) ? _defaultColor : _disabledColor;
            }
        }



        /// <summary>
        /// Метод получения экземпляра класса дял синглтона
        /// </summary>
        /// <returns>Уникальный экземпляр класса</returns>
        public static IconsSelectionProcessor GetInstance()
        {
            //Если уникального экземпляра ещё нет
            if(_instance == null)
                //Инициализируем его
                _instance = new IconsSelectionProcessor();
            //Возвращаем уникальный экземпляр класса
            return _instance;
        }
    }
}
