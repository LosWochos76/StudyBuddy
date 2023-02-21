using System;
using StudyBuddy.BusinessLogic.Parameters;
using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using StudyBuddy.Model.Enum;
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
        public void GetAllTest()
        {
            var filter = new UserFilter();  

            var result = backend.UserService.GetAll(filter);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public void GetByIdTest()
        { 
            var result1 = backend.UserService.GetById(1);
            var result2 = backend.UserService.GetById(2);

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(Role.Admin, result1.Role);
            Assert.Equal(Role.Student, result2.Role);
        }

        [Fact]
        public void ResetPasswordTest() 
        {
            var user = new User() { ID = 5, Email = "test@mail", Password = "password", AccountActive = true };
            backend.UserService.Insert(user);
            var token = backend.AuthenticationService.GenerateUserToken("test@mail");
            var data = new ResetPasswordData() { Token = token, Email = user.Email, Password = user.Password };
       
            var result = backend.UserService.ResetPassword(data);

            Assert.NotNull(result);
            Assert.Equal(LoginStatus.Success, result.Status);
        }
        [Fact]
        public void GeneratePasswordTest()
        {
            var user = new User() { ID = 5, Email = "test@mail", Password = "password", AccountActive = true };
            backend.UserService.Insert(user);
            var token = backend.AuthenticationService.GenerateUserToken("test@mail");
            var data = new ResetPasswordData() { Token = token, Email = user.Email, Password = user.Password };

            var result = backend.UserService.GeneratePassword(data);

            Assert.NotNull(result);
            Assert.NotEqual("password", user.Password);
        }
        [Fact]
        public void VerifyEmailTest()
        {
            var user = new User() { ID = 5, Email = "test@mail", Password = "password", AccountActive = true };
            backend.UserService.Insert(user);
            var token = backend.AuthenticationService.GenerateUserToken("test@mail");
            var data = new VerifyEmailData() { Token = token, Email = user.Email};

            var result = backend.UserService.VerifyEmail(data);

            Assert.NotNull(result);
            Assert.Equal(LoginStatus.Success, result.Status);
            Assert.True(user.EmailConfirmed);
        }
        [Fact]
        public void EnableAccountTest()
        {
            var user = backend.UserService.Insert(new User() { ID = 1 });

            var result = backend.UserService.EnableAccount(user);

            Assert.NotNull(result);
            Assert.True(user.AccountActive);
        }
        [Fact]
        public void DeleteTest()
        {
            var user1 = backend.UserService.Insert(new User() { ID = 1 });
            var user2 = backend.UserService.Insert(new User() { ID = 2 });
            var bevor = backend.UserService.GetAll(new UserFilter());

            backend.UserService.Delete(2);
            var after = backend.UserService.GetAll(new UserFilter());

            Assert.NotEqual(bevor.Count, after.Count);
        }
        [Fact]
        public void AddFriendtest()
        {
            backend.UserService.AddFriend(1, 2);

            var result = backend.UserService.GetAllFriends(new FriendFilter() { UserId = 1 });

            Assert.Equal(1, result.Count);
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

