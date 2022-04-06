using StudyBuddy.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentModalPage : ContentPage
    {
        public CommentModalPage(NewsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = new CommentModalPageViewModel(viewModel);
        }
    }
}