using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JsPlc.Ssc.Link.Portal.Models
{
    public class LinkForm
    {
        public int Id { get; set; }
        public bool Completed { get; set; }
        public DateTime MeetingDate { get; set; }

        public List<Question> Questions { get; set; }
    }
}