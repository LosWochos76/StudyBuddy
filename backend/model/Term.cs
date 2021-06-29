using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{
    public class Term
    {
        public int ID { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Start { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime End { get; set; }

        public Term()
        {
            Start = DateTime.Now.Date;
            End = DateTime.Now.Date;
        }
    }
}