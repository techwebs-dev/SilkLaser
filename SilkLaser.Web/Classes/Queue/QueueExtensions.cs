// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: QueueExtensions.cs
// 
// Created by: ykors_000 at 04.08.2014 16:28
// 
// Property of SoftGears
// 
// ========

using SilkLaser.Web.Classes.Enums;
using SilkLaser.Web.Classes.Interfaces.Queue;
using SilkLaser.Web.Models.Queue;

namespace SilkLaser.Web.Classes.Queue
{
	/// <summary>
	/// Класс с расширениями интерфейса менеджера очередей
	/// </summary>
	public static class QueueExtensions
	{
		/// <summary>
		/// Отправляет Email уведомление на указанный адрес с указанными параметрами
		/// </summary>
		/// <param name="mgr">Менеджер</param>
		/// <param name="recipient">Получатель</param>
		/// <param name="subject">Тема сообщения</param>
		/// <param name="body">Тело сообщения</param>
		public static void SendEmail(this IQueueManager mgr, string recipient, string subject, string body)
		{
			mgr.PushItem(WorkerCommand.Email, new EmailNotificationData(recipient,subject,body));
		}

		/// <summary>
		/// Отправляет СМС уведомление на указанный адрес с указанными параметрами
		/// </summary>
		/// <param name="mgr">Менеджер</param>
		/// <param name="recipient">Получатель</param>
		/// <param name="content">Содержимое</param>
		public static void SendSMS(this IQueueManager mgr, string recipient, string content)
		{
			mgr.PushItem(WorkerCommand.SMS,new SMSNotificationData(recipient,content));
		}
	}
}