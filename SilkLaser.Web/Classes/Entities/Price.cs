// 
// 
// Solution: SilkLaser
// Project: SilkLaser.Web
// File: Price.cs
// 
// Created by: ykors_000 at 10.01.2017 15:34
// 
// Property of SoftGears
// 
// ========

using System.Linq;

namespace SilkLaser.Web.Classes.Entities
{
	/// <summary>
	/// Ценник на услуги
	/// </summary>
	public partial class Price
	{
		/// <summary>
		/// Возвращает оптовую цену на указанное количество визитов
		/// </summary>
		/// <param name="count">Количество визиток</param>
		/// <returns></returns>
		public decimal GetPriceForVisits(int count)
		{
			switch (count)
			{
				case 4:
					return BasePrice * (decimal) 0.8;
				case 6:
					return BasePrice * (decimal) 0.75;
				case 8:
					return BasePrice * (decimal) 0.7;
				case 10:
					return BasePrice * (decimal) 0.6;
				default:
					return BasePrice;
			}
		}

		/// <summary>
		/// Есть ли акция на данный ценник
		/// </summary>
		/// <returns></returns>
		public bool HasSpecialOffer()
		{
			return
				new[] {BasePriceOffer, CoursePriceOffer, CorrectionPriceOffer}.Any(
					i => i.HasValue);
		}
	}
}