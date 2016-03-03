using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.ModelBinding;

namespace JsPlc.Ssc.Link.Portal
{
    public class BindersConfig
    {
        [ExcludeFromCodeCoverage]
        public static void RegisterModelBinders()
        {
            ModelBinders.Binders[typeof(MeetingView)] = new MeetingViewModelBinder();
        }
    }
}
