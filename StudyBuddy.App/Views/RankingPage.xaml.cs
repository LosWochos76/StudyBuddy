using StudyBuddy.App.ViewModels;
using StudyBuddy.Model.Model;
using System.Collections.Generic;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class RankingPage : ContentPage
    {
        private readonly RankingViewModel view_model;
        public RankingPage(IEnumerable<RankEntry> ranks)
        {
            InitializeComponent();

            view_model = new RankingViewModel(ranks);
            BindingContext = view_model;
        }
    }
}