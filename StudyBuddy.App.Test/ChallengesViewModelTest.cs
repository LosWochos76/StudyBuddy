using System.Threading.Tasks;
using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;
using Xunit;

namespace StudyBuddy.App.Test
{
    public class ChallengesViewModelTest : ViewModelBaseTest
    {
        private ChallengesViewModel vm;

        public ChallengesViewModelTest()
        {
            InitMocks();
            vm = new ChallengesViewModel(api, dialog, navigation);
        }

        [Fact]
        public async void Load()
        {
            // Arange
            var credentials = new UserCredentials() { EMail = "admin@admin.de", Password = "secret" };
            await api.Authentication.Login(credentials);

            // Act1
            Assert.Empty(vm.Challenges);
            await vm.LoadMoreCommand.ExecuteAsync();

            // Assert
            Assert.NotEmpty(vm.Challenges);
            Assert.Equal(10, vm.Challenges.Count);

            // Act2
            await vm.LoadMoreCommand.ExecuteAsync();

            // Assert
            Assert.NotEmpty(vm.Challenges);
            Assert.Equal(20, vm.Challenges.Count);
        }
    }
}