using System.Threading.Tasks;

namespace StudyBuddy.App.Misc
{
    public interface INavigationService
    {
        Task GoTo(string path);
    }
}