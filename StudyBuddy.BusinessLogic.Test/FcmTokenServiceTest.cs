using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using StudyBuddy.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class FcmTokenServiceTest
    {
        private IBackend backend;

        [Fact]
        public void GetAllTest()
        {
            var repository = new RepositoryMock();
            backend = new Backend(repository);
            repository.Users.Insert(new User { ID = 1, Email = "admin@admin.de", Role = Role.Admin, AccountActive = true, EmailConfirmed = true });
            backend.CurrentUser = repository.Users.ById(1);
            //var fcmTokens = backend.Repository.FcmTokens.GetForUser(user.ID).Select(token => token.Token);

        }
    }
}