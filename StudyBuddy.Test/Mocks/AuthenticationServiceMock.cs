﻿using System.Threading.Tasks;
using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.Test.Mocks
{
    public class AuthenticationServiceMock : IAuthenticationService
    {
        public event LoginStateChangedHandler LoginStateChanged;
        public string Token { get; private set; } = string.Empty;
        public UserViewModel CurrentUser { get; private set; }
        public bool IsLoggedIn => Token != string.Empty;

        public Task<bool> Login(UserCredentials credentials)
        {
            return Task.Run(() =>
            {
                CurrentUser = new UserViewModel() { ID = 1, Firstname = "Test", Lastname = "Test" };
                Token = "secret_token";
                return true;
            });
        }

        public Task<bool> LoginFromJson(string content)
        {
            return Login(null);
        }

        public void Logout()
        {
            CurrentUser = null;
            Token = string.Empty;
        }
    }
}