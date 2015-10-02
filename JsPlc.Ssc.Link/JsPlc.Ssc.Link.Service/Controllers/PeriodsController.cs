using System.Web.Http;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class PeriodsController : BaseController
    {
        // GET: api/Periods
        public IHttpActionResult Get()
        {
            return Ok(_db.GetPeriods());
        }
    }
}
