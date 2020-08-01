using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using SilkLaser.Web.Classes.Entities;
using SilkLaser.Web.Classes.Enums;
using SilkLaser.Web.Classes.Secutiry;
using SilkLaser.Web.Classes.Utils;
using SilkLaser.Web.Models;

namespace SilkLaser.Web.Controllers
{
	/// <summary>
	/// Контроллер с функциями администрирования сайта
	/// </summary>
	[RoutePrefix("manage")]
	public class ManageController : BaseController
	{
		#region Сводка

		/// <summary>
		/// Отображает дашбоард сводной статистики
		/// </summary>
		/// <returns></returns>
		[Route("dashboard")]
		[AuthorizationCheck()]
		public ActionResult Dashboard()
		{
			PushNavigationItem("Сводка");

			return View();
		}

		/// <summary>
		/// Возвращает аяксовый вид, в котором происходит запрос к Яндекс Метрике и возвращает разметка с графиком
		/// </summary>
		/// <returns></returns>
		[Route("dashboard/visits")]
		[AuthorizationCheck()]
		[OutputCache(Duration = 600)]
		public ActionResult DashboardVisits()
		{
			// Данные авторизации
			var metrikaCounter = ConfigurationManager.AppSettings["MetrikaCounter"];
			var metrikaToken = ConfigurationManager.AppSettings["MetrikaToken"];

			// Данные выборки
			var periodStart = DateTime.Now.AddDays(-14);
			var periodEnd = DateTime.Now;

			// URL обращения
			var url =
				$"https://api-metrika.yandex.ru/stat/traffic/summary.json?id={metrikaCounter}&pretty=1&oauth_token={metrikaToken}&group=day&date1={periodStart:yyyyMMdd}&date2={periodEnd:yyyyMMdd}"; // Api v1.0

			url = $"https://api-metrika.yandex.ru/stat/v1/data?ids={metrikaCounter}&pretty=1&oauth_token={metrikaToken}&group=day&date1={periodStart:yyyy-MM-dd}&date2={periodEnd:yyyy-MM-dd}&preset=traffic&sort=ym:s:datePeriodday";

			// Скачиваем
			var client = new WebClient()
			{
				Encoding = Encoding.UTF8
			};
			var response = client.DownloadString(url);

			// Выполняем парсинг
			var json = JObject.Parse(response);
			var dataArray = (JArray)json.Property("data").Value;

			var model = new DashboardVisitsStatisticsModel(periodStart, periodEnd);
			foreach (JObject dataRow in dataArray)
			{
				/*var pageViews = dataRow.Property("page_views").Value.Value<int>();
				var visits = dataRow.Property("visits").Value.Value<int>();
				var visitors = dataRow.Property("visitors").Value.Value<int>();
				var newVisitors = dataRow.Property("new_visitors").Value.Value<int>();
				var time = dataRow.Property("visit_time").Value.Value<int>();
				var depth = dataRow.Property("depth").Value.Value<double>();
				var denial = dataRow.Property("denial").Value.Value<double>();
				var dateStr = dataRow.Property("date").Value.Value<string>();
				var date = DateTime.ParseExact(dateStr, "yyyyMMdd", CultureInfo.CurrentCulture);*/

				var metrics = (JArray)dataRow.Property("metrics").Value;
				var dimensions = (JArray)dataRow.Property("dimensions").Value;
				var dateStr = (dimensions[0] as JObject).Property("name").Value.Value<string>();
				var date = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.CurrentCulture);
				var visits = metrics[0].Value<int>();
				var visitors = metrics[1].Value<int>();
				var pageViews = metrics[2].Value<int>();

				model.AddDataItem(date, pageViews, visits, visitors, 0, 0, 0, 0);
			}

			return PartialView(model);
		}

		#endregion

		#region Админская авторизация и выход

		/// <summary>
		/// Отображает страницу авторизации в системе админского управления
		/// </summary>
		/// <returns></returns>
		[Route("login")]
		public ActionResult Login()
		{
			return View();
		}

		/// <summary>
		/// Обрабатывает авторизацию пользователя
		/// </summary>
		/// <param name="email">Email пользователя</param>
		/// <param name="password">Пароль</param>
		/// <param name="remember">Запомнить меня</param>
		/// <returns></returns>
		[Route("do-login")]
		[HttpPost]
		public ActionResult DoLogin(string email, string password, bool remember)
		{
			var hash = PasswordUtils.GenerateMD5PasswordHash(password);
			var user =
				DataContext.Users.FirstOrDefault(u => u.Email.ToLower() == (email ?? "").ToLower() && u.PasswordHash == hash);

			if (user == null)
			{
				ShowError("Такой пользователь не найден");

				return RedirectToAction("Login");
			}

			// Авторизуем пользователя
			AuthorizeUser(user, remember);
			DataContext.SubmitChanges();

			return RedirectToAction("Dashboard");
		}

		/// <summary>
		/// Обрабатывает выход пользователя из текущего аккаунта
		/// </summary>
		/// <returns></returns>
		[Route("logout")]
		[AuthorizationCheck()]
		public ActionResult Logout()
		{
			CloseAuthorization();

			return RedirectToAction("Login");
		}

		#endregion

		#region HTML страницы

		/// <summary>
		/// Отображает список созданных в системе HTML страниц
		/// </summary>
		/// <returns></returns>
		[Route("html-pages")]
		[AuthorizationCheck()]
		public ActionResult HtmlPages()
		{
			var pages = DataContext.HtmlPages.OrderBy(c => c.Title).ToList();

			PushNavigationItem("Контент");
			PushNavigationItem("Html страницы");

			return View(pages);
		}

		/// <summary>
		/// Отображает форму создания новой страницы
		/// </summary>
		/// <returns></returns>
		[Route("html-pages/new")]
		[AuthorizationCheck()]
		public ActionResult AddHtmlPage()
		{
			PushNavigationItem("Контент");
			PushNavigationItem("Html страницы");
			PushNavigationItem("Новая страница");

			return View("EditHtmlPage", new HtmlPage());
		}

		/// <summary>
		/// Отображает форму редактирования существующей html страницы
		/// </summary>
		/// <param name="id">Идентификатор страницы</param>
		/// <returns></returns>
		[Route("html-pages/{id}/edit")]
		[AuthorizationCheck()]
		public ActionResult EditHtmlPage(long id)
		{
			var page = DataContext.HtmlPages.FirstOrDefault(r => r.Id == id);
			if (page == null)
			{
				ShowError("Такая страница не найдена");
				return RedirectToAction("HtmlPages");
			}

			PushNavigationItem("Контент");
			PushNavigationItem("Html страницы");
			PushNavigationItem(page.Title);
			return View("EditHtmlPage", page);
		}

		/// <summary>
		/// Обрабатывает сохранение новой страницы или изменение в существующей странице
		/// </summary>
		/// <param name="model">Модель данных</param>
		/// <returns></returns>
		[Route("html-pages/save")]
		[HttpPost]
		[AuthorizationCheck()]
		[ValidateInput(false)]
		public ActionResult SaveHtmlPage(HtmlPage model)
		{
			if (model.Route.StartsWith("/"))
			{
				model.Route = model.Route.Substring(1);
			}

			if (model.Id <= 0)
			{
				model.DateCreated = DateTime.Now.ToUniversalTime();
				model.CreatedBy = CurrentUser.ToString();

				DataContext.HtmlPages.InsertOnSubmit(model);
				DataContext.SubmitChanges();

				ShowSuccess("Html страница была успешно создана");
			}
			else
			{
				// Ищем
				var page = DataContext.HtmlPages.FirstOrDefault(r => r.Id == model.Id);
				if (page == null)
				{
					ShowError("Html страница не найдена");
					return RedirectToAction("HtmlPages");
				}

				// Редактируем
				var oldRoute = page.Route;
				TryUpdateModel(page);
				page.DateModified = DateTime.Now.ToUniversalTime();
				page.ModifiedBy = CurrentUser.ToString();

				DataContext.SubmitChanges();

				if (oldRoute != page.Route)
				{
					RoutesManager.UpdateRoute(oldRoute, page.Route);
				}

				ShowSuccess("Html страница был успешно отредактирована");
			}

			// Регистрируем роут
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			return RedirectToAction("HtmlPages");
		}

		/// <summary>
		/// Обрабатывает удаление указанной страницы
		/// </summary>
		/// <param name="id">Идентификатор страницы</param>
		/// <returns></returns>
		[Route("html-pages/{id}/delete")]
		[AuthorizationCheck()]
		public ActionResult DeleteHtmlPage(long id)
		{
			var page = DataContext.HtmlPages.FirstOrDefault(r => r.Id == id);
			if (page == null)
			{
				ShowError("Такая страница не найдена");
				return RedirectToAction("HtmlPages");
			}

			DataContext.HtmlPages.DeleteOnSubmit(page);
			DataContext.SubmitChanges();

			ShowSuccess("Html страница была успешно удалена");

			// Удаляем роут
			RoutesManager.RemoveRoute(page.Route);

			return RedirectToAction("HtmlPages");
		}

		#endregion

		#region Цены

		/// <summary>
		/// Отображает список цен
		/// </summary>
		/// <returns></returns>
		[Route("prices")]
		[AuthorizationCheck()]
		public ActionResult Prices()
		{
			var prices = DataContext.Prices.ToList();

			PushNavigationItem("Администрирование");
			PushNavigationItem("Цены");

			return View("Prices",prices);
		}

		/// <summary>
		/// Отображает форму создания новой цены
		/// </summary>
		/// <returns></returns>
		[Route("prices/new")]
		[AuthorizationCheck()]
		public ActionResult AddPrices()
		{
			PushNavigationItem("Администрирование");
			PushNavigationItem("Цены");
			PushNavigationItem("Новая цена");

			return View("EditPrice", new Price());
		}

		/// <summary>
		/// Отображает форму редактирования существующей цены
		/// </summary>
		/// <param name="id">Идентификатор цены</param>
		/// <returns></returns>
		[Route("prices/{id}/edit")]
		[AuthorizationCheck()]
		public ActionResult EditPrice(long id)
		{
			var price = DataContext.Prices.FirstOrDefault(r => r.Id == id);
			if (price == null)
			{
				ShowError("Такой ценник не найден");
				return RedirectToAction("Prices");
			}

			PushNavigationItem("Администрирование");
			PushNavigationItem("Цены");
			PushNavigationItem(price.Title);

			return View("EditPrice", price);
		}

		/// <summary>
		/// Обрабатывает сохранение нового ценника или изменение в существующем ценнике
		/// </summary>
		/// <param name="model">Модель данных</param>
		/// <returns></returns>
		[Route("prices/save")]
		[HttpPost][ValidateInput(false)]
		[AuthorizationCheck()]
		public ActionResult SavePrice(Price model)
		{
			// Сохраняем картинку
			var file = Request.Files["Image"];
			string imageUrl = null;
			if (file != null && file.ContentLength > 0 && file.ContentType.ToLower().Contains("image"))
			{
				var fileName = Path.ChangeExtension(Path.GetRandomFileName(), ".png");
				var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Prices");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				var fullPath = Path.Combine(path, fileName);
				var image = new WebImage(file.InputStream);
				image.Resize(900, 900, true, true).Save(fullPath, "PNG");
				imageUrl = "/Files/Prices/" + fileName;
			}

			if (model.Id <= 0)
			{
				model.DateCreated = DateTime.Now.ToUniversalTime();
				model.CreatedBy = CurrentUser.ToString();
				model.Image = imageUrl;

				DataContext.Prices.InsertOnSubmit(model);
				DataContext.SubmitChanges();

				ShowSuccess("Ценник был успешно создан");
			}
			else
			{
				// Ищем
				var price = DataContext.Prices.FirstOrDefault(r => r.Id == model.Id);
				if (price == null)
				{
					ShowError("Ценник не найден");
					return RedirectToAction("Prices");
				}

				// Редактируем
				var oldImageUrl = price.Image;
				TryUpdateModel(price);
				price.DateModified = DateTime.Now.ToUniversalTime();
				price.ModifiedBy = CurrentUser.ToString();
				if (imageUrl != null)
				{
					price.Image = imageUrl;
				}
				else
				{
					price.Image = oldImageUrl;
				}

				DataContext.SubmitChanges();

				ShowSuccess("Ценник был успешно отредактирован");
			}

			return RedirectToAction("Prices");
		}

		/// <summary>
		/// Обрабатывает удаление указанного ценника
		/// </summary>
		/// <param name="id">Идентификатор ценника</param>
		/// <returns></returns>
		[Route("prices/{id}/delete")]
		[AuthorizationCheck()]
		public ActionResult DeletePrice(long id)
		{
			var price = DataContext.Prices.FirstOrDefault(r => r.Id == id);
			if (price == null)
			{
				ShowError("Такой ценник не найден");
				return RedirectToAction("Prices");
			}
			

			DataContext.Prices.DeleteOnSubmit(price);
			DataContext.SubmitChanges();

			ShowSuccess("Ценник был успешно удален");

			return RedirectToAction("Prices");
		}

		#endregion

		#region FAQ

		/// <summary>
		/// Отображает список вопросов
		/// </summary>
		/// <returns></returns>
		[Route("faq")]
		[AuthorizationCheck()]
		public ActionResult Faq()
		{
			var questions = DataContext.FaqQuestions.OrderBy(c => c.Position).ToList();

			PushNavigationItem("Администрирование");
			PushNavigationItem("Вопросы");

			return View("Faq",questions);
		}

		/// <summary>
		/// Отображает форму создания нового вопроса
		/// </summary>
		/// <returns></returns>
		[Route("faq/new")]
		[AuthorizationCheck()]
		public ActionResult AddFaq()
		{
			PushNavigationItem("Администрирование");
			PushNavigationItem("Вопросы");
			PushNavigationItem("Новый вопрос");

			return View("EditFaq", new FaqQuestion());
		}

		/// <summary>
		/// Отображает форму редактирования существующего вопроса
		/// </summary>
		/// <param name="id">Идентификатор вопроса</param>
		/// <returns></returns>
		[Route("faq/{id}/edit")]
		[AuthorizationCheck()]
		public ActionResult EditFaq(long id)
		{
			var question = DataContext.FaqQuestions.FirstOrDefault(r => r.Id == id);
			if (question == null)
			{
				ShowError("Такой вопрос не найден");
				return RedirectToAction("Faq");
			}

			PushNavigationItem("Администрирование");
			PushNavigationItem("Вопросы");
			PushNavigationItem(question.Question);

			return View("EditFaq", question);
		}

		/// <summary>
		/// Обрабатывает сохранение нового вопроса или изменение в существующем вопросе
		/// </summary>
		/// <param name="model">Модель данных</param>
		/// <returns></returns>
		[Route("faq/save")]
		[HttpPost][ValidateInput(false)]
		[AuthorizationCheck()]
		public ActionResult SaveFaq(FaqQuestion model)
		{
			if (model.Id <= 0)
			{
				model.DateCreated = DateTime.Now.ToUniversalTime();
				model.CreatedBy = CurrentUser.ToString();
				model.Position = DataContext.FaqQuestions.Select(p => p.Position).ToArray().DefaultIfEmpty(0).Max() + 1;

				DataContext.FaqQuestions.InsertOnSubmit(model);
				DataContext.SubmitChanges();

				ShowSuccess("Вопрос был успешно создан");
			}
			else
			{
				// Ищем
				var partner = DataContext.FaqQuestions.FirstOrDefault(r => r.Id == model.Id);
				if (partner == null)
				{
					ShowError("Вопрос не найден");
					return RedirectToAction("Faq");
				}

				// Редактируем
				TryUpdateModel(partner);
				partner.DateModified = DateTime.Now.ToUniversalTime();
				partner.ModifiedBy = CurrentUser.ToString();

				DataContext.SubmitChanges();

				ShowSuccess("Вопрос был успешно отредактирован");
			}

			return RedirectToAction("Faq");
		}

		/// <summary>
		/// Обрабатывает удаление указанного вопроса
		/// </summary>
		/// <param name="id">Идентификатор партнера</param>
		/// <returns></returns>
		[Route("faq/{id}/delete")]
		[AuthorizationCheck()]
		public ActionResult DeleteFaq(long id)
		{
			var question = DataContext.FaqQuestions.FirstOrDefault(r => r.Id == id);
			if (question == null)
			{
				ShowError("Такой вопрос не найден");
				return RedirectToAction("Faq");
			}

			DataContext.FaqQuestions.DeleteOnSubmit(question);
			DataContext.SubmitChanges();

			ShowSuccess("Вопрос был успешно удален");

			return RedirectToAction("Faq");
		}

		/// <summary>
		/// Обрабатывает изменение позиции вопроса
		/// </summary>
		/// <param name="faq">Идентификатор вопроса</param>
		/// <param name="position">Номер новой позиции</param>
		/// <returns></returns>
		[Route("faq/faq-position-change")]
		[AuthorizationCheck()]
		public ActionResult FaqPositionChange(long faq, int position)
		{
			try
			{
				var _question = DataContext.FaqQuestions.FirstOrDefault(r => r.Id == faq);
				if (_question == null)
				{
					throw new Exception("Партнер не найден");
				}

				var questions = DataContext.FaqQuestions.OrderBy(r => r.Position).ToList();
				_question = questions.Find(r => r.Id == faq);
				questions.Remove(_question);
				questions.Insert(position, _question);
				var counter = 0;
				foreach (var rm in questions)
				{
					counter++;
					rm.Position = counter;
					rm.DateModified = DateTime.Now.ToUniversalTime();
					rm.ModifiedBy = CurrentUser.ToString();
				}

				DataContext.SubmitChanges();

				return JsonNet(new
				{
					success = true
				});
			}
			catch (Exception e)
			{
				return JsonNet(new
				{
					success = false,
					message = e.Message
				});
			}
		}

		#endregion

		#region Отзывы

		/// <summary>
		/// Отображает список отзывов
		/// </summary>
		/// <returns></returns>
		[Route("reviews")]
		[AuthorizationCheck()]
		public ActionResult Reviews()
		{
			var reviews = DataContext.Reviews.OrderBy(c => c.Position).ToList();

			PushNavigationItem("Администрирование");
			PushNavigationItem("Отзывы");

			return View("Reviews", reviews);
		}

		/// <summary>
		/// Публикует отзыв на сайте
		/// </summary>
		/// <param name="id">Идентификатор отзыва</param>
		/// <returns></returns>
		[Route("reviews/{id}/publish")][AuthorizationCheck()]
		public ActionResult PublishReview(long id)
		{
			var review = DataContext.Reviews.FirstOrDefault(r => r.Id == id);
			if (review == null)
			{
				ShowError("Такой отзыв не найден");
				return RedirectToAction("Reviews");
			}

			review.Published = true;
			DataContext.SubmitChanges();

			ShowSuccess("Отзыв был успешно опубликован");

			return RedirectToAction("Reviews");
		}

		/// <summary>
		/// Прячет отзыв на сайте
		/// </summary>
		/// <param name="id">Идентификатор отзыва</param>
		/// <returns></returns>
		[Route("reviews/{id}/hide")][AuthorizationCheck()]
		public ActionResult HideReview(long id)
		{
			var review = DataContext.Reviews.FirstOrDefault(r => r.Id == id);
			if (review == null)
			{
				ShowError("Такой отзыв не найден");
				return RedirectToAction("Reviews");
			}

			review.Published = false;
			DataContext.SubmitChanges();

			ShowSuccess("Отзыв был успешно скрыт");

			return RedirectToAction("Reviews");
		}

		/// <summary>
		/// Обрабатывает удаление указанного вопроса
		/// </summary>
		/// <param name="id">Идентификатор партнера</param>
		/// <returns></returns>
		[Route("reviews/{id}/delete")]
		[AuthorizationCheck()]
		public ActionResult DeleteReview(long id)
		{
			var review = DataContext.Reviews.FirstOrDefault(r => r.Id == id);
			if (review == null)
			{
				ShowError("Такой отзыв не найден");
				return RedirectToAction("Reviews");
			}

			DataContext.Reviews.DeleteOnSubmit(review);
			DataContext.SubmitChanges();

			ShowSuccess("Отзыв был успешно удален");

			return RedirectToAction("Reviews");
		}

		/// <summary>
		/// Обрабатывает изменение позиции отзыва
		/// </summary>
		/// <param name="review">Идентификатор отзыва</param>
		/// <param name="position">Номер новой позиции</param>
		/// <returns></returns>
		[Route("reviews/review-position-change")]
		[AuthorizationCheck()]
		public ActionResult ReviewPositionChange(long review, int position)
		{
			try
			{
				var _reviews = DataContext.Reviews.FirstOrDefault(r => r.Id == review);
				if (_reviews == null)
				{
					throw new Exception("Отзыв не найден");
				}

				var reviews = DataContext.Reviews.OrderBy(r => r.Position).ToList();
				_reviews = reviews.Find(r => r.Id == review);
				reviews.Remove(_reviews);
				reviews.Insert(position, _reviews);
				var counter = 0;
				foreach (var rm in reviews)
				{
					counter++;
					rm.Position = counter;
				}

				DataContext.SubmitChanges();

				return JsonNet(new
				{
					success = true
				});
			}
			catch (Exception e)
			{
				return JsonNet(new
				{
					success = false,
					message = e.Message
				});
			}
		}

		#endregion

		#region Акции

		/// <summary>
		/// Отображает список акций
		/// </summary>
		/// <returns></returns>
		[Route("offers")]
		[AuthorizationCheck()]
		public ActionResult Offers()
		{
			var offers = DataContext.SpecialOffers.OrderBy(c => c.Position).ToList();

			PushNavigationItem("Администрирование");
			PushNavigationItem("Акции");

			return View("Offers", offers);
		}

		/// <summary>
		/// Отображает форму создания новой акции
		/// </summary>
		/// <returns></returns>
		[Route("offers/new")]
		[AuthorizationCheck()]
		public ActionResult AddOffer()
		{
			PushNavigationItem("Администрирование");
			PushNavigationItem("Акции");
			PushNavigationItem("Новая акция");

			return View("EditOffer", new SpecialOffer());
		}

		/// <summary>
		/// Отображает форму редактирования существующей акции
		/// </summary>
		/// <param name="id">Идентификатор акции</param>
		/// <returns></returns>
		[Route("offers/{id}/edit")]
		[AuthorizationCheck()]
		public ActionResult EditOffer(long id)
		{
			var offer = DataContext.SpecialOffers.FirstOrDefault(r => r.Id == id);
			if (offer == null)
			{
				ShowError("Такая акция не найдена");
				return RedirectToAction("Offers");
			}

			PushNavigationItem("Администрирование");
			PushNavigationItem("Акции");
			PushNavigationItem(offer.Title);

			return View("EditOffer", offer);
		}

		/// <summary>
		/// Обрабатывает сохранение новой акции или изменение в существующей акции
		/// </summary>
		/// <param name="model">Модель данных</param>
		/// <returns></returns>
		[Route("offers/save")]
		[HttpPost]
		[ValidateInput(false)]
		[AuthorizationCheck()]
		public ActionResult SaveOffer(SpecialOffer model)
		{
			// Сохраняем картинку
			var file = Request.Files["Image"];
			string imageUrl = null;
			if (file != null && file.ContentLength > 0 && file.ContentType.ToLower().Contains("image"))
			{
				var fileName = Path.ChangeExtension(Path.GetRandomFileName(), ".jpg");
				var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Offers");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				var fullPath = Path.Combine(path, fileName);
				var image = new WebImage(file.InputStream);
				image.Resize(900, 900, true, true).Save(fullPath, "JPEG");
				imageUrl = "/Files/Offers/" + fileName;
			}

			if (model.Id <= 0)
			{
				model.DateCreated = DateTime.Now.ToUniversalTime();
				model.CreatedBy = CurrentUser.ToString();
				model.Image = imageUrl;

				DataContext.SpecialOffers.InsertOnSubmit(model);
				DataContext.SubmitChanges();

				ShowSuccess("Акция была успешно создана");
			}
			else
			{
				// Ищем
				var offer = DataContext.SpecialOffers.FirstOrDefault(r => r.Id == model.Id);
				if (offer == null)
				{
					ShowError("Акция не найдена");
					return RedirectToAction("Offers");
				}

				// Редактируем
				var oldImageUrl = offer.Image;
				TryUpdateModel(offer);
				offer.DateModified = DateTime.Now.ToUniversalTime();
				offer.ModifiedBy = CurrentUser.ToString();
				if (imageUrl != null)
				{
					offer.Image = imageUrl;
				}
				else
				{
					offer.Image = oldImageUrl;
				}

				DataContext.SubmitChanges();

				ShowSuccess("Акция была успешно отредактирована");
			}

			return RedirectToAction("Offers");
		}

		/// <summary>
		/// Обрабатывает удаление указанной акции
		/// </summary>
		/// <param name="id">Идентификатор акции</param>
		/// <returns></returns>
		[Route("offers/{id}/delete")]
		[AuthorizationCheck()]
		public ActionResult DeleteOffer(long id)
		{
			var offer = DataContext.SpecialOffers.FirstOrDefault(r => r.Id == id);
			if (offer == null)
			{
				ShowError("Такая акция не найдена");
				return RedirectToAction("Offers");
			}

			DataContext.SpecialOffers.DeleteOnSubmit(offer);
			DataContext.SubmitChanges();

			ShowSuccess("Акция был успешно удалена");

			return RedirectToAction("Offers");
		}

		/// <summary>
		/// Обрабатывает изменение позиции акции
		/// </summary>
		/// <param name="offer">Идентификатор акции</param>
		/// <param name="position">Номер новой позиции</param>
		/// <returns></returns>
		[Route("offers/offer-position-change")]
		[AuthorizationCheck()]
		public ActionResult OfferPositionChange(long offer, int position)
		{
			try
			{
				var _offers = DataContext.SpecialOffers.FirstOrDefault(r => r.Id == offer);
				if (_offers == null)
				{
					throw new Exception("Акция не найдена");
				}

				var offers = DataContext.SpecialOffers.OrderBy(r => r.Position).ToList();
				_offers = offers.Find(r => r.Id == offer);
				offers.Remove(_offers);
				offers.Insert(position, _offers);
				var counter = 0;
				foreach (var rm in offers)
				{
					counter++;
					rm.Position = counter;
				}

				DataContext.SubmitChanges();

				return JsonNet(new
				{
					success = true
				});
			}
			catch (Exception e)
			{
				return JsonNet(new
				{
					success = false,
					message = e.Message
				});
			}
		}

		#endregion
	}
}