using System.Linq;
using System.Web.Http;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class EmployeesController : BaseController
    {

        // GET: api/Employees/5
        [HttpGet]
        public IHttpActionResult GetEmployee(string id)
        {
            var employee = _db.GetEmployee(id);
            
            if(employee==null)
                return NotFound();

            return Ok(employee);
        }

        // GET: api/Employees/?managerId=1
        [HttpGet]
        public IHttpActionResult GetTeam([FromUri]string managerId)
        {
            var employees = _db.GetTeam(managerId);

            if(employees==null)
                return NotFound();

            return Ok(employees);
        }

       
    }
}
