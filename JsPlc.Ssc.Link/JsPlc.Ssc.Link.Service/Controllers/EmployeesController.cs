using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class EmployeesController : BaseController
    {

       // GET: api/Employees/?emailAddress=vasu.b@sainsburys.co.uk
        [HttpGet]
        public IHttpActionResult GetEmployee([FromUri]string emailAddress)
        {
            var employee = _db.GetEmployee(emailAddress);

             if(employee==null)
                return NotFound();

            return Ok(employee);
        }

        [HttpGet]
        public IHttpActionResult GetTeam([FromUri]string managerId)
        {
            var employees = _db.GetTeam(managerId);

            var teamViews = employees as IList<TeamView> ?? employees.ToList();

            if(!teamViews.Any())
                return NotFound();

            return Ok(teamViews);
        }

        // GET: api/Employees/?UserName="vasu.b@sainsburys.co.uk"
        [HttpGet]
        public bool IsManager([FromUri]string userName)
        {
            return  _db.IsManager(userName);
        }
    }
}
