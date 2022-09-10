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
    }
}