
namespace StudyBuddy.Model.Model
{
    public class LoginResult 
    {
        public User User { get; set; }
        public int Status { get; set; }
        public string Token { get; set; }
    }
}
