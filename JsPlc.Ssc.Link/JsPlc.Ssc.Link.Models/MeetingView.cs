using System;
using System.Collections.Generic;

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

        public int PeriodId { get; set; }

        public string PeriodDescription { get; set; }
        
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public MeetingStatus Status { get; set; }

        public DateTime MeetingDate { get; set; }

        public IEnumerable<QuestionView> Questions { get; set; }
    }
}