﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class MeetingsController : BaseController
    {
        public MeetingsController() { }

        //public MeetingsController(ILinkRepository repository) : base(repository) { }

        public MeetingsController(IMeetingService repository) : base(repository) { }

        public MeetingsController(//ILinkRepository repository, 
            IMeetingService meetingService, IColleagueService colleagueService) : 
            base(//repository, 
            meetingService, null, colleagueService) { }

        [HttpGet] //api/Meetings/10
        public IHttpActionResult GetMeeting(int id)
        {
            var meeting = _dbMeeting.GetMeeting(id);

            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(meeting);
        }

        [HttpGet] //newMeeting/E001
        [Route("newmeeting/{colleagueId}")]
        public IHttpActionResult CreateMeeting(string colleagueId)
        {
            var meeting = _dbMeeting.CreateMeeting(colleagueId);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        [HttpPost] // POST: api/Meetings
        public IHttpActionResult SaveMeeting(MeetingView meetingView)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_dbMeeting.SaveNewMeeting(meetingView));

            //return CreatedAtRoute("api/meetings", new {id=meetingView.MeetingId}, meetingView);
        }

        [HttpGet] // GET: api/UnshareMeeting/5
        [Route("api/UnshareMeeting/{meetingId}")] 
        public IHttpActionResult UnshareMeeting(int meetingId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var meeting = _dbMeeting.UnshareMeeting(meetingId);

            return Ok(meeting);
        }

        [HttpPut] // PUT: api/Meetings/5
        public IHttpActionResult UpdateMeeting( MeetingView meetingView)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _dbMeeting.UpdateMeeting(meetingView);

            return Ok();
        }

        [HttpGet]
        [Route("mymeetings/{colleagueId}")] // mymeetings/E001
        public IHttpActionResult GetMyMeetings(string colleagueId)
        {
            var meetings = _dbMeeting.GetColleagueAndMeetings(colleagueId);

            if (meetings == null)
                return NotFound();

            return Ok(meetings);
        }

        [HttpGet]
        [Route("mymeetings/{colleagueId}/NextInFuture")] // mymeetings/E001/NextInFuture
        public IHttpActionResult GetNextMeeting(string colleagueId)
        {
            var meeting = _dbMeeting.GetNextMeeting(colleagueId);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        [HttpGet]
        [Route("myteam/{managerId}")] // myteam/E0010
        public IHttpActionResult GetMyTeam(string managerId)
        {
            IEnumerable<ColleagueTeamView> colleaguesAndMeetings = _dbMeeting.GetTeam(managerId);

            var teamViews = colleaguesAndMeetings as IList<ColleagueTeamView> ?? colleaguesAndMeetings.ToList();

            if (!teamViews.Any())
                return NotFound();

            return Ok(teamViews);
        }
    }
}
