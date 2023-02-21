using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.BusinessLogic.Parameters;
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
    public class RequestServiceTest
    {
        private IBackend backend;

        public RequestServiceTest()
        {
            var repository = new RepositoryMock();
            backend = new Backend(repository);
            repository.Users.Insert(new User() { ID = 1, Role = Role.Admin, AccountActive = true});
            repository.Users.Insert(new User() { ID = 2 });
            backend.CurrentUser = repository.Users.ById(1);
        }
        [Fact]
        public void InsertTest()
        {
            var request = new Request() { ID = 1, SenderID = 1, RecipientID = 2, Type = RequestType.Friendship };
            backend.RequestService.Insert(request);

            var result = backend.RequestService.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(RequestType.Friendship, result.Type);

        }
        [Fact]
        public void DeleteTest()
        {
            backend.RequestService.Insert(new Request() { ID = 1, SenderID = 1, RecipientID = 2, Type = RequestType.Friendship });
            backend.RequestService.Insert(new Request() { ID = 2, SenderID = 1, RecipientID = 2, Type = RequestType.Friendship });
            var bevor = backend.RequestService.All(new RequestFilter());

            backend.RequestService.Delete(1);
            var after = backend.RequestService.All(new RequestFilter());

            Assert.NotNull(after);
            Assert.NotNull(bevor);
            Assert.NotEqual(bevor.Count, after.Count);
        }
        [Fact]
        public void AcceptTest()
        {
            backend.RequestService.Insert(new Request() { ID = 1, SenderID = 2, RecipientID = backend.CurrentUser.ID, Type = RequestType.Friendship });

            backend.RequestService.Accept(1);

            Assert.Null(backend.RequestService.GetById(1));
        }

        [Fact]
        public void GetByIdTest()
        {
            backend.RequestService.Insert(new Request() { ID = 1, });
            backend.RequestService.Insert(new Request() { ID = 2, });
            
            var result1 = backend.RequestService.GetById(1);
            var result2 = backend.RequestService.GetById(2);

            Assert.NotNull(result1);    
            Assert.NotNull(result2);
            Assert.Equal(1, result1.ID);
            Assert.Equal(2, result2.ID);
        }
        [Fact]
        public void AllTest()
        {
            backend.RequestService.Insert(new Request() { ID = 1, SenderID = 2, RecipientID = backend.CurrentUser.ID, Type = RequestType.Friendship });
            backend.RequestService.Insert(new Request() { ID = 2, SenderID = 2, RecipientID = backend.CurrentUser.ID, Type = RequestType.Friendship});

            var result = backend.RequestService.All(new RequestFilter());

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
        }
    }
}