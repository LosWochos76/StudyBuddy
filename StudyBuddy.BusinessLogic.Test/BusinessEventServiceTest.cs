using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class BusinessEventServiceTest
    {
        private IBackend backend;
        User user = new User{ID = 1, Role = Role.Admin};
        public BusinessEventServiceTest()
        {
            var repository = new RepositoryMock();
            repository.Users.Insert(user);
            backend = new Backend(repository);
            backend.CurrentUser = user;
        }

        [Fact]
        public void DeleteTest()
        { 
            var testevent = new BusinessEvent();
            testevent.Name= "Test";
            testevent.ID= 1;
            backend.BusinessEventService.Insert(testevent);
            Assert.NotNull(backend.BusinessEventService.GetById(1));

            backend.BusinessEventService.Delete(1);

            Assert.Null(backend.BusinessEventService.GetById(1));

        }
        [Fact]
        public void InsertTest()
        {
            var testevent = new BusinessEvent();
            testevent.Name = "Test";
            testevent.ID = 1;
            testevent.Type = (BusinessEventType)1;

            backend.BusinessEventService.Insert(testevent);

            var result = backend.BusinessEventService.GetById(1);

            Assert.Equal(result.Name, testevent.Name);
        }
        [Fact]
        public void GetByIDTest()
        {
            var testevent1 = new BusinessEvent();
            testevent1.ID= 1;
            testevent1.Name= "Test1";
            var testevent2 = new BusinessEvent();
            testevent2.ID = 2;
            testevent2.Name = "Test2";
            backend.BusinessEventService.Insert(testevent1);
            backend.BusinessEventService.Insert(testevent2);

            var result1 = backend.BusinessEventService.GetById(1);
            var result2 = backend.BusinessEventService.GetById(2);

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(testevent1.Name, result1.Name);
            Assert.Equal(testevent2.Name, result2.Name);

        }
        [Fact]
        public void AllTest()
        {
            var testevent1 = new BusinessEvent();
            testevent1.ID = 1;
            testevent1.Name = "Test1";
            var testevent2 = new BusinessEvent();
            testevent2.ID = 2;
            testevent2.Name = "Test2";
            backend.BusinessEventService.Insert(testevent1);
            backend.BusinessEventService.Insert(testevent2);

            var filter = new BusinessEventFilter { SearchText = string.Empty };
            var result = backend.BusinessEventService.All(filter);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
