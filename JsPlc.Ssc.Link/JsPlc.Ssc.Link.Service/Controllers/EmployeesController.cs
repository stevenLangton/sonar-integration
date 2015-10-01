using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class EmployeesController : BaseController
    {
        // GET: api/Employee
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Employee/5
        public IHttpActionResult Get(int id)
        {
            return  Ok(_db.GetEmployee(id));
        }

        // POST: api/Employee
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employee/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Employee/5
        public void Delete(int id)
        {
        }
    }
}
