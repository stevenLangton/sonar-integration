using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsPlc.Ssc.Link.Core.Interfaces
{
	public interface ILogger
	{
		void Error(object message, Exception exception);

		void Error(object message);

		void Info(object message, Exception exception);

		void Info(object message);

		void InfoFormat(string format, params object[] args);
	}
}
