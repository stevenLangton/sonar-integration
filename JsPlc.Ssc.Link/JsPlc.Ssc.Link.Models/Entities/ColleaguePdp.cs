using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class ColleaguePdp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ColleagueId { get; set; }

        [Required]
        public PdpVersion PdpVersion { get; set; }

        [Required]
        public Period Period { get; set; }

        [Required]
        public bool Shared { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime? LastModified { get; set; }
    }
}
