using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{    
    public class UserCredentials
    {
        [Required]
        [EmailAddress]
        public string EMail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}