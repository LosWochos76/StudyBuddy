using System;
using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class UserServiceTest
    {
        private IBackend backend;

        public UserServiceTest()
        {
            // Arrange
            var repository = new RepositoryMock();
            repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            repository.Users.Insert(new User() { ID = 2, Role = Role.Student });
            backend = new Backend(repository);
            backend.CurrentUser = repository.Users.ById(1);
        }

        [Fact]
        public void RegisterNewUserTest()
        {
            // Arrange
            var user = new User() { Role = Role.Admin, Email="test@test.de", Password="test", AccountActive=true };
            backend.CurrentUser = null;

            // Act
            user = backend.UserService.Insert(user);

            // Assert
            Assert.Equal(Role.Student, user.Role);
            Assert.False(string.IsNullOrEmpty(user.PasswordHash));
        }
    }
}

