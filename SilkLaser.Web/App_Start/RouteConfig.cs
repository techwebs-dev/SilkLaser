using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SilkLaser.Web.Classes.DAL;

namespace SilkLaser.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.Clear();

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapMvcAttributeRoutes();

			// Регистрируем зависимости
			var dc = new SilkLaserDataContext();
			foreach (var page in dc.HtmlPages)
			{
				var route = page.Route;
				if (route.StartsWith("/"))
				{
					route = route.Substring(1);
				}
				routes.MapRoute(
					name: String.Format("Page {0}", page.Id),
					url: route,
					defaults: new { controller = "Main", action = "ViewPage", id = page.Id }
				);
			}

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}