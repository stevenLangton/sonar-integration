namespace JsPlc.Ssc.Link.Models
{
    public class ColleagueView
    {
        //public int LinkUserId { get; set; } // Internal Link User Id

        public string ColleagueId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string KnownAsName { get; set; }
        public string EmailAddress { get; set; }

        public string Department { get; set; }
        public string Grade { get; set; }
        public string Division { get; set; }

        public string ManagerId { get; set; }
        public bool HasManager { get; set; }

        public ColleagueView Manager { get; set; }
        //public string ManagerName { get; set; }
    }
}