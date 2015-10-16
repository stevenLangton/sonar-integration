using System.ComponentModel.DataAnnotations;

namespace JsPlc.Ssc.Link.Models
{
    public class QuestionView
    {
        public int QuestionId { get; set; }

        public string Question { get; set; }

        public string QuestionType { get; set; }

        public int? AnswerId { get; set; }

       [StringLength(maximumLength: 5000, ErrorMessage = "Colleague comment cannot be more than 5000 chars.")]
        public string ColleagueComment { get; set; }

       [StringLength(maximumLength: 5000, ErrorMessage = "Manager comment cannot be more than 5000 chars.")]
       public string ManagerComment { get; set; }
    }
}