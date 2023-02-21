using System;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.App.Test
{
    public class UserViewModelTest
    {
        [Fact]
        public void ToModelTest()
        {
            // Prepare
            var vm = new UserViewModel()
            {
                ID = 1,
                Firstname = "Alex",
                Lastname = "Stuckenholz",
                Nickname = "stucki",
                Password = "test",
                Role = Role.Student
            };

            // Act
            var m = UserViewModel.ToModel(vm);

            // Assert
            Assert.Equal(vm.ID, m.ID);
            Assert.Equal(vm.Firstname, m.Firstname);
            Assert.Equal(vm.Lastname, m.Lastname);
            Assert.Equal(vm.Password, m.Password);
            Assert.Equal(vm.Role, m.Role);
        }
        [Fact]
        public void FromModelTest()
        {
            var user = new User() { ID  = 1, Firstname = "Test", Lastname = "User", Role = Role.Student};

            var result = UserViewModel.FromModel(user);

            Assert.NotNull(result);
            Assert.Equal(user.ID, result.ID);
            Assert.Equal(user.Firstname, result.Firstname);
            Assert.Equal(user.Lastname, result.Lastname);
            Assert.Equal(user.Role, result.Role);
        }
    }
}