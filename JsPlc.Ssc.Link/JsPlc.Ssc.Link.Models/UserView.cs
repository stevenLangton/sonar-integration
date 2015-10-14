using System.Collections.Generic;

namespace JsPlc.Ssc.Link.Models
{
    public class UserView
    {
        public string Id { get; set; }

        public string ColleagueId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public bool IsLineManager { get; set; }
    }
}