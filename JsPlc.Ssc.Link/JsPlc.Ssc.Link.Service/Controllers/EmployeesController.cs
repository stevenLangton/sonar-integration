using System.Web.Http;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class EmployeesController : BaseController
    {

        // GET: api/Employees/5
        public IHttpActionResult GetEmployee(int id)
        {
            return Ok(_db.GetEmployee(id));
        }

        // GET: api/Employees/?managerId=1
        public IHttpActionResult GetEmployees([FromUri]int managerId)
        {
            return Ok(_db.GetEmployees(managerId));
        }

       
    }
}
