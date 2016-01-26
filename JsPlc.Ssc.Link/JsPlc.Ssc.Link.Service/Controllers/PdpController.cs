using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Service.Models;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class PdpController : BaseController
    {
     

        [Route("colleagues/{colleagueId}/pdp", Name = "PdpUrl")]
        public IHttpActionResult PutPdp(LinkPdp Pdp)
        {
            if (ModelState.IsValid)
            {
                var status = _dbPdp.UpdatePdp(Pdp);

                if (status)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("colleagues/{colleagueId}/pdp")]
        public IHttpActionResult GetPdp(string colleagueId)
        {
            LinkPdp pdp = _dbPdp.GetPdp(colleagueId);
            return Ok(pdp);
        }
    }
}