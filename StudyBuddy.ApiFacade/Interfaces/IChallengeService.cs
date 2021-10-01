using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.Model;

namespace StudyBuddy.ApiFacade
{
    public interface IChallengeService
    {
        Task<IEnumerable<Challenge>> ForToday(string tag_string);
        void AcceptFromQrCode(string code);
    }
}