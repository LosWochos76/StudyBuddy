namespace StudyBuddy.App.ViewModels
{
    using StudyBuddy.App.Api;
    using StudyBuddy.App.Misc;
    using StudyBuddy.Model.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class StatisticsViewModel : ViewModelBase
    {
        public ObservableCollection<ChallengeViewModel> AcceptedChallenges { get; private set; } = new ObservableCollection<ChallengeViewModel>();
        public int TotalPoints { get; set; }
        public int NetworkingPointsCount { get; set; }
        public int OrganizingPointsCount { get; set; }
        public int LearningPointsCount { get; set; }
        public int AcceptedChallengesCount { get; set; }
        public int OverallRank { get; set; }
        public bool IsRefreshing { get; set; }

        public ObservableCollection<Badge> Badges { get; set; } = new ObservableCollection<Badge>();

        public StatisticsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Initialize();
        }

        private async void Initialize()
        {
            //this.LoadAcceptedChallenges();
            await api.Challenges.GetAcceptedChallenges(AcceptedChallenges);
            this.Calculate();


            this.Badges = MockBadgeData();
        }

        private void Calculate()
        {
            foreach (var challenge in AcceptedChallenges)
            {
                switch (challenge.Category)
                {
                    case Model.ChallengeCategory.Learning:
                        this.LearningPointsCount += challenge.Points;
                        break;
                    case Model.ChallengeCategory.Networking:
                        this.NetworkingPointsCount += challenge.Points;
                        break;
                    case Model.ChallengeCategory.Organizing:
                        this.OrganizingPointsCount += challenge.Points;
                        break;
                    default:
                        throw new MissingMemberException("unknown ChallengeCategory");
                }
            }
            
            this.TotalPoints = this.NetworkingPointsCount + OrganizingPointsCount + LearningPointsCount;
            this.AcceptedChallengesCount = AcceptedChallenges.Count;
            this.OverallRank = 2;
        }

        private async void LoadAcceptedChallenges()
        {
            Console.WriteLine("-------------------------starting to Load Challenges");
            try
            {
                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    Console.WriteLine("---------- INVOKE MAIN THREAD ASYNC ENTERED");
                    await api.Challenges.GetAcceptedChallenges(AcceptedChallenges);

                });
                Console.WriteLine("-------------------------SUCCESS Loading Challendes"+ this.AcceptedChallenges.Count);

            }
            catch (ApiException e)
            {
                Console.WriteLine("-------------------------FAILED to Load Challenges");

                await dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");

        }

        private ObservableCollection<Badge> MockBadgeData()
        {

            var b1 = new Badge(1, "Ready to go", "Installiere StudyBuddy", true, 100);
            var b2 = new Badge(2, "Initializer", "Nimm deine erste Herausforderung an", true, 100);
            var b3 = new Badge(3, "Socializer", "Besuche die AStA Ersti Party", true, 100);
            var b4 = new Badge(4, "Socializer2", "Besuche die StuPa Sitzung", true, 100);
            var b5 = new Badge(5, "Super duper", "Schließe eine Klausur mit einer 1 ab", true, 100);
            var b6 = new Badge(6, "Run Run Run", "Besuche jede Vorlesung für eine ganze Woche", false, 84);
            var b7 = new Badge(7, "Bookworm", "Besuche die Bibliothek", false, 90);
            var b8 = new Badge(8, "Bookworm 2", "Leihe ein Buch in der Bibliothek aus", false, 21);
            var b9 = new Badge(9, "Know your Neighbour", "Setz dich zu einer Person, die du nicht kennst", false, 55);
            var b10 = new Badge(10, "Party Animal", "Geh auf 5 Parties", false, 5);
            var b11 = new Badge(11, "High Performer", "Schließe 10 Herausforderungen ab", false, 15);

            var list = new ObservableCollection<Badge>();
            list.Add(b1);
            list.Add(b2);
            list.Add(b3);
            list.Add(b4);
            list.Add(b5);
            list.Add(b6);
            list.Add(b7);
            list.Add(b8);
            list.Add(b9);
            list.Add(b10);
            list.Add(b11);

            return list;
        }
    }
}