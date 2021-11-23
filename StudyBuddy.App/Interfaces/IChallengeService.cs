using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IChallengeService
    {
        Task<IEnumerable<ChallengeViewModel>> ForToday(string search_string);
        Task<IEnumerable<ChallengeViewModel>> LoadMore(string search_string, int skip);
        Task<ChallengeViewModel> AcceptFromQrCode(string code);
        Task<bool> Accept(ChallengeViewModel cvm);
        Task<ChallengeViewModel> GetById(int challenge_id);
    }
}