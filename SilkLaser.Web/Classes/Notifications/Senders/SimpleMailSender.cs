// 
// 
// Solution: CityRent
// Project: CityRent.Infrastructure
// File: SimpleMailSender.cs
// 
// Created by: ykors_000 at 09.10.2015 14:14
// 
// Property of SoftGears
// 
// ========

using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using NLog;
using SilkLaser.Web.Classes.Interfaces.Notifications;

namespace SilkLaser.Web.Classes.Notifications.Senders
{
	/// <summary>
	/// Простой класс, отправляющий письма через SMTP шлюз
	/// </summary>
	public class SimpleMailSender: IEmailSender
	{
		/// <summary>
		/// Отправляет Email сообщение с указанным заголовком указанному человеку с указаным содержанием
		/// </summary>
		/// <param name="recipient">Получатель</param>
		/// <param name="subject">Тема-заголовок</param>
		/// <param name="body">Тело сообщения</param>
		public void Send(string recipient, string subject, string body)
		{
			// Получаем данные для отправки
			var connectionData = new MailConnectionString(System.Configuration.ConfigurationManager.AppSettings["MailConnectionString"]);

			// Подгатавливаем клиент
			var mailClient = new SmtpClient(connectionData.Host, connectionData.Port)
			{
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(connectionData.Login, connectionData.Password),
				EnableSsl = connectionData.UseSsl
			};

			// Формируем письмо
			var mailMessage = new MailMessage(new MailAddress(connectionData.FromAddress, connectionData.FromName),
											  new MailAddress(recipient))
			{
				Subject = subject,
				SubjectEncoding = Encoding.UTF8,
				Body = body,
				BodyEncoding = Encoding.UTF8,
				IsBodyHtml = true
			};

			// Пытаемся отправить
			try
			{
				mailClient.Send(mailMessage);
			}
			catch (Exception e)
			{
				var logger = LogManager.GetCurrentClassLogger();
				logger.Error(string.Format("Не удалось отправить письмо получателю {0} по причине: {1}", recipient, e.Message));
			}
		}
	}
}