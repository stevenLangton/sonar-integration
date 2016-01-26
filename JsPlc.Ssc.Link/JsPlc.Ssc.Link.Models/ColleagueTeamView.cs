using System.Collections.Generic;

namespace JsPlc.Ssc.Link.Models
{
    public class ColleagueTeamView
    {
        public ColleagueView Colleague { get; set; }
        public List<LinkMeetingView> Meetings { get; set; }

        public LinkMeetingView LatestMeeting { get; set; } // For the UI: Current Link Meeting: or Upcoming Meeting: 
        public LinkMeetingView LastMeeting { get; set; } // For the UI text: Last Meeting: November 16th 2016
        public int MeetingsInLast12Months { get; set; }
    }
}