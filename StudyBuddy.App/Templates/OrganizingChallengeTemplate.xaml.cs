using StudyBuddy.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganizingChallengeTemplate : Grid
    {
        public ChallengeViewModel ChallengeViewModel { get; set; }
        public ChallengesViewModel ChallengesViewModel { get; set; }
        public OrganizingChallengeTemplate()
        {
            ChallengeViewModel = DependencyService.Get<ChallengeViewModel>();
            ChallengesViewModel = DependencyService.Get<ChallengesViewModel>();
            BindingContext = ChallengeViewModel;
            InitializeComponent();
            
        }
    }
}