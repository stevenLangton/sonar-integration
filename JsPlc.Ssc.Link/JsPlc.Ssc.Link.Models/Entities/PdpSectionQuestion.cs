using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class PdpSectionQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public PdpSection PdpSection { get; set; }

        [Required]
        [StringLength(2000)]
        public string QuestionText { get; set; }

        [Required]
        public int PresentationOrder { get; set; }
    }
}