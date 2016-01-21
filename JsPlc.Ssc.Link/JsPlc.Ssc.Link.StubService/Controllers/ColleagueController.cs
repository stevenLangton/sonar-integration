﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JsPlc.Ssc.Link.StubService.StubModels;
using JsPlc.Ssc.Link.StubService.StubInterfaces;
using Microsoft.Owin.Security.Provider;

namespace JsPlc.Ssc.Link.StubService.Controllers
{
    public class ColleagueController : BaseController
    {
        public ColleagueController() { }

        public ColleagueController(IStubLinkRepository repository) : base(repository) { }

        /// <summary>
        /// Get ColleagueProfile by their empId
        ///  api/colleague/E0002
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet] //
        [Route("api/Colleague/{id}")]
        public IHttpActionResult GetColleague([FromUri] string id)
        {
            StubColleague stubColleague = _db.GetColleague(id);

            if (stubColleague == null)
                return NotFound();

            return Ok(stubColleague);
        }

        /// <summary>
        /// Get ColleagueProfile by colleague's email
        /// api/colleague/?email=first.last@sainsburys.co.uk
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet] 
        [Route("api/ColleagueByEmail/{email}")]
        public IHttpActionResult GetColleagueByEmail([FromUri] string email)
        {
            StubColleague stubColleague = _db.GetColleagueByEmail(email);

            if (stubColleague == null)
                return NotFound();

            return Ok(stubColleague);
        }

        /// <summary>
        /// Get Direct Reports by manager's empId
        /// api/DirectReports/E0010
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/DirectReports/{managerId}")] // api/DirectReports/E0010
        public IHttpActionResult GetDirectReports(string managerId)
        {
            List<StubColleague> colleagues = _db.GetDirectReports(managerId);
            if (colleagues == null)
            {
                return NotFound();
            }
            var directReports = colleagues as IList<StubColleague>;

            if (!directReports.Any())
                return NotFound();

            return Ok(directReports);
        }

        /// <summary>
        /// Get Direct Reports by manager email 
        /// api/directReports/?email=first.last@sainsburys.co.uk
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/DirectReportsByEmail/{email}")] 
        public IHttpActionResult GetDirectReportsByEmail(string email)
        {
            List<StubColleague> colleagues = _db.GetDirectReportsByManagerEmail(email);
            if (colleagues == null)
            {
                return NotFound();
            }

            var directReports = colleagues as IList<StubColleague>;

            if (!directReports.Any())
                return NotFound();

            return Ok(directReports);
        }

        [HttpGet]
        [Route("api/IsManager/{colleagueId}")] // api/IsManager/E0010
        public IHttpActionResult IsManager(string colleagueId)
        {
            var isMgr = _db.IsManager(colleagueId);
            return Ok(isMgr);
        }

        [HttpGet]
        [Route("api/IsManagerByEmail/{email}")] // api/IsManagerByEmail/Luan.Au@linktool.onmicrosoft.com
        public IHttpActionResult IsManagerByEmail(string email)
        {
            var isMgr = _db.IsManagerByEmail(email);
            return Ok(isMgr);
        }
    }
}
