using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public interface IChallengeService
    {
        Task<ChallengeListViewModel> ForToday(string search_string = "", int skip = 0);
        Task<ChallengeViewModel> GetById(int challenge_id);
        Task<bool> Accept(ChallengeViewModel cvm);
        Task<ChallengeListViewModel> Accepted(string search_string = "", int skip = 0);

        Task<ChallengeViewModel> AcceptFromQrCode(string code);
        Task<bool> AcceptWithAddendum(ChallengeViewModel cvm, string prove_addendum);
        Task<AcceptChallengeByLocationResultDTO> AcceptWithLocation(ChallengeViewModel cvm, GeoCoordinate location);
    }
}