using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NetworkingChallengeTemplate : Grid
    {
        public NetworkingChallengeTemplate()
        {
            InitializeComponent();
            BindingContext = TinyIoCContainer.Current.Resolve<ChallengesViewModel>();
        }
    }
}