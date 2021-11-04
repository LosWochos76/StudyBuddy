using System.Collections.Generic;
using System.Drawing;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IChallengeService
    {
        void AcceptFromQrCode(string payload);
        IEnumerable<Challenge> All(ChallengeFilter filter);
        void CreateSeries(CreateSeriesParameter param);
        void Delete(int id);
        Challenge GetById(int id);
        Bitmap GetQrCode(int challenge_id);
        Challenge Insert(Challenge obj);
        IEnumerable<Challenge> GetChallengesOfBadge(int badge_id);
        void RemoveAcceptance(int challenge_id, int user_id);
        Challenge Update(Challenge obj);
    }
}