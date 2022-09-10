using System;
using System.Linq;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.Persistence.Test
{
    public class TagRepositoryTest : BaseTest
    {
        public TagRepositoryTest()
        {
        }

        [Fact]
        public void ForChallengeTest()
        {
            Create();

            repository.Challenges.Insert(new Challenge() { ID = 1 });
            repository.Challenges.Insert(new Challenge() { ID = 2 });
            repository.Tags.Insert(new Tag() { ID = 1 });
            repository.Tags.Insert(new Tag() { ID = 2 });
            repository.Tags.AddTagForChallenge(1, 1);
            repository.Tags.AddTagForChallenge(2, 2);

            // Act
            var objects1 = repository.Tags.ForChallenge(1);
            var objects2 = repository.Tags.ForChallenge(2);
            var objects3 = repository.Tags.ForChallenge(3);

            // Assert
            Assert.NotNull(objects1);
            Assert.NotNull(objects2);
            Assert.NotNull(objects3);
            var tags1 = objects1.ToList();
            var tags2 = objects2.ToList();
            var tags3 = objects3.ToList();
            Assert.NotEmpty(tags1);
            Assert.NotEmpty(tags2);
            Assert.Empty(tags3);
            Assert.Equal(1, tags1.Count);
            Assert.Equal(1, tags2.Count);
            Assert.Equal(1, tags1[0].ID);
            Assert.Equal(2, tags2[0].ID);

            Drop();
        }

        [Fact]
        public void ForBadgeTest()
        {
            Create();

            repository.GameBadges.Insert(new GameBadge() { ID = 1 });
            repository.GameBadges.Insert(new GameBadge() { ID = 2 });
            repository.Tags.Insert(new Tag() { ID = 1 });
            repository.Tags.Insert(new Tag() { ID = 2 });
            repository.Tags.AddTagForBadge(1, 1);
            repository.Tags.AddTagForBadge(2, 2);

            // Act
            var objects1 = repository.Tags.ForBadge(1);
            var objects2 = repository.Tags.ForBadge(2);
            var objects3 = repository.Tags.ForBadge(3);

            // Assert
            Assert.NotNull(objects1);
            Assert.NotNull(objects2);
            Assert.NotNull(objects3);
            var tags1 = objects1.ToList();
            var tags2 = objects2.ToList();
            var tags3 = objects3.ToList();
            Assert.NotEmpty(tags1);
            Assert.NotEmpty(tags2);
            Assert.Empty(tags3);
            Assert.Equal(1, tags1.Count);
            Assert.Equal(1, tags2.Count);
            Assert.Equal(1, tags1[0].ID);
            Assert.Equal(2, tags2[0].ID);

            Drop();
        }
    }
}

