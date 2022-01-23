using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StudyBuddy.App.Models
{
    public class UserStatistics
    {
        public int UserId { get; set; }
        public int TotalNetworkChallengesCount { get; set; }
        public int TotalOrganizingChallengesCount { get; set; }
        public int TotalLearningChallengesCount { get; set; }
        public int TotalAcceptedChallenges
        {
            get { return TotalNetworkChallengesCount + TotalOrganizingChallengesCount + TotalLearningChallengesCount; }
        }

        public int TotalNetworkChallengesPoints { get; set; }
        public int TotalOrganizingChallengesPoints { get; set; }
        public int TotalLearningChallengesPoints { get; set; }
        public int TotalPoints
        {
            get { return TotalNetworkChallengesPoints + TotalOrganizingChallengesPoints + TotalLearningChallengesPoints; }
        }

        public IEnumerable<RankEntry> FriendsRank { get; set; }

        public int LastWeekChallengeCount { get; set; }
        public int ThisWeekChallengeCount { get; set; }
        public int LastMonthChallengeCount { get; set; }
        public int ThisMonthChallengeCount { get; set; }


        public string ThisMonthTrendGlyph
        { 
            get { return GetGlyph(LastMonthChallengeCount, ThisMonthChallengeCount); }
        }

        public Color ThisMonthTrendColor
        {
            get { return GetColor(LastMonthChallengeCount, ThisMonthChallengeCount); }
        }

        public string ThisWeekTrendGlyph
        {
            get { return GetGlyph(LastWeekChallengeCount, ThisWeekChallengeCount); }
        }

        public Color ThisWeekTrendColor
        {
            get { return GetColor(LastWeekChallengeCount, ThisWeekChallengeCount); }
        }


        private string GetGlyph(int lastPeriodCount, int thisPeriodCount)
        {
            if (lastPeriodCount < thisPeriodCount)
            {
                return "\uf106";
            }
            else if (lastPeriodCount > thisPeriodCount)
            {
                return "\uf107";
            }
            else if (lastPeriodCount == thisPeriodCount)
            {
                return "\uf52c";
            }
            else
                return "\uf12a";
        }

        private Color GetColor(int lastPeriodCount, int thisPeriodCount)
        {
            if (lastPeriodCount < thisPeriodCount)
            {
                return Color.Green;
            }
            else if (lastPeriodCount > thisPeriodCount)
            {
                return Color.Orange;
            }
            else if (lastPeriodCount == thisPeriodCount)
            {
                return Color.Black;
            }
            else
                return Color.Red;
        }
    }

}
