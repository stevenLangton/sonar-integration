using System.Collections.Generic;

namespace JsPlc.Ssc.Link.Models
{
    public class TeamView
    {
        public int Id { get; set; }

        public string ColleagueId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public List<LinkMeetingView> Meetings { get; set; }

    }
}