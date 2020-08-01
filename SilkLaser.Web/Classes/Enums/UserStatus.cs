// 
// 
// Solution: PPPGram
// Project: PPPGram.Infrastructure
// File: UserStatus.cs
// 
// Created by: ykors_000 at 21.12.2015 12:09
// 
// Property of SoftGears
// 
// ========
namespace SilkLaser.Web.Classes.Enums
{
	/// <summary>
	/// Статусы пользователей
	/// </summary>
	public enum UserStatus
	{
		/// <summary>
		/// Пользователь активен
		/// </summary>
		Active = 1,

		/// <summary>
		/// Пользователь неактивен
		/// </summary>
		Inactive = 2,

		/// <summary>
		/// Пользователь заблокирован
		/// </summary>
		Blocked = 3
	}
}