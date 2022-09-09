using StudyBuddy.App.Test.Mocks;
using StudyBuddy.App.ViewModels;
using Xunit;

namespace StudyBuddy.App.Test
{
    public class LoginViewModelTest
    {
        [Fact]
        public async void Login_Test()
        {
            // Arrange
            var api = new ApiMock();
            var vm = new LoginViewModel(api);
            var event_raised = false;
            api.Authentication.LoginStateChanged += (obj, eventArgs) => event_raised = true;
            vm.EMail = "alexander.stuckenholz@hshl.de";
            vm.Password = "secret";

            // Act
            await vm.LoginCommand.ExecuteAsync();

            // Assert
            Assert.True(event_raised);
            Assert.NotNull(api.Authentication.CurrentUser);
        }
    }
}