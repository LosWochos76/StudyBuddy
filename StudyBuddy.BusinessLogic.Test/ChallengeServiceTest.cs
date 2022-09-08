using System.Linq;
using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test;

public class ChallengeServiceTest
{
    [Fact]
    public void InsertTest()
    {
        // Arrange
        var repository = new RepositoryMock();
        repository.Users.Insert(new User() { ID = 1, Email = "admin@admin.de", Role = Role.Admin, AccountActive = true, EmailConfirmed = true });
        var backend = new Backend(repository);
        backend.CurrentUser = repository.Users.ById(1);

        // Act
        backend.ChallengeService.Insert(new Challenge() { ID = 1, Name = "Test Challenge", Tags = "#tag1 #tag2" });

        // Assert
        var challenge = backend.Repository.Challenges.ById(1);
        Assert.NotNull(challenge);
        Assert.Equal("Test Challenge", challenge.Name);
        var tags = repository.Tags.ForChallenge(1).ToList();
        Assert.NotNull(tags);
        Assert.Equal(2, tags.Count);
    }

    [Fact]
    public void GetByIdTest()
    {
        // Arrange
        var repository = new RepositoryMock();
        repository.Users.Insert(new User() { ID = 1, Email = "admin@admin.de", Role = Role.Admin, AccountActive = true, EmailConfirmed = true });
        repository.Challenges.Insert(new Challenge() { ID = 1, Name = "Test Challenge" });
        repository.Tags.Insert(new Tag() { ID = 1, Name = "tag1" });
        repository.Tags.Insert(new Tag() { ID = 2, Name = "tag2" });
        repository.Tags.AddTagForChallenge(1, 1);
        repository.Tags.AddTagForChallenge(2, 1);
        var backend = new Backend(repository);
        backend.CurrentUser = repository.Users.ById(1);

        // Act
        var challenge = backend.ChallengeService.GetById(1);

        // Assert
        Assert.NotNull(challenge);
        Assert.Equal("Test Challenge", challenge.Name);
        Assert.Equal("#tag1 #tag2", challenge.Tags); // This is important!
    }

    [Fact]
    public void AddAcceptanceTest()
    {
        // Arrange
        var repository = new RepositoryMock();
        repository.Users.Insert(new User() { ID = 1, Email = "admin@admin.de", Role = Role.Admin, AccountActive = true, EmailConfirmed = true });
        repository.Challenges.Insert(new Challenge() { ID = 1, Name = "Test Challenge" });
        repository.Tags.Insert(new Tag() { ID = 1, Name = "tag1" });
        repository.Tags.Insert(new Tag() { ID = 2, Name = "tag2" });
        repository.Tags.AddTagForChallenge(1, 1);
        repository.Tags.AddTagForChallenge(2, 1);
        repository.GameBadges.Insert(new GameBadge() { ID = 1, Name = "Gamebadge", RequiredCoverage=1 });
        repository.Tags.AddTagForBadge(1, 1);
        repository.Tags.AddTagForBadge(2, 1);
        var backend = new Backend(repository);
        backend.CurrentUser = repository.Users.ById(1);

        // Act
        backend.ChallengeService.AddAcceptance(1, 1);

        // Assert
        var badges = repository.GameBadges.GetReceivedBadgesOfUser(1, null).ToList();
        Assert.NotNull(badges);
        Assert.Equal(1, badges.Count);
    }
}