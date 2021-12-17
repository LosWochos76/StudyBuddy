using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.Test.Mocks
{
    public class ChallengeServiceMock : IChallengeService
    {
        private List<ChallengeViewModel> challenges = new List<ChallengeViewModel>();

        public ChallengeServiceMock()
        {
            challenges.Add(new ChallengeViewModel()
            {
                ID = 1,
                Category = ChallengeCategory.Learning,
                Name = "Test-Challenge1",
                Points = 5,
                OwnerID = 1,
                Prove = ChallengeProve.ByTrust
            });
        }

        public Task<bool> Accept(ChallengeViewModel cvm)
        {
            return Task.Run(() =>
            {
                return true;
            });
        }

        public Task<ChallengeViewModel> AcceptFromQrCode(string code)
        {
            return Task.Run(() =>
            {
                return challenges[0];
            });
        }

        public Task<bool> AcceptWithAddendum(ChallengeViewModel cvm, string prove_addendum)
        {
            return Task.Run(() =>
            {
                return true;
            });
        }

        public void AddChallenges(IEnumerable<RequestViewModel> requests)
        {
        }

        public Task<IEnumerable<ChallengeViewModel>> ForToday(string search_string = "", int skip = 0)
        {
            return Task.Run(() =>
            {
                return (IEnumerable<ChallengeViewModel>)challenges;
            });
        }

        public Task<IEnumerable<ChallengeViewModel>> GetAcceptedChallenges()
        {
            throw new NotImplementedException();
        }

        public Task<ChallengeViewModel> GetById(int challenge_id)
        {
            return Task.Run(() =>
            {
                return challenges[0];
            });
        }
    }
}
