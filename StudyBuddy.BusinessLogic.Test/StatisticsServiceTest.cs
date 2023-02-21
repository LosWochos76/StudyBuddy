using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Persistence;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class StatisticsServiceTest
    {
        private IBackend backend;
        public StatisticsServiceTest()
        { 
            var repository = new RepositoryMock();
            backend = new Backend(repository);
        }
        [Fact]
        public void GetUserStatisticsTest()
        {
            backend.Repository.Users.Insert(new User() {ID = 1, Email = "testmail", Password = "password", Role = Role.Admin, AccountActive = true, EmailConfirmed = true });
            backend.CurrentUser = backend.Repository.Users.ById(1);

            var result = backend.StatisticsService.GetUserStatistics(1);

            Assert.Null(result);
        }
    }
}
