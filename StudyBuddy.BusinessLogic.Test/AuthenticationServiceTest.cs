using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class AuthenticationServiceTest
    {
        private IBackend backend;

        public AuthenticationServiceTest()
        {
            var repository = new RepositoryMock();
            backend = new Backend(repository);
        }

        [Fact]
        public void GenerateUserTokenTest()
        {
            var user1 = new User() { Email = "test@test.de", Password = "test", AccountActive = true };
            var user2 = new User() { Email = "hallo@hallo.de", Password = "hallo", AccountActive = true };

            user1 = backend.UserService.Insert(user1);
            user2 = backend.UserService.Insert(user2);

            string token1 = backend.AuthenticationService.GenerateUserToken(user1.Email);
            string token2 = backend.AuthenticationService.GenerateUserToken(user2.Email);

            Assert.NotNull(token1);
            Assert.NotEmpty(token1);
            Assert.NotNull(token2);
            Assert.NotEmpty(token2);
            Assert.NotEqual(token1, token2);
        }

        [Fact]
        public void CheckForValidMailTest()
        {
            var user1 = new User() { Email = "test@test.de", Password = "test", AccountActive = true };
            var user2 = new User() { Email = "hallo@hallo.de", Password = "hallo", AccountActive = true };

            user1 = backend.UserService.Insert(user1);
            user2 = backend.UserService.Insert(user2);

            Assert.NotEmpty(user1.Email);
            Assert.NotEmpty(user2.Email);
            Assert.False(backend.AuthenticationService.CheckForValidMail("unit@test.de"));
            Assert.True(backend.AuthenticationService.CheckForValidMail("test@test.de"));
        }

        [Fact]
        public void CheckTokenTest()
        {
            var user1 = new User() { ID = 1, Email = "test@test.de", Password = "test", AccountActive = true };

            user1 = backend.UserService.Insert(user1);

            string token1 = backend.AuthenticationService.GenerateUserToken(user1.Email);

            Assert.False(backend.AuthenticationService.CheckToken("TestTokenString"));
            Assert.True(!backend.AuthenticationService.CheckToken(token1));
        }

        [Fact]
        public void CheckPasswordResetTokenTest()
        {
            var user1 = new User() { ID = 1, Email = "test@test.de", Password = "test", AccountActive = true };

            user1 = backend.UserService.Insert(user1);

            string token1 = backend.AuthenticationService.GenerateUserToken(user1.Email);

            Assert.True(backend.AuthenticationService.CheckPasswordResetToken(token1, user1.PasswordHash));
            Assert.False(backend.AuthenticationService.CheckPasswordResetToken("randomString", user1.PasswordHash));
        }

        [Fact]
        public void LoginTest()
        {
            var user1 = new User() { ID = 1, Email = "test@test.de", Password = "test", AccountActive = false};
            user1 = backend.UserService.Insert(user1);

            UserCredentials uc1 = new UserCredentials();
            uc1.EMail = user1.Email;
            uc1.Password = user1.Password;

            var LR = backend.AuthenticationService.Login(uc1);
            //7 = AccountDisabled
            Assert.Equal(7, (int)LR.Status);

            user1.AccountActive = true;
            LR = backend.AuthenticationService.Login(uc1);
            //1 = EmailNotVerified
            Assert.Equal(1, (int)LR.Status);

            user1.EmailConfirmed= true;
            LR = backend.AuthenticationService.Login(uc1);
            //0 = Success
            Assert.Equal(0, (int)LR.Status);

            UserCredentials uc2 = new UserCredentials();
            uc2.EMail = "tada@bla.de";
            uc2.Password= "Tee";
            LR = backend.AuthenticationService.Login(uc2);
            //3 = UserNotFound
            Assert.Equal(3, (int)LR.Status);
        }
    }
}