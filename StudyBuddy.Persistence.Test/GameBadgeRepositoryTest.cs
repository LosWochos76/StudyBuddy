using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.Persistence.Test;

public class GameBadgeRepositoryTest : BaseTest
{
    [Fact]
    public void GetSuccessRateTest()
    {
        Create();

        // Arrange
        repository.Challenges.Insert(new Challenge() { ID = 1, Name = "Test Challenge1" });
        repository.Challenges.Insert(new Challenge() { ID = 2, Name = "Test Challenge2" });
        repository.Tags.Insert(new Tag() { ID = 1, Name = "tag1" });
        repository.Tags.Insert(new Tag() { ID = 2, Name = "tag2" });
        repository.Tags.AddTagForChallenge(1, 1);
        repository.Tags.AddTagForChallenge(2, 1);
        repository.Tags.AddTagForChallenge(1, 2);
        repository.Tags.AddTagForChallenge(2, 2);
        repository.GameBadges.Insert(new GameBadge() { ID = 1, Name = "Gamebadge1", RequiredCoverage=0.5 });
        repository.GameBadges.Insert(new GameBadge() { ID = 2, Name = "Gamebadge2", RequiredCoverage=1 });
        repository.Tags.AddTagForBadge(1, 1);
        repository.Tags.AddTagForBadge(2, 1);
        repository.Tags.AddTagForBadge(1, 2);
        repository.Tags.AddTagForBadge(2, 2);

        // Act1
        var success_rate = repository.GameBadges.GetSuccessRate(1, 1);
        Assert.NotNull(success_rate);
        Assert.Equal(1, success_rate.UserId);
        Assert.Equal(1, success_rate.BadgeId);
        Assert.Equal(2, success_rate.OverallChallengeCount);
        Assert.Equal(0, success_rate.AcceptedChallengeCount);

        // Act2
        repository.Challenges.AddAcceptance(1, 1);
        success_rate = repository.GameBadges.GetSuccessRate(1, 1);
        Assert.Equal(1, success_rate.UserId);
        Assert.Equal(1, success_rate.BadgeId);
        Assert.Equal(2, success_rate.OverallChallengeCount);
        Assert.Equal(1, success_rate.AcceptedChallengeCount);

        // Act3
        repository.Challenges.AddAcceptance(2, 1);
        success_rate = repository.GameBadges.GetSuccessRate(1, 1);
        Assert.Equal(1, success_rate.UserId);
        Assert.Equal(1, success_rate.BadgeId);
        Assert.Equal(2, success_rate.OverallChallengeCount);
        Assert.Equal(2, success_rate.AcceptedChallengeCount);

        Drop();
    }
}