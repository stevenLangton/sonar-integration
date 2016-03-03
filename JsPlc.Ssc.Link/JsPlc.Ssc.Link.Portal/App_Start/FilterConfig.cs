using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;

namespace JsPlc.Ssc.Link.Portal
{
    public class FilterConfig
    {
        [ExcludeFromCodeCoverage]
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
