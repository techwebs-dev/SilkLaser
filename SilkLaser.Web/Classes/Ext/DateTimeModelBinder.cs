// ============================================================
// 
// 	Asgard
// 	Asgard.Web.Public 
// 	DateTimeModelBinder.cs
// 
// 	Created by: ykorshev 
// 	 at 04.08.2013 22:00
// 
// ============================================================

using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;

namespace SilkLaser.Web.Classes.Ext
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Привязывает указанное свойство, используя заданные контекст контроллера и контекст привязки, а также заданный дескриптор свойства.
        /// </summary>
        /// <param name="controllerContext">Контекст, в котором функционирует контроллер.Сведения о контексте включают информацию о контроллере, HTTP-содержимом, контексте запроса и данных маршрута.</param><param name="bindingContext">Контекст, в котором привязана модель.Контекст содержит такие сведения, как объект модели, имя модели, тип модели, фильтр свойств и поставщик значений.</param><param name="propertyDescriptor">Описывает свойство, которое требуется привязать.Дескриптор предоставляет информацию, такую как тип компонента, тип свойства и значение свойства.Также предоставляет методы для получения или задания значения свойства.</param>
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType == typeof(DateTime))
            {
                var submittedVal = controllerContext.HttpContext.Request[propertyDescriptor.Name];
                if (!String.IsNullOrEmpty(submittedVal))
                {
                    DateTime val = DateTime.MinValue;
                    try
                    {
                        val = Convert.ToDateTime(submittedVal);
                    }
                    catch (Exception)
                    {
                    }
                    propertyDescriptor.SetValue(bindingContext.Model, val);
                }
            }
            else if (propertyDescriptor.PropertyType == typeof(DateTime?))
            {
                var submittedVal = controllerContext.HttpContext.Request[propertyDescriptor.Name];
                if (!String.IsNullOrEmpty(submittedVal))
                {
                    DateTime? val = null;
                    try
                    {
                        val = Convert.ToDateTime(controllerContext.HttpContext.Request[propertyDescriptor.Name]);
                    }
                    catch (Exception)
                    {
                        val = null;
                    }
                    propertyDescriptor.SetValue(bindingContext.Model, val);
                }
            } else
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }
        }
    }

	public class CustomDateBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			if (controllerContext == null)
				throw new ArgumentNullException("controllerContext", "controllerContext is null.");
			if (bindingContext == null)
				throw new ArgumentNullException("bindingContext", "bindingContext is null.");

			var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

			if (value == null)
				throw new ArgumentNullException(bindingContext.ModelName);

			CultureInfo cultureInf = (CultureInfo)CultureInfo.CurrentCulture.Clone();
			cultureInf.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

			bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

			try
			{
				var date = value.ConvertTo(typeof(DateTime), cultureInf);

				return date;
			}
			catch (Exception ex)
			{
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
				return null;
			}
		}
	}

	public class NullableCustomDateBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			if (controllerContext == null)
				throw new ArgumentNullException("controllerContext", "controllerContext is null.");
			if (bindingContext == null)
				throw new ArgumentNullException("bindingContext", "bindingContext is null.");

			var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

			if (value == null) return null;

			CultureInfo cultureInf = (CultureInfo)CultureInfo.CurrentCulture.Clone();
			cultureInf.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

			bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

			try
			{
				var date = value.ConvertTo(typeof(DateTime), cultureInf);

				return date;
			}
			catch (Exception ex)
			{
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
				return null;
			}
		}
	}
}