﻿using AutoMapper;
using JsPlc.Ssc.Link.Core.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace JsPlc.Ssc.Link.IoC
{
	/// <summary>
	/// Sets up the application startup state.
	/// </summary>
	public static class Bootstrapper
	{
		public static IUnityContainer Container { get; private set; }

		/// <summary>
		/// Sets up the dependency injection container and 
		/// the global service locator.
		/// </summary>
		/// <param name="container">The container used for dependency injection.</param>
		public static void ConfigureIoC(IUnityContainer container)
		{
			Container = container;

			//TO DO: add shared configuration logic when required

			ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
		}

		/// <summary>
		/// Sets up the automapper mappings.
		/// </summary>
		public static void SetupAutoMapper()
		{
			AppDomain.CurrentDomain.GetAssemblies()
				.Where(assembly => assembly.FullName.ToUpper().StartsWith("JSPLC"))
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => type.GetInterfaces().Contains(typeof(IAutoMapperInitialise)))
				.Where(type => !type.IsAbstract)
				.Select(type => ServiceLocator.Current.GetInstance(type) as IAutoMapperInitialise)
				.Where(initialiser => initialiser != null)
				.ToList()
				.ForEach(initialiser => initialiser.Initialise());

			//TODO: Stop using this obsolete operation.
			Mapper.AssertConfigurationIsValid();
		}
	}
}
