using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class StatisticPage : ContentPage
    {
        public StatisticPage()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<StatisticsViewModel>(); ;
        }
    }
}