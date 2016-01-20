using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    /// <summary>
    /// This Entity will be the mapping between Link and HR.
    /// All Link Entities will be mapped to the LinkUser.Id (Foreign Key). 
    /// Dont need this table, we'll key everything by ColleagueId which we get from AAD/ColleagueService
    /// </summary>

    //public class LinkUser
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }

    //    public string ColleagueId { get; set; } // We need to store ColleagueId as not likely to change

    //    public string EmailAddress { get; set; } // Ideally should not store email addr. as it might change. (e.g. emp gets married or changes name etc.)

    //    public ICollection<LinkMeeting> Meetings { get; set; }
    //}
}