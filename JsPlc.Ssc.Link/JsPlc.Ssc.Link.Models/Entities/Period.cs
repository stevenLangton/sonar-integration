using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsPlc.Ssc.Link.Models.Entities
{
    public class Period
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        //[DefaultValue(PeriodType.Quarter)]
        public PeriodType PeriodType { get; set; }
    }

    public enum PeriodType
    {
        Day = 1,
        Week = 2,
        Month = 3,
        // Valid definitions for now Quarter, Year
        Quarter = 4,
        Year = 5,
    }
}