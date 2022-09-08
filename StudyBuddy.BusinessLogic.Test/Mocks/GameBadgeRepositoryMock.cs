using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class GameBadgeRepositoryMock : IGameBadgeRepository
    {
        private TagRepositoryMock tags;
        ChallengeRepositoryMock challenges;
        private List<GameBadge> objects = new List<GameBadge>();
        private HashSet<Tuple<int, int>> user_to_gamebadge = new HashSet<Tuple<int, int>>();

        public GameBadgeRepositoryMock(TagRepositoryMock tags, ChallengeRepositoryMock challenges)
        {
            this.tags = tags;
            this.challenges = challenges;
        }

        public IEnumerable<GameBadge> All(GameBadgeFilter filter)
        {
            return objects;
        }

        public GameBadge ById(int id)
        {
            return objects.Where(obj => obj.ID == id).FirstOrDefault();
        }

        public void Delete(int id)
        {
            objects.RemoveAll(obj => obj.ID == id);
        }

        public IEnumerable<GameBadge> GetBadgesForChallenge(int challenge_id)
        {
            foreach (var t in tags.GetChallengeToBadge())
                if (t.ChallengeID == challenge_id)
                    yield return ById(t.BadgeID);
        }

        public int GetCount(GameBadgeFilter filter)
        {
            return objects.Count;
        }

        public void AddBadgeToUser(int user_id, int badge_id)
        {
            user_to_gamebadge.Add(new Tuple<int, int>(user_id, badge_id));
        }

        public void RemoveAllBadgesFromUser(int user_id)
        {
            user_to_gamebadge.RemoveWhere(obj => obj.Item1 == user_id);
        }

        public void RemoveBadgeFromUser(int user_id, int badge_id)
        {
            user_to_gamebadge.RemoveWhere(obj => obj.Item1 == user_id && obj.Item2 == badge_id);
        }

        public IEnumerable<GameBadge> GetReceivedBadgesOfUser(int user_id, GameBadgeFilter filter)
        {
            foreach (var tuple in user_to_gamebadge)
                if (tuple.Item1 == user_id)
                    yield return ById(tuple.Item2);
        }

        public BadgeSuccessRate GetSuccessRate(int badge_id, int user_id)
        {
            var badges_for_challenge = challenges.GetChallengesOfBadge(badge_id).ToList();
            var filter = new ChallengeFilter() { OnlyAccepted = true, CurrentUserId = user_id };
            var accepted_challenges = challenges.All(filter).ToList();

            return new BadgeSuccessRate()
            {
                BadgeId = badge_id,
                UserId = user_id,
                OverallChallengeCount = badges_for_challenge.Count,
                AcceptedChallengeCount = accepted_challenges.Count
            };
        }

        public void Insert(GameBadge obj)
        {
            if (obj.ID == 0)
                obj.ID = GetCount(null) + 1;

            objects.Add(obj);
        }

        public void Save(GameBadge obj)
        {
            if (obj.ID == 0)
                Insert(obj);
        }

        public void Update(GameBadge obj)
        {
            int pos = objects.FindIndex(o => o.ID == obj.ID);
            objects[pos] = obj;
        }
    }
}