using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IChallengeService
    {
        Task<IEnumerable<ChallengeViewModel>> ForToday(string tag_string, bool reload = false);
        Task<bool> AcceptFromQrCode(string code);
    }
}