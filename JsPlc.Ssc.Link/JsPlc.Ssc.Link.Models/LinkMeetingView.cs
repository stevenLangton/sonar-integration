using System;
using System.ComponentModel.DataAnnotations;

namespace JsPlc.Ssc.Link.Models
{
    public class LinkMeetingView
    {
        public int Id { get; set; }

        public MeetingStatus Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime MeetingDate { get; set; }
    }
}