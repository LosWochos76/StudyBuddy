using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{
    public class StudyProgram
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(3)]
        public string Acronym { get; set; }

        [Required]
        public string Name { get; set; }
    }
}