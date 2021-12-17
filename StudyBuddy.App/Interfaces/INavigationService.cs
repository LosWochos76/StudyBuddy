using Xamarin.Forms;

namespace StudyBuddy.App.Misc
{
    public interface INavigationService
    {
        void GoTo(string path);
        void Push(Page page);
        void Pop();
    }
}