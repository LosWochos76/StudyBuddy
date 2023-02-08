using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class MailKitHelperTest
    {
        [Fact]
        public void SendMailTest()
        {
            var empfaenger = "fakemail@todo";
            var betreff = "test_betreff";
            var nachricht = "moin";

            var ausgabe = MailKitHelper.SendMail(empfaenger, betreff, nachricht);

            Assert.False(ausgabe);
        }
    }
}
