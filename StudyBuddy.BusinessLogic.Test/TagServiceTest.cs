using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class TagServiceTest
    {
        private IBackend backend;

        public TagServiceTest()
        {
            var repository = new RepositoryMock();
            backend = new Backend(repository);
            backend.Repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            backend.CurrentUser = backend.Repository.Users.ById(1);
        }
        [Fact]
        public void InsertTest()
        {
            backend.TagService.Insert(new Tag() {  ID = 1});

            var result = backend.TagService.GetById(1);

            Assert.NotNull(result); 
            Assert.Equal(1, result.ID);
        }
        [Fact]
        public void DeleteTest()
        {
            backend.TagService.Insert(new Tag() { ID = 1 });
            backend.TagService.Insert(new Tag() { ID = 2 });
            var bevor = backend.TagService.GetAll(new TagFilter());

            backend.TagService.Delete(1);
            var after = backend.TagService.GetAll(new TagFilter());

            Assert.NotNull(after);
            Assert.NotNull(bevor);
            Assert.NotEqual(bevor.Count, after.Count);
        }
        [Fact]
        public void CreateOrFindMultipleTest()
        {
            String test = "tag1,#tag2;tag3";

            var result = backend.TagService.CreateOrFindMultiple(test);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

    }
}
