namespace JsPlc.Ssc.Link.Models
{
    public class UserView
    {
        public string Id { get; set; } // UserTable UserId - Guid
 
        //public string ColleagueId { get; set; }

        //public string FirstName { get; set; }

        //public string LastName { get; set; }

        //public string EmailAddress { get; set; }

        public EmployeeView Colleague { get; set; }

        public bool IsLineManager { get; set; } // Checked by Api
    }
}