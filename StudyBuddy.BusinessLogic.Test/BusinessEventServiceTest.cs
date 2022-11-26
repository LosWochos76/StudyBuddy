using StudyBuddy.BusinessLogic.Test.Mocks;
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
            backend = new Backend(repository);
        }

        [Fact]
        public void DeleteTest()
        { 
        
        }
    }
}
