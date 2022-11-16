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
    }
}