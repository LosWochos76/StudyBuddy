using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.Test.Mocks
{
    public class NavigationServiceMock : INavigationService
    {
        private string last_path = string.Empty;
        private Page last_page = null;

        public async Task GoTo(string path)
        {
            last_path = path;
        }

        public async Task Pop()
        {
        }

        public async Task Push(Page page)
        {
            last_page = page;
        }

        public string GetLastPath()
        {
            return last_path;
        }

        public Page GetLastPage()
        {
            return last_page;
        }
    }
}