using StudyBuddy.BusinessLogic.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class Base64Test
    {
        [Fact]
        public void EncodeTest()
        {
            var teststring = "L";
            var checkstring = Base64.Encode(teststring);

            Assert.Equal("TA==", checkstring);
        }
        [Fact]
        public void DecodeTest() 
        {
            var teststring = "TA==";
            var checkstring = Base64.Decode(teststring);

            Assert.Equal("L", checkstring);
        }

    }

}
