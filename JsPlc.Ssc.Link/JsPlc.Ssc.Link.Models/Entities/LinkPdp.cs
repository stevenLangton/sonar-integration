using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class LinkPdp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ColleagueId { get; set; } 

        [Required]
        public bool signOff { get; set; }

        public string achieveObjectives { get; set; }

        public string achieveObjectivesActions { get; set; }

        public string achieveObjectivesWhen { get; set; }

        public string keyStrengths { get; set; }

        public string keyStrengthsActions { get; set; }

        public string keyStrengthsWhen { get; set; }

        public string careerAspirations { get; set; }

        public string careerAspirationsActions { get; set; }

        public string careerAspirationsWhen{ get; set; }

    }
}
