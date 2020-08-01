// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: QueueManager.cs
// 
// Created by: ykors_000 at 04.08.2014 16:15
// 
// Property of SoftGears
// 
// ========

using System;
using Newtonsoft.Json;
using SilkLaser.Web.Classes.DAL;
using SilkLaser.Web.Classes.Entities;
using SilkLaser.Web.Classes.Enums;
using SilkLaser.Web.Classes.Interfaces.Queue;

namespace SilkLaser.Web.Classes.Queue
{
	/// <summary>
	/// Менеджер для работы с очередями объектов
	/// </summary>
	public class DBQueueManager: IQueueManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public DBQueueManager(SilkLaserDataContext dataContext)
		{
			DataContext = dataContext;
		}

		/// <summary>
		/// Контекст доступа к данным
		/// </summary>
		public SilkLaserDataContext DataContext { get; set; }

		/// <summary>
		/// Добавляет новый элемент в очередь сообщений
		/// </summary>
		/// <param name="itemType">Тип элемента очереди</param>
		/// <param name="data">Связанные данные</param>
		public void PushItem(WorkerCommand itemType, object data)
		{
			DataContext.WorkerQueueItems.InsertOnSubmit(new WorkerQueueItem()
			{
				Data = JsonConvert.SerializeObject(data),
				ItemType = (short) itemType,
				Processed = false,
				DateCreated = DateTime.Now,
			});
			DataContext.SubmitChanges();
		}
	}
}