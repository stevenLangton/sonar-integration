using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace JsPlc.Ssc.Link.Service.Filters
{
	public class GlobalActionExecutedExceptionFilter : ExceptionFilterAttribute
	{
		private static readonly ILog log = LogManager.GetLogger("GlobalActionExecutedEx");

		public override void OnException(HttpActionExecutedContext context)
		{
			log.Error(context.Exception);
		}
	}
}