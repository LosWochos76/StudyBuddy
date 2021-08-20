using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.Model;
using StudyBuddy.ApiFacade;

[assembly: Xamarin.Forms.Dependency(typeof(LocalChallengeRepository))]
namespace StudyBuddy.ApiFacade
{
    public class LocalChallengeRepository : IChallengeRepository
    {
        public Task<IEnumerable<Challenge>> All()
        {
            return Task<IEnumerable<Challenge>>.Factory.StartNew(() =>
            {
                return new List<Challenge>();
            });
        }

        public Task<Challenge> ById(int id)
        {
            return Task<Challenge>.Factory.StartNew(() =>
            {
                return null;
            });
        }
    }
}
