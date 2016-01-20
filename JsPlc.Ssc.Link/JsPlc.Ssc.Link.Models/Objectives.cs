using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models
{
    public class Objectives
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int  ManagerId { get; set; } 
      
        public ObjectiveStatus ColleagueSignOff { get; set; }

        public ObjectiveStatus ManagerSignOff { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? SignOffDate { get; set; }

        [Required]        
        public DateTime LastAmendedDate { get; set; }

        [Required]
        public int LastAmendedBy { get; set; }

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