using StudyBuddy.BusinessLogic.Misc;
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
    public class PasswordGeneratorTest
    {
        [Fact]
        public void GetUniqueKeyTest()
        {
            var key1 = PasswordGenerator.GetUniqueKey(8);
            var key2 = PasswordGenerator.GetUniqueKey(8);

            Assert.NotEmpty(key1);
            Assert.NotEmpty(key2);  
            Assert.NotEqual(key1, key2);
        }
    }
}
