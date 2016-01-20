using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.StubService.StubModels
{
    /// <summary>
    /// Reflects definition of Colleague in Enterprise Services (Id might disappear, so dont rely on it)
    /// </summary>
    public class StubColleague
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ColleagueId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ManagerId { get; set; }
    }
}