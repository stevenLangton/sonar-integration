using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class PdpSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public PdpVersion PdpVersion { get; set; }

        //[Required]
        //public int PdpVersionId { get; set; }

        [Required]
        public Section Section { get; set; }

        [Required]
        public int PresentationOrder { get; set; }

        public List<PdpSectionQuestion> Questions { get; set; }

    }
}