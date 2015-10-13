using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JsPlc.Ssc.Link.Portal.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Answer Answer { get; set; }
    }
}