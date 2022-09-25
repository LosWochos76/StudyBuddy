using System;
using StudyBuddy.Model.Enum;

namespace StudyBuddy.Model
{
    public class LoginResult 
    {
        public User User { get; set; }
        public LoginStatus Status { get; set; }
        public string Token { get; set; }

        public LoginResult()
        {
            
        }


        public LoginResult(string token, User user)
        {
            Token = token;
            User = user;
        }
    }
}
