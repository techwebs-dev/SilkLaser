// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: EmailNotificationData.cs
// 
// Created by: ykors_000 at 04.08.2014 16:25
// 
// Property of SoftGears
// 
// ========

using Newtonsoft.Json;

namespace SilkLaser.Web.Models.Queue
{
	/// <summary>
	/// Модель Email уведомления для очереди
	/// </summary>
	public class EmailNotificationData
	{
		/// <summary>
		/// Получатель
		/// </summary>
		[JsonProperty("recipient")]
		public string Recipient { get; set; }

		/// <summary>
		/// Заголовок
		/// </summary>
		[JsonProperty("subject")]
		public string Subject { get; set; }

		/// <summary>
		/// Тело сообщения
		/// </summary>
		[JsonProperty("body")]
		public string Body { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public EmailNotificationData(string recipient, string subject, string body)
		{
			Recipient = recipient;
			Subject = subject;
			Body = body;
		}
	}
}