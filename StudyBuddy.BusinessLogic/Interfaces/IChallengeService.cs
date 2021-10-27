using System.Collections.Generic;
using System.Drawing;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IChallengeService
    {
        void AcceptFromQrCode(string payload);
        IEnumerable<Challenge> All();
        void CreateSeries(CreateSeriesParameter param);
        void Delete(int id);
        IEnumerable<Challenge> ForToday();
        Challenge GetById(int id);
        IEnumerable<Challenge> GetByText(string text);
        Bitmap GetQrCode(int challenge_id);
        Challenge Insert(Challenge obj);
        IEnumerable<Challenge> OfBadge(int id);
        void RemoveAcceptance(int challenge_id, int user_id);
        Challenge Update(Challenge obj);
    }
}