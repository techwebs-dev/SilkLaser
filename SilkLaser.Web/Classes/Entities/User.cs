// 
// 
// Solution: SeoDesk
// Project: SeoDesk.Manager.Infrastructure
// File: User.cs
// 
// Created by: ykors_000 at 12.10.2016 13:46
// 
// Property of SoftGears
// 
// ========

using System;
using System.Linq;
using System.Text;
using SilkLaser.Web.Classes.Utils;

namespace SilkLaser.Web.Classes.Entities
{
	/// <summary>
	/// Пользователь менеджерской системы
	/// </summary>
	public partial class User
	{

		/// <summary>
		/// Возвращает урл аватарки
		/// </summary>
		/// <returns></returns>
		public string GetAvatar()
		{
			return Photo ?? String.Format("http://www.gravatar.com/avatar/{0}?d=monsterid",
								 PasswordUtils.GenerateMD5PasswordHash(Email.Trim().ToLower()).ToLower());
		}

		/// <summary>
		/// Возвращает фио пользователя
		/// </summary>
		/// <returns></returns>
		public string GetFio()
		{
			return String.Format("{0} {1} {2}", LastName, FirstName, SurName).Trim();
		}

		/// <summary>
		/// Возвращает ФИО в сокращенном формате
		/// </summary>
		/// <returns></returns>
		public string GetFioSmall()
		{
			var sb = new StringBuilder();
			sb.Append(LastName);
			if (!String.IsNullOrEmpty(FirstName))
				sb.AppendFormat(" {0}.", FirstName.First());
			if (!String.IsNullOrEmpty(SurName))
				sb.AppendFormat(" {0}.", SurName.First());
			return sb.ToString();
		}

		/// <summary>
		/// Возвращает фио в международном формате
		/// </summary>
		/// <returns></returns>
		public string GetFioInternation()
		{
			var sb = new StringBuilder();
			sb.Append(FirstName);
			if (!String.IsNullOrEmpty(SurName))
				sb.AppendFormat(" {0}", SurName.First());
			if (!String.IsNullOrEmpty(LastName))
				sb.AppendFormat(" {0}", LastName);
			return sb.ToString().Transliterate();
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			var name = String.Format("{0} <{1}>", GetFioSmall(), Email);
			if (name.Length > 255)
			{
				return name.Substring(0, 254);
			}

			return name;
		}

		/// <summary>
		/// Конвертирует указанную дату и время в UTC в локальную дату и время в часовом поясе пользователя
		/// </summary>
		/// <param name="dateTime">Входная дата и время</param>
		/// <returns></returns>
		public DateTime ConvertDateToTimeZone(DateTime dateTime)
		{
			return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById(Timezone));
		}

		/// <summary>
		/// Конвертирует указанную дату и время в UTC в локальную дату и время в часовом поясе пользователя
		/// </summary>
		/// <param name="dateTime">Входная дата и время</param>
		/// <returns></returns>
		public DateTime? ConvertDateToTimeZone(DateTime? dateTime)
		{
			if (dateTime == null)
			{
				return null;
			}
			var dt = DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
			return TimeZoneInfo.ConvertTimeFromUtc(dt, TimeZoneInfo.FindSystemTimeZoneById(Timezone));
		}

		/// <summary>
		/// Конвертируем указанную дату и время в GMT формат с учетом временной зоны пользователя
		/// </summary>
		/// <param name="dateTime">Дата и время</param>
		/// <returns></returns>
		public DateTime ConvertDateToUtc(DateTime dateTime)
		{
			var dt = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
			var timeZone = TimeZoneInfo.FindSystemTimeZoneById(Timezone);
			return TimeZoneInfo.ConvertTime(dt, timeZone).ToUniversalTime();
		}
	}
}