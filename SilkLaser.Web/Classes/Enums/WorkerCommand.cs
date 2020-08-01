// 
// 
// Solution: CityTimer
// Project: CityTimer.Domain
// File: WorkerCommand.cs
// 
// Created by: ykors_000 at 16.07.2014 13:09
// 
// Property of SoftGears
// 
// ========

namespace SilkLaser.Web.Classes.Enums
{
	/// <summary>
	/// Команды отложенного обработчика
	/// </summary>
	public enum WorkerCommand: short
	{
		/// <summary>
		/// Email уведомление
		/// </summary>
		[EnumDescription("Email уведомление")]
		Email = 1,

		/// <summary>
		/// SMS уведомление
		/// </summary>
		[EnumDescription("SMS уведомление")]
		SMS = 2,

		/// <summary>
		/// Push уведомление
		/// </summary>
		[EnumDescription("Push уведомление")]
		Push = 3,
	}
}