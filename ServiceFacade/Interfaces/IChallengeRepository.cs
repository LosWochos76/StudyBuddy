using System.Threading.Tasks;
using StudyBuddy.Model;

namespace StudyBuddy.ServiceFacade
{
    public interface IChallengeRepository
    {
        Task<Challenge> ById(int id);
    }
}