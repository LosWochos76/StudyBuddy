using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Nickname { get; set; }
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public Role Role { get; set; }
        public int? ProgramID { get; set; }

        public bool IsAdmin { get { return Role == Role.Admin; } }
        public bool IsStudent { get { return Role == Role.Student; } }
        public bool IsInstructor { get { return Role == Role.Instructor; } }
    }
}