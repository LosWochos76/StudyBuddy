using System.Threading.Tasks;
using Xamarin.Forms;

namespace StudyBuddy.App.Misc
{
    public class NagigationService : INagigationService
    {
        public Task GoTo(string path)
        {
            return Shell.Current.GoToAsync(path);
        }
    }
}
