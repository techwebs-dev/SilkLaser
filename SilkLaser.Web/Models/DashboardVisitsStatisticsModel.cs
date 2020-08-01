// 
// 
// Solution: RusHandball
// Project: RusHandball.Web
// File: DashboardVisitsStatisticsModel.cs
// 
// Created by: ykors_000 at 09.12.2016 16:30
// 
// Property of SoftGears
// 
// ========

using System;
using System.Collections.Generic;

namespace SilkLaser.Web.Models
{
	/// <summary>
	/// Модель для построения графиков статистики на дашбоарде
	/// </summary>
	public class DashboardVisitsStatisticsModel
	{
		/// <summary>
		/// Пассив посещаемости по дням
		/// </summary>
		public IList<VisitStatsDataItem> DataItems { get; set; }

		/// <summary>
		/// Начало периода
		/// </summary>
		public DateTime PeriodStart { get;set; }

		/// <summary>
		/// Конец периода
		/// </summary>
		public DateTime PeriodEnd { get; set; }

		/// <summary>
		/// Создает модель для построения статистики посещаемости
		/// </summary>
		/// <param name="periodStart"></param>
		/// <param name="periodEnd"></param>
		public DashboardVisitsStatisticsModel(DateTime periodStart, DateTime periodEnd)
		{
			PeriodStart = periodStart;
			PeriodEnd = periodEnd;
			DataItems = new List<VisitStatsDataItem>();
		}

		/// <summary>
		/// Элемент массива посещаемости
		/// </summary>
		public class VisitStatsDataItem
		{
			/// <summary>
			/// Дата
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// кол-во просмотров страниц
			/// </summary>
			public int PageViews { get; set; }

			/// <summary>
			/// Кол-во визитов
			/// </summary>
			public int Visits { get; set; }

			/// <summary>
			/// Кол-во посетителей
			/// </summary>
			public int Visitors { get; set; }

			/// <summary>
			/// Новых посетителей
			/// </summary>
			public int NewVisitors { get; set; }

			/// <summary>
			/// Глубина просмотра
			/// </summary>
			public double Depth { get; set; }

			/// <summary>
			/// Время на сайте
			/// </summary>
			public int Time { get; set; }

			/// <summary>
			/// Показатель отказов
			/// </summary>
			public double Denials { get; set; }
		}

		/// <summary>
		/// Добавляет элемент в массив данных
		/// </summary>
		/// <param name="date">Дата</param>
		/// <param name="pageViews">Кол-во просмотров страницы</param>
		/// <param name="visits">Визитов</param>
		/// <param name="visitors">Посетителей</param>
		/// <param name="newVisitors">Новых посетителей</param>
		/// <param name="time">Время на сайте</param>
		/// <param name="depth">Глубина просмотра</param>
		/// <param name="denial">Показатель отказов</param>
		public void AddDataItem(DateTime date, int pageViews, int visits, int visitors, int newVisitors, int time, double depth, double denial)
		{
			DataItems.Add(new VisitStatsDataItem()
			{
				Date = date,
				Denials = denial,
				Depth = depth,
				NewVisitors = newVisitors,
				PageViews = pageViews,
				Time = time,
				Visitors = visitors,
				Visits = visits
			});
		}
	}
}