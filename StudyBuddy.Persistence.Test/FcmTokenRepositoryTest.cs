using System;
using System.Linq;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.Persistence.Test
{
    public class FcmTokenRepositoryTest : BaseTest
    {
        [Fact]
        public void GetForUserTest()
        {
            Create();

            // Aragnge
            repository.FcmTokens.Save(new FcmToken() { ID = 1, UserID = 1, Token = "test1", Created = DateTime.Now.Date.AddDays(-10) });
            repository.FcmTokens.Save(new FcmToken() { ID = 2, UserID = 1, Token = "test2", Created = DateTime.Now.Date });

            // Act
            var objects = repository.FcmTokens.GetForUser(1);

            // Assert
            Assert.NotNull(objects);
            var tokens = objects.ToList();
            Assert.Equal(2, tokens.Count);
            Assert.Equal(2, tokens[0].ID);

            Drop();
        }
    }
}