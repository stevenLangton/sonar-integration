using System;
using System.ComponentModel.DataAnnotations;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Models
{
    public class LinkMeetingView
    {
        public int MeetingId { get; set ; }

        public string Year { get; set; }

        public string Period { get; set; }

        public string ColleagueId { get; set; }

        public string ManagerAtTimeId { get; set; } // Manager at that time when meeting was conducted..

        public ColleagueView Colleague { get; set; }

        public ColleagueView ManagerAtTime { get; set; } // Manager at that time when meeting was conducted..

        public MeetingStatus ColleagueSignOff { get; set; }

        public MeetingStatus ManagerSignOff { get; set; }

        public MeetingStatus Status {
            get { return ColleagueSignOff==MeetingStatus.Completed && ManagerSignOff ==MeetingStatus.Completed ? MeetingStatus.Completed : MeetingStatus.InComplete; }
        }
        
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime MeetingDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ColleagueSignedOffDate { get; set; }
    
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ManagerSignedOffDate { get; set; }
    }
}