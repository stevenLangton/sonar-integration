using JsPlc.Ssc.Link.Core.Interfaces;
using JsPlc.Ssc.Link.ImportRoutine.Interfaces;
using JsPlc.Ssc.Link.IoC;
using System;
using System.ServiceProcess;


namespace JsPlc.Ssc.Link.ImportRoutine.Service
{
	
    static class Program
    {
		/// <summary>
        /// The main entry point for the application.
        /// </summary>
		static void Main(string[] args)
        {
			//setup unity configuration
			UnityConfig.RegisterComponents(DependencyFactory.Container);

			ILogger logger = DependencyFactory.Resolve<ILogger>();

			IDataLoader dataLoader = DependencyFactory.Resolve<IDataLoader>();
			IDataTransformer dataTransformer = DependencyFactory.Resolve<IDataTransformer>();
			IFileProcessor fileProcessor = DependencyFactory.Resolve<IFileProcessor>();

			var service = new ImportService(logger, dataLoader, dataTransformer, fileProcessor);

			if (Environment.UserInteractive)
			{
				service.RunOnStart(args);
				Console.WriteLine("Press any key to stop program");
				Console.Read();
				service.RunOnStop();
			}
			else
			{
				ServiceBase.Run(service);
			}
        }
    }
}
