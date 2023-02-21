using Microsoft.IdentityModel.Tokens;
using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class JwtTokenTest
    {
        private IBackend backend;
    
        public JwtTokenTest() 
        {
            var repository = new RepositoryMock();
            backend = new Backend(repository);
            
        }
        [Fact]
        public void PasswordResetTokenTest()
        {
            var jwt = new JwtToken();
            var user = new User() { ID = 1, Email = "test@test.de", Password = "test" };
            user = backend.UserService.Insert(user);
            var RS = jwt.PasswordResetToken(user.ID, user.PasswordHash);

            Assert.True(jwt.CheckPasswordResetToken(RS, user.PasswordHash));

            var user2 = new User() { ID = 2, Email = "bla@blub.de", Password = "level" };
            user2 = backend.UserService.Insert(user2);
            var RT = jwt.PasswordResetToken(user2.ID, user2.PasswordHash);

            Assert.True(jwt.CheckPasswordResetToken(RT, user2.PasswordHash));

            Assert.NotEqual(RS, RT);
        }
        [Fact]
        public void FromUserTest()
        {
            var jwt = new JwtToken();
            var user = new User() { ID = 2, Email = "test@test.de", Password = "test" };
            user = backend.UserService.Insert(user);

            var UT = jwt.FromUser(user.ID);
            
            Assert.Equal(2, jwt.FromToken(UT));
        }
    }
}
