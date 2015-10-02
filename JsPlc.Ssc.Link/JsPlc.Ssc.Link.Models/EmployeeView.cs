namespace JsPlc.Ssc.Link.Models
{
    public class EmployeeView
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public int ManagerId { get; set; }

        public string ManagerName { get; set; }
    }
}