using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace JsPlc.Ssc.Link.Service
{
    public class ElmahHandleWebApiErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var e = context.Exception;
            RaiseErrorSignal(e);
        }

        private static bool RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null)
                return false;
            var signal = Elmah.ErrorSignal.FromContext(context);
            if (signal == null)
                return false;
            signal.Raise(e, context);
            return true;
        }
    }
}