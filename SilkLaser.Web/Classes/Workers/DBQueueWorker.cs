// 
// 
// Solution: HealthCare
// Project: HealthCare.Infrastructure
// File: QueueWorker.cs
// 
// Created by: ykors_000 at 13.08.2014 14:20
// 
// Property of SoftGears
// 
// ========

using System;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using NLog;
using SilkLaser.Web.Classes.DAL;
using SilkLaser.Web.Classes.Entities;
using SilkLaser.Web.Classes.Enums;
using SilkLaser.Web.Classes.Interfaces.Notifications;
using SilkLaser.Web.Models.Queue;

namespace SilkLaser.Web.Classes.Workers
{
	/// <summary>
	/// Обработчик очередей из базы данных
	/// </summary>
	public class DBQueueWorker
	{
		/// <summary>
		/// Абстрактный отправитель сообщений
		/// </summary>
		public IEmailSender EmailSender { get; set; }

		/// <summary>
		/// Абстрактный отправитель SMS уведомлений
		/// </summary>
		public ISMSSender SMSSender { get; set; }

		/// <summary>
		/// Конструктор инициализирующий таймер и начальные свойства рабочего процесса
		/// </summary>
		public DBQueueWorker(IEmailSender emailSender, ISMSSender smsSender)
		{
			EmailSender = emailSender;
			SMSSender = smsSender;
			ProcessTimer = new Timer(DoProcess, null, 10000, 30000);
			Logger = LogManager.GetCurrentClassLogger();
		}

		/// <summary>
		/// Выполняет срабатывание таймера
		/// </summary>
		/// <param name="state"></param>
		private void DoProcess(object state)
		{
			if (Processing)
			{
				return;
			}
			Processing = true;
			try
			{
				ExecuteWorker();
			}
			finally
			{
				Processing = false;
			}
		}

		/// <summary>
		/// Таймер выполняет обработку
		/// </summary>
		public bool Processing { get; set; }

		/// <summary>
		/// Логгер
		/// </summary>
		public Logger Logger { get; set; }

		/// <summary>
		/// Таймер выполняющий обработку
		/// </summary>
		public Timer ProcessTimer { get; private set; }

		/// <summary>
		/// Выполяет действия в ходе срабатывания процесса.
		/// </summary>
		protected void ExecuteWorker()
		{
			var dc = new SilkLaserDataContext();

			try
			{
				// Берем те элементы что лежат в очереди
				var queuedItems = dc.WorkerQueueItems.Where(i => !i.Processed).ToList();
				if (queuedItems.Count == 0)
				{
					return;
				}
				Logger.Info(string.Format("Обработка очереди. В очереди {0} элементов", queuedItems.Count));

				// Пробегаемся по очереди
				foreach (var itm in queuedItems)
				{
					itm.DateProcessStart = DateTime.Now;
					switch ((WorkerCommand)itm.ItemType)
					{
						case WorkerCommand.Email:
							ProcessEmailCommand(itm);
							break;
						case WorkerCommand.SMS:
							ProcessSMSCommand(itm);
							break;
						case WorkerCommand.Push:
							ProcessPushCommand(itm);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					itm.DateProcessEnd = DateTime.Now;
					itm.Processed = true;
					dc.SubmitChanges();
				}

				GC.Collect();
				Logger.Info("Обработка очереди завершена");
			}
			catch (Exception e)
			{
				Logger.Error(e);
				Logger.Fatal(e);
			}
			
		}

		/// <summary>
		/// Отправляет SMS уведомление с параметрами
		/// </summary>
		/// <param name="itm"></param>
		private void ProcessSMSCommand(WorkerQueueItem itm)
		{
			var data = JsonConvert.DeserializeObject<SMSNotificationData>(itm.Data);
			Logger.Trace(string.Format("Отправка СМС уведомления для {0} с содержанием \"{1}\"", data.Recipient, data.Content));

			SMSSender.Send(data.Recipient, data.Content);

			Logger.Trace(string.Format("СМС уведомление для {0} успешно отправлено", data.Recipient));
		}

		/// <summary>
		/// Отправляет Email уведомление с параметрами
		/// </summary>
		/// <param name="itm">Элемент рабочей очереди</param>
		private void ProcessEmailCommand(WorkerQueueItem itm)
		{
			var data = JsonConvert.DeserializeObject<EmailNotificationData>(itm.Data);
			Logger.Trace(string.Format("Отправка Email сообщения на {0} с заголовком \"{1}\"", data.Recipient, data.Subject));

			EmailSender.Send(data.Recipient, data.Subject, data.Body);

			Logger.Trace(string.Format("Email сообщение для {0} успешно отправлено", data.Recipient));
		}

		/// <summary>
		/// Отправляет PUSH уведомление используя службы Push уведомлений
		/// </summary>
		/// <param name="itm"></param>
		private void ProcessPushCommand(WorkerQueueItem itm)
		{
			
		}

		/// <summary>
		/// Инициализирует воркер
		/// </summary>
		public void Init()
		{
			
		}
	}
}