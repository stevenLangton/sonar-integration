using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Newtonsoft.Json;

namespace JsPlc.Ssc.Link.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

  //      [JsonIgnore]
        public ICollection<Answer> Answers { get; set; }
    }
} 