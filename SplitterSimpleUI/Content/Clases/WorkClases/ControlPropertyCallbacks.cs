using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;

namespace SplitterSimpleUI.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс обработных вызовов, связанных со свойствами контроллов
    /// </summary>
    public static class ControlPropertyCallbacks
    {

        /// <summary>
        /// Обработчик события изменения свойства через Binding
        /// </summary>
        /// <param name="depObj">Объект, для которого изменяем свойство</param>
        /// <param name="e">Описание нового значения свойства</param>
        public static void ControlPropertyChangedNew(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            //Получаем изменённое свойство зависимостей
            DependencyProperty depProp = e.Property;
            //Получаем тип изменяемого объекта
            Type ownerType = depProp.OwnerType;
            //Получаем имя метода обновления контента
            string setterName = $"Set{depProp.Name}";
            //Получаем сам метод обновления контента для данного свойства
            MethodInfo setter = ownerType.GetMethod(setterName);
            //Вызываем метод обновления контента
            setter.Invoke(depObj, new object[] { e.NewValue });
        }
    }
}
