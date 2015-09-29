using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models
{
    public class Period
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Period")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Period Start")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Period End")]
        public DateTime End { get; set; }
    }
}