using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.Model;

namespace StudyBuddy.ApiFacade
{
    public interface IChallengeService
    {
        Task<IEnumerable<Challenge>> ForToday(string tag_string);
        Task<bool> AcceptFromQrCode(string code);
    }
}