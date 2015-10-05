using System;
using System.Collections.Generic;

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
        
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public MeetingStatus Status { get; set; }

        public DateTime MeetingDate { get; set; }

        public IEnumerable<QuestionView> Questions { get; set; }
    }
}