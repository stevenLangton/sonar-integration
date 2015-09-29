using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace JsPlc.Ssc.Link.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public int PeriodId { get; set; }
        [ForeignKey("PeriodId")]
        public virtual Period Period { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}