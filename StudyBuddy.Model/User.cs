namespace StudyBuddy.Model
{
    public class User : Entity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public int? ProgramID { get; set; }
        public int? EnrolledInTermID { get; set; }

        public bool IsAdmin => Role == Role.Admin;
        public bool IsStudent => Role == Role.Student;
        public bool IsInstructor => Role == Role.Instructor;
    }
}