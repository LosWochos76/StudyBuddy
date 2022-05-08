using System.Threading.Tasks;
using Xamarin.Forms;

namespace StudyBuddy.App.Misc
{
    public interface INavigationService
    {
        Task GoTo(string path);
        Task Push(Page page);
        Task Pop();
    }
}