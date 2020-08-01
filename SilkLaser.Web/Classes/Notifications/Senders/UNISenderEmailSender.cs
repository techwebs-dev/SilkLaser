// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: UNISenderEmailSender.cs
// 
// Created by: ykors_000 at 13.08.2014 14:51
// 
// Property of SoftGears
// 
// ========

using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using SilkLaser.Web.Classes.Interfaces.Notifications;

namespace SilkLaser.Web.Classes.Notifications.Senders
{
	/// <summary>
	/// Отправщик Email сообщений, использующий UniSender Api
	/// </summary>
	public class UniSenderEmailSender : IEmailSender
	{
		/// <summary>
		/// API Ключ
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// Имя отправителя
		/// </summary>
		public string SenderName { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public UniSenderEmailSender()
		{
			ApiKey = System.Configuration.ConfigurationManager.AppSettings["UniSenderApiKey"];
			SenderName = System.Configuration.ConfigurationManager.AppSettings["UniSenderSenderName"];
		}

		/// <summary>
		/// Отправляет Email сообщение с указанным заголовком указанному человеку с указаным содержанием
		/// </summary>
		/// <param name="recipient">Получатель</param>
		/// <param name="subject">Тема-заголовок</param>
		/// <param name="body">Тело сообщения</param>
		public void Send(string recipient, string subject, string body)
		{
			// Отправляем письмо
			var client = new WebClient();
			var request = "http://api.unisender.com/ru/api/sendEmail?format=json";
			var requestParams = new NameValueCollection
            {
                {"api_key", ApiKey},
                {"email", recipient},
                {"sender_name", SenderName},
                {"sender_email", "mailing.nprdv@gmail.com"},
                {"subject", subject},
                {"body", body},
                {"list_id", "2959162"},
                {"lang", "ru"}
            };

			// Пытаемся отправить
			try
			{
				var response = Encoding.UTF8.GetString(client.UploadValues(request, requestParams));
				if (response.Contains("\"email_id\""))
				{
					// Все ок
				}
				else
				{
					throw new Exception("Не удалось отправить Email уведомление. Сервер вернул: "+response);
				}
			}
			catch (Exception e)
			{
				throw new Exception("Не удалось отправить Email уведомление", e);
			}
		}
	}
}