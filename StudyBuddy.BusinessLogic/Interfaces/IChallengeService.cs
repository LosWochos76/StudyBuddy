using System.Collections.Generic;
using System.Drawing;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IChallengeService
    {
        Challenge GetById(int id);
        IEnumerable<Challenge> All(ChallengeFilter filter);
        Challenge Insert(Challenge obj);
        Challenge Update(Challenge obj);
        void Delete(int id);
        
        Bitmap GetQrCode(int challenge_id);
        void CreateSeries(CreateSeriesParameter param);
        IEnumerable<Challenge> GetChallengesOfBadge(int badge_id);

        void RemoveAcceptance(int challenge_id, int user_id);
        void AddAcceptance(int challenge_id, int user_id);
        void Accept(int challenge_id);
        Challenge AcceptFromQrCode(string payload);
        bool AcceptWithAddendum(int challenge_id, string prove_addendum);
    }
}