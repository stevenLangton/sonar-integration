using JsPlc.Ssc.Link.Core.Interfaces;
using JsPlc.Ssc.Link.ImportRoutine.Interfaces;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;


namespace JsPlc.Ssc.Link.ImportRoutine.Service
{
    public partial class ImportService : ServiceBase
    {
		private Timer _timer;
		private ILogger _logger;
		private IDataLoader _dataLoader;
		private IDataTransformer _dataTransformer;
		private IFileProcessor _fileProcessor;
		private bool _loading;
		private int _minuteOfAnHour = int.Parse(ConfigurationManager.AppSettings["RunAtTheMinuteOfAnHour"]);
		private volatile static object _locker = new object();

        public ImportService(ILogger logger, Interfaces.IDataLoader dataLoader, Interfaces.IDataTransformer dataTransformer, Interfaces.IFileProcessor fileProcessor)
		{
			// TODO: Complete member initialization
			this._logger = logger;
			this._dataLoader = dataLoader;
			this._dataTransformer = dataTransformer;
			this._fileProcessor = fileProcessor;
		}

		public void RunOnStart(string[] args)
		{
			OnStart(args);
		}

		public void RunOnStop()
		{
			OnStop();
		}

        protected override void OnStart(string[] args)
        {
			_logger.Info("Service started");
			_timer = new Timer(10 * 1000);  // 10 seconds - should be less than a minute
			_timer.AutoReset = true;
			_timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
			_timer.Start();
        }

		protected override void OnStop()
        {
			_logger.Info("Service stopped");
			_timer.Stop();
			_timer.Dispose();
        }

		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if(DateTime.Now.Minute == _minuteOfAnHour)
			{
				loadFiles();
			}
		}

		private void loadFiles()
		{
			if (_loading == false)
			{
				lock (_locker)
				{
					if (_loading == false)
					{
						_loading = true;

						try
						{
							//1. - Parsing FIM file
							_fileProcessor.ProcessFIMFile();

							//2. - Parsing AbInitio file
							_fileProcessor.ProcessAbInitioFile();

							var processedData = _fileProcessor.ProcessedData;

							if (processedData.Count > 0)
							{
								_logger.InfoFormat("Loading {0} records", processedData.Count);

								//3. - Transforming data to DataTable
								var transformedData = _dataTransformer.Transform(processedData);

								//4. - Loading data to database
								_dataLoader.Load(transformedData);
							}

							//5. - Moving processed files
							_fileProcessor.MoveFilesToProcessedFolder();
						}
						catch (Exception ex)
						{
							_logger.Error(ex);
						}

						_loading = false;
					}
				}
			}
		}
    }
}
