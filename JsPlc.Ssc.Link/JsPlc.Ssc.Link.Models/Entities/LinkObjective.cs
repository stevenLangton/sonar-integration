using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class LinkObjective
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ColleagueId { get; set; } // Use E001 etc (so that we can store EmployeeId without they having registered as LinkUser)

        [Required]
        public string ManagerId { get; set; } // Use E0010 etc (so that we can store EmployeeId)
      
        public ObjectiveStatus ColleagueSignOff { get; set; }

        public ObjectiveStatus ManagerSignOff { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? SignOffDate { get; set; }

        [Required]        
        public DateTime LastAmendedDate { get; set; }

        [Required]
        public string LastAmendedBy { get; set; }

        public string Objective { get; set; } 

        public string MeasuredBy { get; set; }

        public string RelevantTo { get; set; }

        [Required]
        public string Title { get; set; } 
    }

    public enum ObjectiveStatus
    {
        Approved = 1,
        Draft = 0
    }

    
}