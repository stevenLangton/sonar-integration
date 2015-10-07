using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JsPlc.Ssc.Link.Portal.Models.MockData
{
    public static class MockData
    {
        public static LinkForm MockLinkForm()
        {
            return new LinkForm
            {
                Id = 0,
                MeetingDate = DateTime.Now,
                Completed = false,
                Questions = new List<Question>()
                {
                    new Question{ Id = 0, Description = "Question 1?", PeriodId = 0, Answer = null},
                    new Question{ Id = 1, Description = "Question 2?", PeriodId = 0, Answer = null},
                    new Question{ Id = 2, Description = "Question 3?", PeriodId = 0, Answer = null}
                }                                     
            };
        }
    }
}