using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string QuestionType { get; set; } // 'b' or 'f' for backward or forward

       [ScriptIgnore]
        public ICollection<Answer> Answers { get; set; }
    }
} 