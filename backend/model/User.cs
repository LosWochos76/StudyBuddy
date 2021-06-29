using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public int? ProgramID { get; set; }
        public int? EnrolledInTermID { get; set; }
        
        public bool IsAdmin { get { return Role == Role.Admin; } }
        public bool IsStudent { get { return Role == Role.Student; } }
        public bool IsInstructor { get { return Role == Role.Instructor; } }
    }
}