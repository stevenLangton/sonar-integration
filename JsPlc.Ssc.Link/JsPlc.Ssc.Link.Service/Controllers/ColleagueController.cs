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

        [HttpGet] // api/Colleague/vasundhara.b@sainsburys.co.uk
        [Route("api/ColleagueByEmail/{email}")]
        public IHttpActionResult GetMyDetailsByEmail([FromUri]string email)
        {
            var colleague = _dbColleagues.GetColleagueByEmail(email);

             if(colleague==null)
                return NotFound();

            return Ok(colleague);
        }

        [HttpGet] // api/IsManagerByEmail/vasundhara.b@sainsburys.co.uk
        [Route("api/IsManagerByEmail/{email}")]
        public bool IsManagerByEmail([FromUri]string email)
        {
            return _dbColleagues.IsManagerByEmail(email);
        }

        
    }
}
