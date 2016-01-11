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
      
        [Required]
        public ObjectiveStatus ColleagueSignOff { get; set; }

        [Required]
        public ObjectiveStatus ManagerSignOff { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? SignOffDate { get; set; }
        [Required]        
        public DateTime LastAmendedDate { get; set; }
        [Required]
        public int LastAmendedBy { get; set; }

        public string Objective { get; set; } 
    }

    public enum ObjectiveStatus
    {
        Approved = 1,
        Draft = 0
    }

    
}