using SkiaSharp;
using StudyBuddy.BusinessLogic.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class GameBadgeServiceTest
    {
        private IBackend backend;
        public GameBadgeServiceTest()
        {
            var repository = new RepositoryMock();
            backend = new Backend(repository);
        }

        [Fact]
        public void InsertTest()
        {
            backend.Repository.Users.Insert(new User (){ID = 1, Role = Role.Admin });
            backend.CurrentUser = backend.Repository.Users.ById(1);

            var result = backend.GameBadgeService.Insert(new GameBadge() { Name = "testbadge"});

            Assert.NotNull(result);
            Assert.Equal("testbadge", result.Name);
        }

        [Fact]
        public void DeleteTest()
        {
            backend.Repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            backend.CurrentUser = backend.Repository.Users.ById(1);
            var badge = backend.GameBadgeService.Insert(new GameBadge() { Name = "testbadge" });
            var filter = new GameBadgeFilter();

            backend.GameBadgeService.Delete(1);
            var result = backend.GameBadgeService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public void UpdateTest()
        {
            backend.Repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            backend.CurrentUser = backend.Repository.Users.ById(1);
            var badge = backend.GameBadgeService.Insert(new GameBadge() { Name = "testbadge", OwnerID = backend.CurrentUser.ID });
            var tag = backend.TagService.Insert(new Tag() { Name = "testTag" });

            var result = backend.GameBadgeService.Update(badge);

            Assert.NotNull(result);

        }

        [Fact]
        public void GetByIdTest()
        {
            backend.Repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            backend.CurrentUser = backend.Repository.Users.ById(1);
            var badge1 = backend.GameBadgeService.Insert(new GameBadge() { Name = "testbadge1", OwnerID = backend.CurrentUser.ID });
            var badge2 = backend.GameBadgeService.Insert(new GameBadge() { Name = "testbadge2", OwnerID = backend.CurrentUser.ID });

            var result1 = backend.GameBadgeService.GetById(1);
            var result2 = backend.GameBadgeService.GetById(2);

            Assert.Equal("testbadge1", result1.Name);
            Assert.Equal("testbadge2", result2.Name);
        }

        [Fact]
        public void AllTest()
        {
            backend.Repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            backend.CurrentUser = backend.Repository.Users.ById(1);
            var badge1 = backend.GameBadgeService.Insert(new GameBadge() { Name = "testbadge1", OwnerID = backend.CurrentUser.ID });
            var badge2 = backend.GameBadgeService.Insert(new GameBadge() { Name = "testbadge2", OwnerID = backend.CurrentUser.ID });
            var filter = new GameBadgeFilter();

            var result = backend.GameBadgeService.All(filter);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void AddBadgeToUserTest()
        {
            backend.Repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            backend.CurrentUser = backend.Repository.Users.ById(1);
            var badge1 = backend.GameBadgeService.Insert(new GameBadge() { ID = 1, Name = "testbadge1", OwnerID = 1});
            var badge2 = backend.GameBadgeService.Insert(new GameBadge() { ID = 2, Name = "testbadge2", OwnerID = 1});

            backend.GameBadgeService.AddBadgeToUser(1, 1);
            backend.GameBadgeService.AddBadgeToUser(1, 2);

            Assert.Equal(1, badge1.OwnerID);
            Assert.Equal(1, badge2.OwnerID);
        }
    }
}
