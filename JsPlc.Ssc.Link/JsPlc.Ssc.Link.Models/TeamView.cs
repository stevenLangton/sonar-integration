using System.Collections.Generic;

namespace JsPlc.Ssc.Link.Models
{
    public class ColleagueTeamView
    {
        public ColleagueView Colleague { get; set; }
        public List<LinkMeetingView> Meetings { get; set; }
    }
}