namespace JsPlc.Ssc.Link.Models
{
    public class QuestionView
    {
        public int QuestionId { get; set; }

        public string Question { get; set; }

        public int? AnswerId { get; set; }

        public string ColleagueComment { get; set; }

        public string ManagerComment { get; set; }
    }
}