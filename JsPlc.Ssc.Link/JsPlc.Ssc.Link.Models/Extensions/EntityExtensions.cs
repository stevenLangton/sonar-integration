using System;
using System.Collections.Generic;
using System.Linq;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Portal.Helpers.Extensions
{
    public static class EntityExtensions
    {
        public static MeetingView ToMeetingView(this LinkMeeting linkMeeting)
        {
            var retval = new MeetingView
            {
                ColleagueId = linkMeeting.ColleagueId,
                ColleagueSignOff = linkMeeting.ColleagueSignOff,
                ManagerId = linkMeeting.ManagerId,
                ManagerSignOff = linkMeeting.ManagerSignOff,
                MeetingDate = linkMeeting.MeetingDate,
                ColleagueSignedOffDate = linkMeeting.ColleagueSignedOffDate,
                ManagerSignedOffDate = linkMeeting.ManagerSignedOffDate,
                MeetingId = linkMeeting.Id,
            };
            return retval;
        }
    }
}