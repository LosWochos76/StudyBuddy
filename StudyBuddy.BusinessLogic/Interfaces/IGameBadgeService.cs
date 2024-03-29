﻿using System;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IGameBadgeService
    {
        GameBadgeList All(GameBadgeFilter filter);
        GameBadge GetById(int id);
        GameBadge Insert(GameBadge obj);
        GameBadge Update(GameBadge obj);
        void Delete(int id);

        GameBadgeList GetBadgesForChallenge(int challenge_id);
        public BadgeSuccessRate GetSuccessRate(int badge_id, int user_id);

        void AddBadgeToUser(int user_id, int badge_id);
        void RemoveBadgeFromUser(int user_id, int badge_id);
        GameBadgeList GetReceivedBadgesOfUser(int user_id, GameBadgeFilter filter);

        bool CheckIfUserEarnedGameBadgeAfterChallengeCompleted(User user, Challenge challenge);

        // Events
        event BadgeReceivedEventHandler BadgeReceived;
    }

    public delegate void BadgeReceivedEventHandler(object sender, BadgeReceivedEventArgs e);

    public class BadgeReceivedEventArgs : EventArgs
    {
        public GameBadge GameBadge { get; set; }
        public User User { get; set; }

        public BadgeReceivedEventArgs(GameBadge gb, User u)
        {
            GameBadge = gb;
            User = u;
        }
    }
}