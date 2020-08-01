// 
// 
// Solution: SilkLaser
// Project: SilkLaser.Web
// File: BodyGroup.cs
// 
// Created by: ykors_000 at 10.01.2017 14:47
// 
// Property of SoftGears
// 
// ========
namespace SilkLaser.Web.Classes.Enums
{
	/// <summary>
	/// Группы частей тела
	/// </summary>
	public enum BodyGroup
	{
		[EnumDescription("Лицо")]
		Head = 1,

		[EnumDescription("Тело")]
		Body = 2,

		[EnumDescription("Бикини")]
		Bikini = 3,

		[EnumDescription("Комбинации")]
		Combine = 4,
	}
}