using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace JsPlc.Ssc.Link.Service.Models
{
    public class ObjectiveAdd
    {

        public string LastAmendedByColleagueId { get; set; }
        public string Objective { get; set; }
    }


    public class ObjectiveUpdate : ObjectiveAdd
    {
        public bool ColleagueSignOff { get; set; }
        public bool ManagerSignOff { get; set; }
        public DateTime? SignOffDate { get; set; } 
    }

}