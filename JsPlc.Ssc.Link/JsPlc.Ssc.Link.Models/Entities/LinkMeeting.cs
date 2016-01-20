using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class LinkMeeting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; } // E001 etc (so that we can store EmployeeId without they having registered as LinkUser)

        [Required]
        public string ManagerId { get; set; } // E0010 etc (same reason as above)
      
        [Required]
        public MeetingStatus ColleagueSignOff { get; set; }

        [Required]
        public MeetingStatus ManagerSignOff { get; set; }
        
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