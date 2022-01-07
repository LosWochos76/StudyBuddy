using System.Threading.Tasks;
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
        public async void Initial_Load()
        {
            Assert.Empty(vm.Challenges);
            await api.Authentication.Login(null);

            Assert.NotEmpty(vm.Challenges);
        }

        [Fact]
        public async void Load_More()
        {
            await api.Authentication.Login(null);
            Assert.NotEmpty(vm.Challenges);

            await vm.LoadMoreCommand.ExecuteAsync();
            Assert.Equal(20, vm.Skip);

            for (int i=0; i<20; i++)
                await vm.LoadMoreCommand.ExecuteAsync();

            Assert.Equal(-1, vm.ItemThreshold);
        }

        [Fact]
        public async void Perform_Search()
        {
            await api.Authentication.Login(null);
            Assert.NotEmpty(vm.Challenges);

            vm.SearchText = "22";
            await Task.Delay(2000);

            Assert.Single(vm.Challenges);
        }
    }
}