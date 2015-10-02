using System;

namespace JsPlc.Ssc.Link.Models
{
    public class MeetingView
    {
        public int MeetingId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int PeriodId { get; set; }
        public string PeriodDescription { get; set; }
        public MeetingStatus Status { get; set; }
        public DateTime MeetingDate { get; set; }
        public int QuestionId { get; set; }
        public string ColleagueComments { get; set; }
        public string ManagerComments { get; set; }
    }
}