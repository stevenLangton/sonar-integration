﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ColleagueComments { get; set; }

        public string ManagerComments { get; set; }
        
        public int QuestionId { get; set; }
        
        public int LinkMeetingId { get; set; }
        
    }
}