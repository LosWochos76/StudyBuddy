using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IChallengeService
    {
        Task<IEnumerable<ChallengeViewModel>> ForToday(string search_string = "", int skip = 0);
        Task<ChallengeViewModel> AcceptFromQrCode(string code);
        Task<bool> Accept(ChallengeViewModel cvm);
        Task GetAcceptedChallenges(ObservableCollection<ChallengeViewModel> list);
        Task GetAcceptedChallengesForUser(ObservableCollection<ChallengeViewModel> list, int user_id);
        Task<ChallengeViewModel> GetById(int challenge_id);
    }
}