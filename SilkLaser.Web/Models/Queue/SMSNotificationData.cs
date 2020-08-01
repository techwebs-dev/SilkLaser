// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: SMSNotificationData.cs
// 
// Created by: ykors_000 at 13.08.2014 14:38
// 
// Property of SoftGears
// 
// ========

using Newtonsoft.Json;

namespace SilkLaser.Web.Models.Queue
{
	/// <summary>
	/// Класс для хранения данных СМС уведомлений
	/// </summary>
	public class SMSNotificationData
	{
		/// <summary>
		/// Новый конструктор данных смс уведомления
		/// </summary>
		/// <param name="recipient"></param>
		/// <param name="content"></param>
		public SMSNotificationData(string recipient, string content)
		{
			Recipient = recipient;
			Content = content;
		}

		/// <summary>
		/// Получатель
		/// </summary>
		[JsonProperty("recipient")]
		public string Recipient { get; set; }

		/// <summary>
		/// Содержание
		/// </summary>
		[JsonProperty("content")]
		public string Content { get; set; }
	}
}