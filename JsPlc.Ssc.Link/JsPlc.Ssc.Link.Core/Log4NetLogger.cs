using JsPlc.Ssc.Link.Core.Interfaces;
using log4net;
using System;

namespace JsPlc.Ssc.Link.Core
{
	public class Log4NetLogger : ILogger
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(Log4NetLogger));

		public void Error(object message, Exception exception)
		{
			_logger.Error(message, exception);
		}

		public void Error(object message)
		{
			_logger.Error(message);
		}

		public void Info(object message, Exception exception)
		{
			_logger.Info(message, exception);
		}

		public void Info(object message)
		{
			_logger.Info(message);
		}

		public void InfoFormat(string format, params object[] args)
		{
			_logger.InfoFormat(format, args);
		}
	}
}
