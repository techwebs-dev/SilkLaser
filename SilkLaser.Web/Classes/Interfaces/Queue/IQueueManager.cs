// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: IQueueManager.cs
// 
// Created by: ykors_000 at 04.08.2014 16:17
// 
// Property of SoftGears
// 
// ========

using SilkLaser.Web.Classes.Enums;

namespace SilkLaser.Web.Classes.Interfaces.Queue
{
	/// <summary>
	/// Абстрактный менеджер по работе с очередями отложенной обработки
	/// </summary>
	public interface IQueueManager
	{
		/// <summary>
		/// Добавляет новый элемент в очередь сообщений
		/// </summary>
		/// <param name="itemType">Тип элемента очереди</param>
		/// <param name="data">Связанные данные</param>
		void PushItem(WorkerCommand itemType, object data);
	}
}