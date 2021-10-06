using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> OfChallenge(int challenge_id);
        Task<string> OfChallengeAsString(int challenge_id);
    }
}
