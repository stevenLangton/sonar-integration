using System.Collections.Generic;

namespace JsPlc.Ssc.Link.Models
{
    public class TeamView
    {
        // suggestion: why not have full EmployeeView object in here instead of fields (as we have in UserView object)
        public int EmployeeId { get; set; }

        public string ColleagueId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public List<LinkMeetingView> Meetings { get; set; }

    }
}