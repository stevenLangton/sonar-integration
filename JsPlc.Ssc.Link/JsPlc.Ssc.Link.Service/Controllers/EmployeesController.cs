using System.Linq;
using System.Web.Http;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class EmployeesController : BaseController
    {

       // GET: api/Employees/?managerId=1
        [HttpGet]
        public IHttpActionResult GetTeam([FromUri]string managerId)
        {
            var employees = _db.GetTeam(managerId);

            if(!employees.Any())
                return NotFound();

            return Ok(employees);
        }

        // GET: api/Employees/?UserName="vasu.b@sainsburys.co.uk"
        [HttpGet]
        public bool IsManager([FromUri]string userName)
        {
            return  _db.IsManager(userName);
        }
    }
}
