using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.Persistence.Test
{
    public class BusinessEventRepositoryTest: BaseTest
    {
        [Fact]
        public void InsertTest()
        {
            Create();
            repository.Users.Insert(new User() { ID = 1 });
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 1, Name = "TestEvent", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });
            
            //Eigentlich redundant an der Stelle
            

            Drop();
        }
        [Fact]
        public void DeleteTest()
        {
            Create();
            repository.Users.Insert(new User() { ID = 1 });
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 1, Name = "TestEvent", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });

            repository.BusinessEvents.Delete(1);
            var result = repository.BusinessEvents.All(new BusinessEventFilter());
            
            Assert.Empty(result);

            Drop();
        }
        [Fact]
        public void GetByIdTest()
        {
            Create();
            repository.Users.Insert(new User() { ID = 1});
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 1, Name = "TestEvent", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });

            var result = repository.BusinessEvents.GetById(1);

            Assert.NotNull(result);
            Assert.Equal("TestEvent", result.Name);

            Drop();
        }
        [Fact]
        public void AllTest()
        {
            Create();
            repository.Users.Insert(new User() { ID = 1 });
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 1, Name = "TestEvent", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 2, Name = "TestTest", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 3, Name = "Test3", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });

            var result = repository.BusinessEvents.All(new BusinessEventFilter());

            Assert.NotNull(result);
            Assert.Equal("TestEvent", result.Where(obj => obj.ID == 1).FirstOrDefault().Name);
            Drop();
        }
        [Fact]
        public void GetCountTest()
        { 
            Create();
            repository.Users.Insert(new User() { ID = 1 });
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 1, Name = "TestEvent", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 2, Name = "TestTest", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 3, Name = "Test3", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });

            var result = repository.BusinessEvents.GetCount(new BusinessEventFilter());

            Assert.NotNull(result);
            Assert.Equal(3 , result);
            Drop();
        }
        [Fact]
        public void UpdateTest()
        {
            Create();
            repository.Users.Insert(new User() { ID = 1 });
            repository.BusinessEvents.Insert(new BusinessEvent() { ID = 1, Name = "Hello", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });

            var result = repository.BusinessEvents.Update(new BusinessEvent() { ID = 1, Name = "TestEvent", OwnerID = 1, Type = BusinessEventType.UserRegistered, Code = "test" });

            Assert.NotNull(result);
            Assert.Equal("TestEvent", result.Name);
            Drop();
        }
    }
}
