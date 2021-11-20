using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StudyBuddy.App.Misc
{
    public class NagigationService : INavigationService
    {
        public Task GoTo(string path)
        {
            Shell.Current.FlyoutIsPresented = false;

            if (Shell.Current.Navigation.NavigationStack.Count > 0)
            {
                var list = Shell.Current.Navigation.NavigationStack.ToList();
                foreach (var page in list)
                    if (page != null)
                        Shell.Current.Navigation.RemovePage(page);
            }

            return Shell.Current.GoToAsync(path);
            
        }

        public Task Push(Page page)
        {
            Shell.Current.FlyoutIsPresented = false;
            return Shell.Current.Navigation.PushAsync(page);
            
        }

        public Task Pop()
        {
            Shell.Current.FlyoutIsPresented = false;
            return Shell.Current.Navigation.PopAsync();
        }
    }
}