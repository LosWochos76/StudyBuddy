using StudyBuddy.App.ViewModels;
using Xunit;

namespace StudyBuddy.Test
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
        public void Test1()
        {
            Assert.Empty(vm.Challenges);
            api.Authentication.Login(null);
        }
    }
}