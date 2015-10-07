using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JsPlc.Ssc.Link.Portal.Models
{

    public class LinkForm
    {
        public bool Complete { get; set; }
        public DateTime LinkDate { get; set; }
        public int Period { get; set; }

        public IEnumerable<Question> questions { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int PeriodId { get; set; }
        public string ColleageComments { get; set; }
        public string ManagerComments { get; set; }
    }
}