﻿using System.Web.Http;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class MeetingsController : BaseController
    {
        public MeetingsController() { }

        public MeetingsController(ILinkRepository repository) : base(repository) { }

        [HttpGet] //api/Meetings/10
        public IHttpActionResult GetMeeting(int id)
        {
            var meeting = _db.GetMeeting(id);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        [HttpGet] //newMeeting/E001
        [Route("newmeeting/{colleagueId}")]
        public IHttpActionResult CreateMeeting(string colleagueId)
        {
            var meeting = _db.CreateMeeting(colleagueId);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        [HttpPost] // POST: api/Meetings
        public IHttpActionResult SaveMeeting(MeetingView meetingView )
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            return Ok(_db.SaveMeeting(meetingView));

            //return CreatedAtRoute("api/meetings", new {id=meetingView.MeetingId}, meetingView);
        }

        [HttpPut] // PUT: api/Meetings/5
        public IHttpActionResult UpdateMeeting( MeetingView meetingView)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _db.UpdateMeeting(meetingView);

            return Ok();
        }
    }
}
