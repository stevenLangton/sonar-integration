using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace JsPlc.Ssc.Link.Models
{
    public class MeetingView
    {
        [Required]
        public int MeetingId { get; set; } // DB id

        //public int EmployeeId { get; set; } // DB id, no such thing now..
        [Required]
        public string ColleagueId { get; set; } // E001 etc..

        public string ColleagueName { get; set; }

        [Required]
        public string ManagerId { get; set; } // M001 etc..

        public string ManagerName { get; set; }

        [Required]
        public MeetingStatus ColleagueSignOff { get; set; }

        [Required]
        public MeetingStatus ManagerSignOff { get; set; }

        [Required]
        public DateTime MeetingDate { get; set; }

        public DateTime? ColleagueSignedOffDate { get; set; }

        public DateTime? ManagerSignedOffDate { get; set; }

        // This field actually says, who's viewing this and is only a UI field, not persisted..
        public bool ColleagueInitiated { get; set; } // defaults to false so safer to use instead of ManagerInitiated

        [Required]
        public MeetingSharingStatus SharingStatus { get; set; } 

        public DateTime? SharingDate { get; set; }

        public IEnumerable<QuestionView> Questions { get; set; }

        public ColleagueView Colleague { get; set; }
        public ColleagueView Manager { get; set; }
    }
}