using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using SilkLaser.Web.Classes.DAL;
using SilkLaser.Web.Classes.IoC;
using SilkLaser.Web.Classes.Workers;

namespace SilkLaser.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			Locator.Init(new SilkLaserModule());
			Locator.GetService<DBQueueWorker>().Init();

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		/// <summary>
		/// Начала сессии пользователя
		/// </summary>
		protected void Session_Start()
		{
			// Контекст
			var context = HttpContext.Current;

			// Ищем авторизационную куку
			var authCookie = context.Request.Cookies["auth"];
			if (authCookie != null)
			{
				var identity = authCookie["identity"];
				var pass = authCookie["pass"];

				var dc =
					new SilkLaserDataContext();
				var user = dc.Users.FirstOrDefault(u => u.Email == identity && u.PasswordHash == pass);
				if (user != null)
				{
					context.Session["CurrentUser"] = user.Id;
					user.LastSeen = DateTime.Now.ToUniversalTime();
					dc.SubmitChanges();
				}
				else
				{
					context.Response.Cookies.Add(new HttpCookie("auth")
					{
						Expires = DateTime.Now.AddDays(-1)
					});
				}
			}
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

			CultureInfo cInf = new CultureInfo("ru-RU", false);
			// NOTE: change the culture name en-ZA to whatever culture suits your needs

			cInf.DateTimeFormat.DateSeparator = ".";
			cInf.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
			cInf.DateTimeFormat.LongDatePattern = "dd.MM.yyyy hh:mm:ss";

			System.Threading.Thread.CurrentThread.CurrentCulture = cInf;
			System.Threading.Thread.CurrentThread.CurrentUICulture = cInf;
		}
	}
}