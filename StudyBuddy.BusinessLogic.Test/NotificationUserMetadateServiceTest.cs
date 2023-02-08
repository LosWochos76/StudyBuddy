using Org.BouncyCastle.Security;
using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    
    public class NotificationUserMetadateServiceTest
    {
        private IBackend backend;

        public NotificationUserMetadateServiceTest()
        {
            var repository = new RepositoryMock();
            repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            backend = new Backend(repository);
            backend.CurrentUser = repository.Users.ById(1);
        }
        [Fact]
        public void getAllTest()
        {
            backend.Repository.NotificationUserMetadata.Insert(new NotificationUserMetadata() { Id = 1});
            backend.Repository.NotificationUserMetadata.Insert(new NotificationUserMetadata() { Id = 2});
            var filter = new NotificationUserMetadataFilter();

            var result = backend.NotificationUserMetadataService.GetAll(filter);
            
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public void DeleteTest()
        {
            backend.Repository.NotificationUserMetadata.Insert(new NotificationUserMetadata() { Id = 1 });
            backend.Repository.NotificationUserMetadata.Insert(new NotificationUserMetadata() { Id = 2 });
            var filter = new NotificationUserMetadataFilter();
            var bevor = backend.NotificationUserMetadataService.GetAll(filter);

            backend.NotificationUserMetadataService.Delete(2);
            var after = backend.NotificationUserMetadataService.GetAll(filter);

            Assert.Equal(2, bevor.Count);
            Assert.Equal(1, after.Count);
        }
    }
}
