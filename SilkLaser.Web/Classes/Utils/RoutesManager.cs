using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace SilkLaser.Web.Classes.Utils
{
    /// <summary>
    /// Статический менеджер роутов
    /// </summary>
    public static class RoutesManager
    {
        /// <summary>
        /// Регистрирует указанный роут с указанными параметрами маппинга
        /// </summary>
        /// <param name="name"></param>
        /// <param name="route">Строковый путь роута</param>
        /// <param name="routeMapping">Маппинг</param>
        /// <param name="insertFirst">Вставить ли роут самым первым в таблице роутов</param>
        public static void RegisterRoute(string name, string route, object routeMapping, bool insertFirst = false)
        {
            var newRoute = RouteTable.Routes.MapRoute(name, route, routeMapping);
            if (insertFirst)
            {
                RouteTable.Routes.Remove(newRoute);
                RouteTable.Routes.Insert(0,newRoute);
            }
        }

        /// <summary>
        /// Удаляет указанный роут из таблицы роутов
        /// </summary>
        /// <param name="route">Роут</param>
        public static void RemoveRoute(string route)
        {
            /*var findedRoute = RouteTable.Routes.Cast<Route>().FirstOrDefault(r => r.Url.ToLower() == route.ToLower());
            if (findedRoute != null)
            {
                RouteTable.Routes.Remove(findedRoute);
            }*/
			// TODO: починить
        }

        /// <summary>
        /// Обновляет указанный роут на новое значение
        /// </summary>
        /// <param name="oldRoute">старый роут</param>
        /// <param name="newRoute">новый роут</param>
        public static void UpdateRoute(string oldRoute, string newRoute)
        {
            /*var findedRoute = RouteTable.Routes.Cast<Route>().FirstOrDefault(r => r.Url.ToLower() == oldRoute.ToLower());
            if (findedRoute != null)
            {
                findedRoute.Url = newRoute;
            }*/
			// TODO починить
        }
    }
}