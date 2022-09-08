using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class ChallengeRepositoryMock : IChallengeRepository
    {
        private TagRepositoryMock tags;
        private List<Challenge> objects = new List<Challenge>();
        private HashSet<Tuple<int, int>> acceptances = new HashSet<Tuple<int, int>>();

        public ChallengeRepositoryMock(TagRepositoryMock tags)
        {
            this.tags = tags;
        }

        public void AddAcceptance(int challenge_id, int user_id)
        {
            acceptances.Add(new Tuple<int, int>(challenge_id, user_id));
        }

        private bool HasAccepted(int challenge_id, int user_id)
        {
            return acceptances.Contains(new Tuple<int, int>(challenge_id, user_id));
        }

        public IEnumerable<Challenge> All(ChallengeFilter filter)
        {
            if (filter == null)
                filter = new ChallengeFilter();

            foreach (var obj in objects)
            {
                if (filter.OnlyAccepted && !HasAccepted(obj.ID, filter.CurrentUserId))
                    continue;

                if (filter.OnlyUnacceped && HasAccepted(obj.ID, filter.CurrentUserId))
                    continue;

                // Eine Krücke das hier zu machen, aber besser als alle anderen Alternativen
                obj.Tags = TagList.ToString(tags.ForChallenge(obj.ID));
                yield return obj;
            }
        }

        public Challenge ById(int id)
        {
            var obj = objects.Where(obj => obj.ID == id).FirstOrDefault();

            // Eine Krücke das hier zu machen, aber besser als alle anderen Alternativen
            obj.Tags = TagList.ToString(tags.ForChallenge(id));
            return obj;
        }

        public void Delete(int id)
        {
            objects.RemoveAll(obj => obj.ID == id);
        }

        public IEnumerable<Challenge> GetChallengesOfBadge(int badge_id)
        {
            var tuples = tags.GetChallengeToBadge().ToList();
            var is_in = new HashSet<int>();

            foreach (var t in tuples)
                if (t.BadgeID == badge_id)
                    if (is_in.Add(t.ChallengeID))
                        yield return ById(t.ChallengeID);
        }

        public int GetCount(ChallengeFilter filter)
        {
            return objects.Count;
        }

        public void Insert(Challenge obj)
        {
            if (obj.ID == 0)
                obj.ID = GetCount(null) + 1;

            objects.Add(obj);
        }

        public void RemoveAcceptance(int challenge_id, int user_id)
        {
            acceptances.RemoveWhere(obj => obj.Item1 == challenge_id && obj.Item2 == user_id);
        }

        public void RemoveAllAcceptances(int user_id)
        {
            acceptances.RemoveWhere(obj => obj.Item2 == user_id);
        }

        public void Save(Challenge obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Update(Challenge obj)
        {
            int pos = objects.FindIndex(o => o.ID == obj.ID);
            objects[pos] = obj;
        }
    }
}