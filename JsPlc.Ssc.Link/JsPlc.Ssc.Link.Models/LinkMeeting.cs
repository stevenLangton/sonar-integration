using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Remoting.Contexts;

namespace JsPlc.Ssc.Link.Models
{
    public class LinkMeeting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; } 
        
        public int PeriodId { get; set; }

        [Required]
        public MeetingStatus Status { get; set; }

        [Required]
        public DateTime MeetingDate { get; set; }

        public ICollection<Answer> Answers { get; set; } 
    }

    public enum MeetingStatus
    {
        Completed = 1,
        InComplete = 0
    }
}