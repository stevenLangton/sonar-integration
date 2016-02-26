using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Models
{
    public class MeetingView
    {
        public int MeetingId { get; set; } // DB id

        //public int EmployeeId { get; set; } // DB id, no such thing now..

        public string ColleagueId { get; set; } // E001 etc..

        public string ColleagueName { get; set; }

        public string ManagerId { get; set; } // M001 etc..

        public string ManagerName { get; set; }
      
        public MeetingStatus ColleagueSignOff { get; set; }

        public MeetingStatus ManagerSignOff { get; set; }

        public DateTime MeetingDate { get; set; }

        public DateTime? ColleagueSignedOffDate { get; set; }

        public DateTime? ManagerSignedOffDate { get; set; }

        // This field actually says, who's viewing this and is only a UI field, not persisted..
        public bool ColleagueInitiated { get; set; } // defaults to false so safer to use instead of ManagerInitiated

        public MeetingSharingStatus SharingStatus { get; set; } 

        public DateTime? SharingDate { get; set; }

        public IEnumerable<QuestionView> Questions { get; set; }

        public ColleagueView Colleague { get; set; }
        public ColleagueView Manager { get; set; }
    }
}