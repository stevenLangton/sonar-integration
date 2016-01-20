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

        public MeetingStatus ColleagueSignOff { get; set; }

        public MeetingStatus ManagerSignOff { get; set; }

        public MeetingStatus Status {
            get { return ColleagueSignOff==MeetingStatus.Completed && ManagerSignOff ==MeetingStatus.Completed ? MeetingStatus.Completed : MeetingStatus.InComplete; }
        }
        
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime MeetingDate { get; set; }
    }
}