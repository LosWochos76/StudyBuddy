using System.Linq;
using Xamarin.Forms;

namespace StudyBuddy.App.Misc
{
    public class NagigationService : INavigationService
    {
        public async void GoTo(string path)
        {
            Shell.Current.FlyoutIsPresented = false;

            if (Shell.Current.Navigation.NavigationStack.Count > 0)
            {
                var list = Shell.Current.Navigation.NavigationStack.ToList();
                foreach (var page in list)
                    if (page != null)
                        Shell.Current.Navigation.RemovePage(page);
            }

            await Shell.Current.GoToAsync(path);
            
        }

        public async void Push(Page page)
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.Navigation.PushAsync(page);
            
        }

        public async void Pop()
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.Navigation.PopAsync();
        }
    }
}