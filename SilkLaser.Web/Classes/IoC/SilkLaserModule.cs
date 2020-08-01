// 
// 
// Solution: SeoDesk
// Project: SeoDesk.Manager.Web
// File: ManagerModule.cs
// 
// Created by: ykors_000 at 12.10.2016 15:14
// 
// Property of SoftGears
// 
// ========

using System.Configuration;
using Autofac;
using SilkLaser.Web.Classes.DAL;
using SilkLaser.Web.Classes.Interfaces.Notifications;
using SilkLaser.Web.Classes.Interfaces.Queue;
using SilkLaser.Web.Classes.Notifications.Senders;
using SilkLaser.Web.Classes.Queue;
using SilkLaser.Web.Classes.Workers;

namespace SilkLaser.Web.Classes.IoC
{
	/// <summary>
	/// Модуль менеджера
	/// </summary>
	public class SilkLaserModule: Autofac.Module
	{
		/// <summary>
		/// Override to add registrations to the container.
		/// </summary>
		/// <remarks>
		/// Note that the ContainerBuilder parameter is unique to this module.
		/// </remarks>
		/// <param name="builder">The builder through which components can be
		///             registered.</param>
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(
				c => new SilkLaserDataContext())
				.AsSelf()
				.InstancePerRequest();
			builder.RegisterType<DBQueueManager>().As<IQueueManager>().InstancePerRequest();
			builder.RegisterType<SimpleMailSender>().As<IEmailSender>();
			builder.RegisterType<SMSPilotSender>().As<ISMSSender>();
			builder.RegisterType<DBQueueWorker>().AsSelf().SingleInstance();
		}
	}
}