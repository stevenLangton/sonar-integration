using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.StubService.StubModels
{
    public class ColleagueDto
    {
        public string ColleagueId { get; set; } // Key to Meetings, Objectives, Pdps

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string KnownAsName { get; set; }
        public string EmailAddress { get; set; }

        public string Department { get; set; }
        public string Grade { get; set; }
        public string Division { get; set; }

        public string ManagerId { get; set; } 
        public bool HasManager { get; set; } // Allows lazy loading..

        public ColleagueDto Manager { get; set; }
    }
}