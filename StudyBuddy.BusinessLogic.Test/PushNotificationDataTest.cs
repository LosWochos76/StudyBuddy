using StudyBuddy.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace StudyBuddy.BusinessLogic.Test
{
    public class PushNotificationDataTest
    {
        [Fact]
        public void todataTest_emptyPath()
        {
            var PT = new PushNotificationData();
            PT.PagePath = string.Empty;

            var result = PT.toData();

            Assert.Empty(result["PagePath"]);
        }
        [Fact]
        public void todataTest_defaultType()
        {
            var PT = new PushNotificationData();
            PT.PagePath = string.Empty;

            var result = PT.toData();

            Assert.Empty(result["PagePath"]);
            Assert.Equal("Normal", result["PushNotificationType"]);
        }
        [Fact]
        public void todataTest()
        {
            var PT = new PushNotificationData();
            PT.PushNotificationType = PushNotificationTypes.FriendShipRequest;
            PT.PagePath = "TestPath";

            var result = PT.toData();

            Assert.Equal("TestPath",result["PagePath"]);
            Assert.Equal("FriendShipRequest", result["PushNotificationType"]);
        }
    }
}
