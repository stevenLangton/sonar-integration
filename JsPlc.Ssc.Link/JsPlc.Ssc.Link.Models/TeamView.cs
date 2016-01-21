using System.Collections.Generic;

namespace JsPlc.Ssc.Link.Models
{
    public class TeamView
    {
        public ColleagueView Colleague { get; set; }
        public List<LinkMeetingView> Meetings { get; set; }
    }
}