using StudyBuddy.App.ViewModels;
using Xunit;

namespace StudyBuddy.App.Test
{
    public class LoginViewModelTest : ViewModelBaseTest
    {
        private LoginViewModel vm;

        public LoginViewModelTest()
        {
            InitMocks();
            vm = new LoginViewModel(api, dialog, navigation); 
        }

        [Fact]
        public async void Login_Test()
        {
            var event_raised = false;
            api.Authentication.LoginStateChanged += (obj, eventArgs) => event_raised = true;

            vm.EMail = "alexander.stuckenholz@hshl.de";
            vm.Password = "secret";
            await vm.LoginCommand.ExecuteAsync();

            Assert.True(event_raised);
            Assert.NotNull(api.Authentication.CurrentUser);
            Assert.Equal("//ChallengesPage", navigation.GetLastPath());
        }
    }
}
