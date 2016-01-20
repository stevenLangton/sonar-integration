﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    
    public class EmployeesController : BaseController
    {

        public EmployeesController() { }

        public EmployeesController(ILinkRepository repository) : base(repository) { }

        public EmployeesController(IMeetingService repository) : base(repository) { }

        [HttpGet] // api/employees/?emailaddress=vasundhara.b@sainsburys.co.uk
        public IHttpActionResult GetMyDetails([FromUri]string emailAddress)
        {
            var employee = _dbColleagues.GetColleague(emailAddress);

             if(employee==null)
                return NotFound();

            return Ok(employee);
        }
        
        [HttpGet]
        [Route("mymeetings/{colleagueId}")] // mymeetings/E001
        public IHttpActionResult GetMyMeetings(string colleagueId)
        {
            var meetings = _dbMeeting.GetMeetings(colleagueId);

            if (meetings == null)
                return NotFound();

            return Ok(meetings);
        }

        [HttpGet] // api/employees/?username=vasundhara.b@sainsburys.co.uk
        public bool IsManager([FromUri]string email)
        {
            return _dbColleagues.IsManagerByEmail(email);
        }

        [HttpGet]
        [Route("myteam/{managerId}")] // myteam/E0010
        public IHttpActionResult GetMyTeam(string managerId)
        {
            IEnumerable<TeamView> employees = _dbMeeting.GetTeam(managerId);

            var teamViews = employees as IList<TeamView> ?? employees.ToList();

            if(!teamViews.Any())
                return NotFound();

            return Ok(teamViews);
        }

        
    }
}
