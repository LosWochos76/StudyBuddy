namespace StudyBuddy.App.ViewModels
{
    using StudyBuddy.App.Api;
    using StudyBuddy.App.Misc;
    using StudyBuddy.Model.Model;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class StatisticsViewModel : ViewModelBase
    {
        public ObservableCollection<ChallengeStatistics> ChallengeStatistic { get; set; } = new ObservableCollection<ChallengeStatistics>();
        public string TotalPoints { get; set; }
        public string NetworkingPointsCount { get; set; }
        public string OrganizingPointsCount { get; set; }
        public string LearningPointsCount { get; set; }
        public ObservableCollection<Badge> Badges { get; set; } = new ObservableCollection<Badge>();

        public StatisticsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.NetworkingPointsCount = "10";
            this.OrganizingPointsCount = "432";
            this.LearningPointsCount = "110";
            this.TotalPoints = "552";
            this.Badges = MockBadgeData();
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