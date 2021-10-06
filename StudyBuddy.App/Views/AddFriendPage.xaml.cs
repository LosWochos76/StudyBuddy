using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class AddFriendPage : ContentPage
    {
        private AddFriendViewModel view_model;
        private Task task;

        public AddFriendPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<AddFriendViewModel>();
            BindingContext = view_model;
        }

        private void searchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            view_model.SearchText = e.NewTextValue;
            if (task == null || task.IsCompleted)
            {
                task = Task.Run(async () =>
                {
                    await Task.Delay(800);
                    view_model.ApplyFilter();
                });
            }
        }
    }
}