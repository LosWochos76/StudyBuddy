using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IChallengeService
    {
        Task ForToday(ObservableCollection<ChallengeViewModel> list, string search_string);
        Task<ChallengeViewModel> AcceptFromQrCode(string code);
        Task<bool> Accept(ChallengeViewModel cvm);
        Task GetAcceptedChallenges(ObservableCollection<ChallengeViewModel> list);
        Task GetAcceptedChallengesForUser(ObservableCollection<ChallengeViewModel> list, int user_id);
        Task<ChallengeViewModel> GetById(int challenge_id);
    }
}