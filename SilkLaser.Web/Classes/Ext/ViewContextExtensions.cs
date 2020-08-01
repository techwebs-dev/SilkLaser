using System.Web.Mvc;
using SilkLaser.Web.Classes.DAL;
using SilkLaser.Web.Classes.Entities;
using SilkLaser.Web.Controllers;

namespace SilkLaser.Web.Classes.Ext
{
    /// <summary>
    /// Статический класс с расширениями вью
    /// </summary>
    public static class ViewContextExtensions
    {
        /// <summary>
        /// Текущий аутентифицированный пользователь
        /// </summary>
        /// <param name="viewContext">Контекст вью</param>
        /// <returns>Объект пользователя</returns>
        public static User CurrentUser(this ViewContext viewContext)
        {
            var baseController = viewContext.Controller as BaseController;
	        return baseController?.CurrentUser;
        }

        /// <summary>
        /// Проверяет аутентифицирован ли текущий пользователь
        /// </summary>
        /// <param name="viewContext">контекст вью</param>
        /// <returns>true если да, иначе false</returns>
        public static bool IsAuthentificated(this ViewContext viewContext)
        {
            var baseController = viewContext.Controller as BaseController;
	        return baseController?.CurrentUser != null;
        }

		/// <summary>
		/// Возвращает текущий контекст для доступа к данным
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
	    public static SilkLaserDataContext DataContext(this ViewContext viewContext)
	    {
			var baseController = viewContext.Controller as BaseController;
		    return baseController?.DataContext;
	    }
    }
}