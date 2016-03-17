using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    // Now ColleaguePdp in Sprint8Stuff branch
    public class LinkPdp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ColleagueId { get; set; } 

        //[Required]
        //public bool signOff { get; set; }

        [DataType(DataType.MultilineText)]
        public string achieveObjectives { get; set; }

        [DataType(DataType.MultilineText)]
        public string achieveObjectivesActions { get; set; }

        public string achieveObjectivesWhen { get; set; }

        [DataType(DataType.MultilineText)]
        public string keyStrengths { get; set; }

        [DataType(DataType.MultilineText)]
        public string keyStrengthsActions { get; set; }

        public string keyStrengthsWhen { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string careerAspirations { get; set; }

        [DataType(DataType.MultilineText)]
        public string careerAspirationsActions { get; set; }

        public string careerAspirationsWhen{ get; set; }

        // Defaults to 0
        public PdpStatus ColleagueSignOff { get; set; } // Colleague completed 
        public DateTime? ColleagueSignOffDate { get; set; } 

        public PdpStatus ManagerSignOff { get; set; } // Manager Approved
        public DateTime? ManagerSignOffDate { get; set; } 

        public PdpSharingStatus SharingStatus { get; set; } // Shared tickbox
        public DateTime? SharingDate { get; set; }

    }
    public enum PdpStatus
    {
        Completed = 1,
        InComplete = 0
    }

    public enum PdpSharingStatus
    {
        NotShared = 0,
        Shared = 1
    }
}
