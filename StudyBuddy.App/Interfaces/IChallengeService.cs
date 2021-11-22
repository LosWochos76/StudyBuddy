using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public interface IChallengeService<T>
    {
        Task<IEnumerable<T>> ForToday(string search_string);
        Task<IEnumerable<T>> LoadMore(string search_string, int skip);
        Task<ChallengeViewModel> AcceptFromQrCode(string code);
        Task<bool> Accept(ChallengeViewModel cvm);
        Task<ChallengeViewModel> GetById(int challenge_id);
    }
}