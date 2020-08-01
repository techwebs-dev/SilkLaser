using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using SilkLaser.Web.Classes.Entities;
using SilkLaser.Web.Classes.Queue;
using SilkLaser.Web.Classes.Utils;
using SilkLaser.Web.Models;

namespace SilkLaser.Web.Controllers
{
	/// <summary>
	/// Главный контроллер
	/// </summary>
    public class MainController : BaseController
    {
	    #region Главная страница

	    /// <summary>
		/// Отображает главную страницу сайта
		/// </summary>
		/// <returns></returns>
		[Route("")]
	    public ActionResult Index()
	    {
		    return Redirect("/laser-epilation");
	    }

	    #endregion

	    #region Просмотр страниц

	    /// <summary>
		/// Отображает указанную статическую страницу
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult ViewPage(long id)
		{
			var page = DataContext.HtmlPages.FirstOrDefault(p => p.Id == id);
			if (page == null)
			{
				return Redirect("");
			}

			return View("ViewPage",page);
		}

	    #endregion

	    #region Цены

	    /// <summary>
		/// Отображает страницу с ценами
		/// </summary>
		/// <returns></returns>
		[Route("prices")]
	    public ActionResult Prices()
		{
			return View("Prices");
		}

		/// <summary>
		/// Отображает страницу с указанным ценником
		/// </summary>
		/// <param name="id">Идентификатор ценника</param>
		/// <returns></returns>
		[Route("prices/{id}/{seoTitle}")]
	    public ActionResult ViewPrice(long id)
		{
			var price = DataContext.Prices.FirstOrDefault(p => p.Id == id);
			if (price == null)
			{
				return RedirectToAction("Prices");
			}

			return View("ViewPrice", price);
		}

		/// <summary>
		/// Отображает частичный фрагмент таблицы цен
		/// </summary>
		/// <returns></returns>
		[Route("prices-table")]
	    public ActionResult PricesTable()
	    {
		    return PartialView("PricesTable");
	    }

	    #endregion

	    #region FAQ

	    /// <summary>
		/// Отображает страницу с вопросами и ответами
		/// </summary>
		/// <returns></returns>
		[Route("faq")]
	    public ActionResult Faq()
	    {
		    return View("Faq");
	    }

		/// <summary>
		/// Отображает страницу из FAQ
		/// </summary>
		/// <param name="id">Ид вопроса</param>
		/// <returns></returns>
		[Route("faq/{id}/{seoTitle}")]
	    public ActionResult ViewFaqItem(long id)
		{
			var item = DataContext.FaqQuestions.FirstOrDefault(i => i.Id == id);
			if (item == null)
			{
				return RedirectToAction("Index");
			}

			return View("ViewFaqItem", item);
		}

	    #endregion

	    #region Отзывы

	    /// <summary>
		/// Отображает страницу с отзывами
		/// </summary>
		/// <returns></returns>
		[Route("reviews")]
	    public ActionResult Reviews()
		{
			return View("Reviews");
		}

		/// <summary>
		/// Возвращает частичный вид с отзывами в карусели
		/// </summary>
		/// <returns></returns>
		[Route("reviews-carousel")]
	    public ActionResult ReviewsCarousel()
		{
			return PartialView("ReviewsCarousel");
		}

		/// <summary>
		/// Отображает карусель отзывов для лендинга
		/// </summary>
		/// <returns></returns>
		[Route("landing-reviews-carousel")]
	    public ActionResult LandingReviewsCarousel()
		{
			return PartialView("LandingReviewsCarousel");
		}

		/// <summary>
		/// Сохраняет отзыв в базе данных
		/// </summary>
		/// <param name="t">Тип</param>
		/// <param name="n">Имя</param>
		/// <param name="p">Телефон</param>
		/// <param name="c">Текст отзыва</param>
		/// <returns></returns>
		[HttpPost][Route("submit-review")]
	    public ActionResult SubmitReview(string t, string n, string p, string c)
		{
			// Сохраняем картинку
			var file = Request.Files["file"];
			string imageUrl = null;
			if (file != null && file.ContentLength > 0 && file.ContentType.ToLower().Contains("image"))
			{
				var fileName = Path.ChangeExtension(Path.GetRandomFileName(), ".png");
				var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Reviews");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				var fullPath = Path.Combine(path, fileName);
				var image = new WebImage(file.InputStream);
				image.Resize(46, 46, true, true).Save(fullPath, "PNG");
				imageUrl = "/Files/Reviews/" + fileName;
			}

			var review = new Review()
			{
				Category = t,
				Content = c,
				CreatorIP = Request.UserHostAddress,
				DateCreated = DateTime.Now.ToUniversalTime(),
				Name = n,
				Phone = p,
				Position = DataContext.Reviews.Select(r => r.Position).ToArray().DefaultIfEmpty(0).Max() + 1,
			Image = imageUrl
			};
			DataContext.Reviews.InsertOnSubmit(review);
			DataContext.SubmitChanges();

			return View("ReviewSubmitted");
		}

	    #endregion

	    #region Поиск

		/// <summary>
		/// Обрабатывает поисковый запрос пользователя
		/// </summary>
		/// <param name="q"></param>
		/// <returns></returns>
		[Route("search")]
	    public ActionResult Search(string q)
		{
			var results = new List<SearchResultItem>();

			if (!String.IsNullOrEmpty(q))
			{
				q = q.ToLower();

				// Поиск по страницам
				results.AddRange(DataContext.HtmlPages.Where(p => p.Title.ToLower().Contains(q) || p.HTMLTitle.ToLower().Contains(q) || p.Content.Contains(q)).Select(p => new SearchResultItem()
				{
					Title = p.HTMLTitle,
					Url = "/"+p.Route
				}));

				// Поиск по ценам
				results.AddRange(DataContext.Prices.Where(p => p.Title.ToLower().Contains(q) || p.HTMLTitle.ToLower().Contains(q) || (p.Description != null &&  p.Description.ToLower().Contains(q))).Select(p => new SearchResultItem()
				{
					Title = p.HTMLTitle,
					Url = $"/prices/{p.Id}/{StringUtils.TitleToSeoRoute(p.Title)}"
				}));

				// Поиск по вопросам
				results.AddRange(DataContext.FaqQuestions.Where(p => p.Question.ToLower().Contains(q) || p.Answer.ToLower().Contains(q)).Select(p => new SearchResultItem()
				{
					Title = p.Question,
					Url = $"/faq/{p.Id}/{StringUtils.TitleToSeoRoute(p.Question)}"
				}));
			}

			ViewBag.query = q;

			return View("SearchResults",results);
		}

	    #endregion

	    #region Акции

		/// <summary>
		/// Отображает страницу с акциями
		/// </summary>
		/// <returns></returns>
		[Route("offers")]
	    public ActionResult Offers()
		{
			var offers = DataContext.SpecialOffers.OrderBy(o => o.Position).ToList();

			return View("Offers", offers);
		}

	    #endregion

	    #region Обратная связь

	    /// <summary>
	    /// Обрабатывает поступление заявки из формы обратной связи
	    /// </summary>
	    /// <param name="n">Имя</param>
	    /// <param name="p">Телефон</param>
	    /// <param name="b">Часть тела</param>
	    /// <param name="f">Имя формы</param>
	    /// <param name="c">Djghjc</param>
	    /// <returns></returns>
	    [Route("feedback")][HttpPost]
	    public ActionResult Feedback(string n, string p, string b, string f, string c)
		{
			var message = $"<p>Получена заявка с формы обратной связи {f} со следующими данными:</p>" +
			              "<ul>" +
			              $"<li><b>Имя</b>: {n}</li>" +
			              $"<li><b>Телефон</b>: {p}</li>" +
			              $"<li><b>Часть тела</b>: {b ?? "Неуказано"}</li>" +
			              $"<li><b>Вопрос</b>: {c ?? "Неуказан"}</li>" +
			              $"<li><b>IP адрес</b>: {Request.UserHostAddress}</li>" +
			              "</ul>";

			QueueManager.SendEmail(ConfigurationManager.AppSettings["FeedbackEmail"],"Новая заявка с сайта SilkLaser.ru",message);

			return Json(new {success = true});
		}

	    #endregion
    }
}