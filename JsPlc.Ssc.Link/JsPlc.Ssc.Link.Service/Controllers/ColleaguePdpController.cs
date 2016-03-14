using System.Web.Http;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class ColleaguePdpController : BaseController
    {
        // Same routes as PdpController (which is now deprecated in favour of this approach)
        [HttpGet]
        [Route("colleagues/{colleagueId}/pdp")]
        public IHttpActionResult GetPdp(string colleagueId)
        {
            ColleaguePdp pdp = _dbColleaguePdp.GetPdp(colleagueId);
            return Ok(pdp);
        }

        [HttpPut] 
        [Route("colleagues/{colleagueId}/pdp", Name = "PdpUrl")]
        public IHttpActionResult PutPdp(ColleaguePdp pdp)
        {
            ColleaguePdp cpdp = _dbColleaguePdp.UpdatePdp(pdp);
            return Ok(cpdp);
        }
    }
}