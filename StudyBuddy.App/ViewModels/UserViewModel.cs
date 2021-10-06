using StudyBuddy.App.Misc;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class UserViewModel
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public int CountOfCommonFriends { get; set; }

        public bool IsAdmin => Role == Role.Admin;
        public bool IsStudent => Role == Role.Student;
        public bool IsInstructor => Role == Role.Instructor;

        public string FullName
        {
            get
            {
                return string.Format("{0} {1} ({2})", Firstname, Lastname, Nickname);
            }
        }

        public static UserViewModel FromModel(User u)
        {
            return new UserViewModel()
            {
                ID = u.ID,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Nickname = u.Nickname,
                Email = u.Email,
                Role = u.Role
            };
        }

        public bool ContainsAny(string search_text)
        {
            var keywords = Helper.SplitIntoKeywords(search_text);
            return Helper.ContainsAny(Firstname + Lastname + Nickname + Email, keywords);
        }
    }
}
