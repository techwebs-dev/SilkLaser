// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: ISMSSender.cs
// 
// Created by: ykors_000 at 13.08.2014 14:39
// 
// Property of SoftGears
// 
// ========
namespace SilkLaser.Web.Classes.Interfaces.Notifications
{
	/// <summary>
	/// Абстрактный класс для отправки SMS уведомлений
	/// </summary>
	public interface ISMSSender
	{
		/// <summary>
		/// Отправляет СМС уведомление с указанным содержанием указанному человеку
		/// </summary>
		/// <param name="recipient">Получатель</param>
		/// <param name="content">Содержание</param>
		void Send(string recipient, string content);
	}
}