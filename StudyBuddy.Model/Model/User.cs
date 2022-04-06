using System.Text.Json.Serialization;

namespace StudyBuddy.Model
{
    public class User : Entity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public string Email { get; set; }
        public Role Role { get; set; }

        [JsonIgnore]
        public bool IsAdmin => Role == Role.Admin;

        [JsonIgnore]
        public bool IsStudent => Role == Role.Student;

        [JsonIgnore]
        public bool IsInstructor => Role == Role.Instructor;

        // Is only loaded in certain contexts
        public int CommonFriends { get; set; }
        public Request FriendshipRequest { get; set; }
    }
}