using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class ColleaguePdpSectionInstance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ColleaguePdp ColleaguePdp { get; set; }

        [Required]
        public PdpSection PdpSection { get; set; }

        [Required]
        public int InstanceNumber { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public List<ColleaguePdpAnswer> ColleaguePdpAnswers { get; set; } 
    }
}
