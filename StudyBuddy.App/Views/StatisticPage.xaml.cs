using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class StatisticPage : ContentPage
    {
        private readonly StatisticsViewModel view_model;

        public StatisticPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<StatisticsViewModel>();
            BindingContext = view_model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            System.Console.WriteLine("-----APPEARED-----");
            view_model.Refresh();
            
            //your code here;
        }
    }
}