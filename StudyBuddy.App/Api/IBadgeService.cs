using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IBadgeService
    {
        Task<IEnumerable<BadgeViewModel>> GetBadges(bool reload = false);
    }
}
