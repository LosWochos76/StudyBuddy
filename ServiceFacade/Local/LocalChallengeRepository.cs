using System;
using System.Threading.Tasks;
using StudyBuddy.Model;
using StudyBuddy.ServiceFacade;

[assembly: Xamarin.Forms.Dependency(typeof(LocalChallengeRepository))]
namespace StudyBuddy.ServiceFacade
{
    public class LocalChallengeRepository : IChallengeRepository
    {
        public Task<Challenge> ById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
