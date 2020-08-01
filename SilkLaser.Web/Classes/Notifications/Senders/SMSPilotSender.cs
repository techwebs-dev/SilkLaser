// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: SMSPilotSender.cs
// 
// Created by: ykors_000 at 13.08.2014 14:42
// 
// Property of SoftGears
// 
// ========

using System;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using SilkLaser.Web.Classes.Interfaces.Notifications;
using SilkLaser.Web.Classes.Utils;

namespace SilkLaser.Web.Classes.Notifications.Senders
{
	/// <summary>
	/// Класс для отправки СМС уведомлений используя сервис SMS Pilot
	/// </summary>
	public class SMSPilotSender: ISMSSender
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public SMSPilotSender()
		{
			ApiKey = System.Configuration.ConfigurationManager.AppSettings["SMSPilotApiKey"];
			SenderName = System.Configuration.ConfigurationManager.AppSettings["SMSPilotSenderName"];
		}

		/// <summary>
		///  Имя отправителя
		/// </summary>
		public string SenderName { get; set; }

		/// <summary>
		/// API Ключ
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// Отправляет СМС уведомление с указанным содержанием указанному человеку
		/// </summary>
		/// <param name="recipient">Получатель</param>
		/// <param name="content">Содержание</param>
		public void Send(string recipient, string content)
		{
			var n = StringUtils.NormalizePhoneNumber(recipient);

			// Отправляем смс уведомление
			// Корневой объект отправки
			var rootObject = new JObject(new JProperty("apikey", new JValue(ApiKey)));

			// Массив, содержащий отправляемые сообщения
			var sendArray = new JArray();

			var sendObject = new JObject(
					new JProperty("id", new JValue(Guid.NewGuid())),
					new JProperty("from", new JValue(SenderName)),
					new JProperty("to", new JValue(n)),
					new JProperty("text", new JValue(content)));

			sendArray.Add(sendObject);
			rootObject.Add(new JProperty("send", sendArray));

			// Отправляем
			HttpWebResponse smsResponse = null;
			try
			{
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(String.Format("http://smspilot.ru/api2.php?json={0}", HttpUtility.UrlEncode(rootObject.ToString())));
				smsResponse = (HttpWebResponse)webRequest.GetResponse();

				// Обрабатываем результат
				bool sendSuccess = false;
				var responseStream = smsResponse.GetResponseStream();
				if (responseStream != null)
				{
					// Анализируем ответ
					var json = new StreamReader(responseStream).ReadToEnd();
					if (json.Contains("\"send\""))
					{
						// NOTE: все ок
					}
					else
					{
						throw new Exception(string.Format("Не удалось отправить СМС уведомление. Сервер вернул {0}", json));
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception("Не удалось отправить СМС уведомление",e);
			}
		}
	}
}