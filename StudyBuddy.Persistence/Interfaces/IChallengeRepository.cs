using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IChallengeRepository
    {
        Challenge ById(int id);
        IEnumerable<Challenge> All(int from = 0, int max = 1000);
        IEnumerable<Challenge> ForToday(DateTime today, int from = 0, int max = 1000);
        IEnumerable<Challenge> ByText(string text, int from = 0, int max = 1000);
        IEnumerable<Challenge> OfOwner(int owner_id, int from = 0, int max = 1000);
        IEnumerable<Challenge> OfOwnerByText(int owner_id, string text, int from = 0, int max = 1000);
        void Save(Challenge obj);
        void Insert(Challenge obj);
        void Update(Challenge obj);
        void Delete(int id);

        // Link to badges:
        IEnumerable<Challenge> OfBadge(int badge_id);

        // Link to acceptance:
        void AddAcceptance(int challenge_id, int user_id);
        void RemoveAcceptance(int challenge_id, int user_id);
        IEnumerable<Challenge> Accepted(int user_id);
    }
}