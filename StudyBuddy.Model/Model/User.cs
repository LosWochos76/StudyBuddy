using System;
using System.Text.Json.Serialization;

namespace StudyBuddy.Model
{
    public class User : Entity
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Student;
        public bool EmailConfirmed { get; set; } = false;
        public bool AccountActive { get; set; } = false;
        public DateTime Created { get; set; }

        [JsonIgnore]
        public bool IsAdmin => Role == Role.Admin;

        [JsonIgnore]
        public bool IsStudent => Role == Role.Student;

        [JsonIgnore]
        public bool IsInstructor => Role == Role.Instructor;

        // Is only loaded in certain contexts
        public int CommonFriends { get; set; } = 0;
        public Request FriendshipRequest { get; set; } = null;
    }
}