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

        public BusinessEventServiceTest()
        {
            var repository = new RepositoryMock();
            repository.Users.Insert(new User() { ID = 1, Role = Role.Student });
            backend = new Backend(repository);
        }

        [Fact]
        public void DeleteTest()
        { 
        
        }
        [Fact]
        public void InsertTest()
        {

        }
        [Fact]
        public void UpdateTest()
        {

        }
        [Fact]
        public void GetByIDTest()
        {

        }
        [Fact]
        public void TriggerEventTest()
        {

        }
        [Fact]
        public void ExecuteTest()
        {

        }
    }
}
