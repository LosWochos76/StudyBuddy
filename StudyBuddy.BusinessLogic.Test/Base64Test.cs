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
            //Arrange
            var teststring = "L";

            //Act
            var checkstring = Base64.Encode(teststring);

            //Assert
            Assert.Equal("TA==", checkstring);
        }
        [Fact]
        public void DecodeTest() 
        {
            //Arrange
            var teststring = "TA==";

            //Act
            var checkstring = Base64.Decode(teststring);

            //Assert
            Assert.Equal("L", checkstring);
        }

    }

}
