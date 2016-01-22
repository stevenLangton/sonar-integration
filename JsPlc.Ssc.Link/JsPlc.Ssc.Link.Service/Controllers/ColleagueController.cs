using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    
    public class ColleagueController : BaseController
    {
        public ColleagueController() { }

        public ColleagueController(ILinkRepository repository) : base(repository) { }

        [HttpGet] // api/Colleague/E002
        [Route("api/Colleague/{id}")]
        public IHttpActionResult GetMyDetailsById([FromUri]string id)
        {
            var colleague = _dbColleagues.GetColleague(id);

             if(colleague==null)
                return NotFound();

            return Ok(colleague);
        }

        [HttpGet] // api/Colleague/vasundhara.b@sainsburys.co.uk
        [Route("api/ColleagueByEmail/{email}")]
        public IHttpActionResult GetMyDetailsByEmail([FromUri]string email)
        {
            var colleague = _dbColleagues.GetColleagueByEmail(email);

            if (colleague == null)
                return NotFound();

            return Ok(colleague);
        }

        [HttpGet] // api/DirectReports/E0010
        [Route("api/DirectReports/{managerId}")]
        public IHttpActionResult GetDirectReports([FromUri]string managerId)
        {
            return Ok(_dbColleagues.GetDirectReports(managerId));
        }

        [HttpGet] // api/DirectReports/E0010
        [Route("api/DirectReportsByEmail/{email}")]
        public IHttpActionResult GetDirectReportsByEmail([FromUri]string email)
        {
            return Ok(_dbColleagues.GetDirectReportsByManagerEmail(email));
        }

        [HttpGet] // api/IsManager/{id}
        [Route("api/IsManager/{id}")]
        public bool IsManagerById([FromUri]string id)
        {
            return _dbColleagues.IsManager(id);
        }

        [HttpGet] // api/IsManagerByEmail/vasundhara.b@sainsburys.co.uk
        [Route("api/IsManagerByEmail/{email}")]
        public bool IsManagerByEmail([FromUri]string email)
        {
            return _dbColleagues.IsManagerByEmail(email);
        }

        
    }
}
