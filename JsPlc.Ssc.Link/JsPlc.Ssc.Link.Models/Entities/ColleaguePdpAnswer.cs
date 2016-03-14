using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class ColleaguePdpAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ColleaguePdpSectionInstance ColleaguePdpSectionInstance { get; set; }

        [Required]
        public PdpSectionQuestion PdpSectionQuestion { get; set; }

        [Required]
        public string AnswerText { get; set; }
    }
}
