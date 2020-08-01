// 
// 
// Solution: SilkLaser
// Project: SilkLaser.Web
// File: SearchResultItem.cs
// 
// Created by: ykors_000 at 12.01.2017 13:28
// 
// Property of SoftGears
// 
// ========
namespace SilkLaser.Web.Models
{
	/// <summary>
	/// Модель элемента в результатах поиска
	/// </summary>
	public class SearchResultItem
	{
		/// <summary>
		/// Найденный заголовок страницы
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Ссылка на страницу
		/// </summary>
		public string Url { get; set; }
	}
}