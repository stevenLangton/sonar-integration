using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    // Now ColleaguePdp
    public class LinkPdp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ColleagueId { get; set; } 

        [Required]
        public bool signOff { get; set; }

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

    }
}
