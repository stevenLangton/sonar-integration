using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Models
{
    public class MeetingView
    {
        public int MeetingId { get; set; } // DB id

        public int EmployeeId { get; set; } // DB id

        public string ColleagueId { get; set; } // E001 etc..

        public string ColleagueName { get; set; }

        public string ManagerId { get; set; } // M001 etc..

        public string ManagerName { get; set; }
      
        public MeetingStatus ColleagueSignOff { get; set; }

        public MeetingStatus ManagerSignOff { get; set; }

        public DateTime MeetingDate { get; set; }

        public bool ColleagueInitiated { get; set; } // defaults to false so safer to use instead of ManagerInitiated

        public IEnumerable<QuestionView> Questions { get; set; }
    }
}