using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SilkLaser.Web.Classes.DAL;
using SilkLaser.Web.Classes.Entities;
using SilkLaser.Web.Classes.Interfaces.Queue;
using SilkLaser.Web.Classes.IoC;
using SilkLaser.Web.Classes.Navigation;
using SilkLaser.Web.Classes.Notifications.Templates;
using SilkLaser.Web.Classes.Queue;
using SilkLaser.Web.Classes.UI;

namespace SilkLaser.Web.Controllers
{
	/// <summary>
	/// Базовый контроллер
	/// </summary>
    public abstract class BaseController : Controller
    {
		/// <summary>
		/// Контекст доступа к данным
		/// </summary>
		public SilkLaserDataContext DataContext { get; private set; }

		/// <summary>
		/// Навигационная цепочка
		/// </summary>
		public IList<NavigationChainItem> NavigationChain { get; private set; }

		/// <summary>
		/// Добавляет элемент в навигационную цепочку
		/// </summary>
		/// <param name="title">Название элемента</param>
		/// <param name="href">Ссылка</param>
		/// <param name="description">Описание</param>
		public void PushNavigationItem(string title, string href = "#", string description = null)
		{
			NavigationChain.Add(new NavigationChainItem()
			{
				Title = title,
				Description = description,
				Url = href,
				Active = href == "#"
			});
		}

		/// <summary>
		/// Хранение текущего пользователя
		/// </summary>
		private User _user { get; set; }

		/// <summary>
		/// Текущий авторизованный пользователь
		/// </summary>
		public User CurrentUser
		{
			get
			{
				object fromSess = Session["CurrentUser"];
				if (fromSess == null)
				{
					return null;
				}
				var userId = (long)fromSess;
				if (_user == null)
				{
					_user = DataContext.Users.FirstOrDefault(u => u.Id == userId);
					if (_user.Timezone == null)
					{
						_user.Timezone = "Vladivostok Standart Time";
						DataContext.SubmitChanges();
					}
				}
				return _user;
			}
			set
			{
				Session["CurrentUser"] = value != null ? (object)value.Id : null;
				_user = value;
			}
		}

		/// <summary>
		/// Является ли текущий пользователь авторизованным
		/// </summary>
		public bool IsAuthentificated
		{
			get { return CurrentUser != null; }
		}

		/// <summary>
		/// Убирает авторизацию текущего пользователя и убирает авторизационные куки если они есть
		/// </summary>
		public void CloseAuthorization()
		{
			CurrentUser = null;

			// убираем куки если они есть
			var authCookie = Response.Cookies["auth"];
			if (authCookie != null)
			{
				authCookie = new HttpCookie("auth")
				{
					Expires = DateTime.Now.AddDays(-1)
				};
				Response.Cookies.Add(authCookie);
			}
		}

		/// <summary>
		/// Авторизирует текущего пользователя
		/// </summary>
		/// <param name="user">Пользователь которого установить как текущего</param>
		/// <param name="remember">Запомнить ли пользователя</param>
		public void AuthorizeUser(User user, bool remember = true)
		{
			CurrentUser = user;
			if (remember)
			{
				// Устанавливаем собственные авторизационные куки
				var authCookie = new HttpCookie("auth");
				authCookie.Values["identity"] = user.Email;
				authCookie.Values["pass"] = user.PasswordHash;
				authCookie.Expires = DateTime.Now.AddDays(365);
				Response.Cookies.Add(authCookie);
			}
			CurrentUser.LastSeen = DateTime.Now.ToUniversalTime();
		}
		/// <summary>
		/// Отображает сообщение об ошибке
		/// </summary>
		/// <param name="message">Сообщение</param>
		public void ShowError(string message)
		{
			new UINotificationManager().Error(message);
		}

		/// <summary>
		/// Отображает сообщение об успешном выполнении операции
		/// </summary>
		/// <param name="message"></param>
		public void ShowSuccess(string message)
		{
			new UINotificationManager().Success(message);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
		/// </summary>
		public BaseController()
		{
			DataContext = Locator.GetService<SilkLaserDataContext>();
			NavigationChain = new List<NavigationChainItem>();
			QueueManager = Locator.GetService<IQueueManager>();
		}

		/// <summary>
		/// Абстрактный менеджер очередей
		/// </summary>
		public IQueueManager QueueManager { get; set; }

		/// <summary>
		/// Возвращает результат в виде JSON.net сериализованного объекта
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public ActionResult JsonNet(object data)
		{
			return Content(JsonConvert.SerializeObject(data, new IsoDateTimeConverter()), "application/json");
		}

		/// <summary>
		/// Уведомляем указанного пользователя Email сообщением
		/// </summary>
		/// <param name="user">Пользователь</param>
		/// <param name="subject">Тема сообщения</param>
		/// <param name="content">Содержимое</param>
		public void NotifyEmail(User user, string subject, string content)
		{
			QueueManager.SendEmail(user.Email, subject, content);
		}
	}
}