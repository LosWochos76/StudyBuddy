
using StudyBuddy.App.ViewModels;
using Xunit;

namespace StudyBuddy.Test
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
        public void Login_Test()
        {
            vm.EMail = "alexander.stuckenholz@hshl.de";
            vm.Password = "secret";
            vm.LoginCommand.Execute(null);

            Assert.NotNull(api.Authentication.CurrentUser);
        }
    }
}
