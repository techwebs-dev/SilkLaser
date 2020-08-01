using System.Web.Mvc;
using SilkLaser.Web.Controllers;

namespace SilkLaser.Web.Classes.Secutiry
{
    /// <summary>
    /// Аспект, валидирующий авторизованность пользователя на выполнение действия
    /// </summary>
    public class AuthorizationCheckAttribute: ActionFilterAttribute
    {
		/// <summary>
		/// Пермишен, которым должен обладать пользователь чтобы пройти
		/// </summary>
	    public long RequiedPermission { get; set; }

	    /// <summary>
        /// Урл, куда происходит редирект если пользователь не авторизован
        /// </summary>
        public string RedirectUrl { get; private set; }

	    /// <summary>
	    /// Инициализирует новый атрибут, помещающий действие аспектом
	    /// </summary>
	    /// <param name="requiedPermission">Необходимый пермишен для доступа</param>
	    /// <param name="redirectUrl">Урл, куда редиректить неавторизованного пользователя</param>
	    public AuthorizationCheckAttribute(long requiedPermission = -1 ,string redirectUrl = "/manage/login")
	    {
		    RequiedPermission = requiedPermission;
		    RedirectUrl = redirectUrl;
	    }

	    /// <summary>
        /// Фильтруем действие перед его выполнение
        /// </summary>
        /// <param name="filterContext">Контекст действия</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentUser = (filterContext.Controller as BaseController)?.CurrentUser;
            if (currentUser == null)
            {
                filterContext.Result = new RedirectResult(this.RedirectUrl);
            } 
            else
            {
				base.OnActionExecuting(filterContext);
			}
        }
    }
}