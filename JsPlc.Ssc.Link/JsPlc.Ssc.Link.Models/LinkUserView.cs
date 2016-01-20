namespace JsPlc.Ssc.Link.Models
{
    public class LinkUserView
    {
        public string UserId { get; set; } // A Guid.. unsure why needed..

        public ColleagueView Colleague { get; set; }

        public bool IsLineManager { get; set; } // Checked by Api
    }
}