using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models
{
    public class LinkMeeting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public int PeriodId { get; set; }
        [ForeignKey("PeriodId")]
        public virtual Period Period { get; set; }

        [Required]
        [Display(Name = "Status")]
        public MeetingStatus Status { get; set; }

        [Required]
        public DateTime MeetingDate { get; set; }
    }
    public enum MeetingStatus
    {
        Completed = 1,
        InComplete = 0
    }
}