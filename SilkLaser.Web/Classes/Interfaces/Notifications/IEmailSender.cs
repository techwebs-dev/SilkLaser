// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: IEmailSender.cs
// 
// Created by: ykors_000 at 13.08.2014 14:39
// 
// Property of SoftGears
// 
// ========
namespace SilkLaser.Web.Classes.Interfaces.Notifications
{
	/// <summary>
	/// Абстрактный класс для отправки Email сообщений
	/// </summary>
	public interface IEmailSender
	{
		/// <summary>
		/// Отправляет Email сообщение с указанным заголовком указанному человеку с указаным содержанием
		/// </summary>
		/// <param name="recipient">Получатель</param>
		/// <param name="subject">Тема-заголовок</param>
		/// <param name="body">Тело сообщения</param>
		void Send(string recipient, string subject, string body);
	}
}