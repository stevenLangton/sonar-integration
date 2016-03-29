using JsPlc.Ssc.Link.Core;
using JsPlc.Ssc.Link.Core.Interfaces;
using JsPlc.Ssc.Link.ImportRoutine.Interfaces;
using JsPlc.Ssc.Link.IoC;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsPlc.Ssc.Link.ImportRoutine.Service
{
	public static class UnityConfig
	{
		public static void RegisterComponents(IUnityContainer outerContaner)
		{
			var container = outerContaner ?? new UnityContainer();

			//run global project configruation
			Bootstrapper.ConfigureIoC(container);

			container.RegisterType<ILogger, Log4NetLogger>();
			container.RegisterType<IDataLoader, DatabaseDataLoader>();
			container.RegisterType<IDataTransformer, DefaultTransformer>();
			container.RegisterType<IFileProcessor, DdatFileProcessor>();
		}
	}
}
