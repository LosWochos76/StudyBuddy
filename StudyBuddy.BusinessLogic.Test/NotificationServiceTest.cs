using System.Linq;
using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using StudyBuddy.Persistence;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class NotificationServiceTest
    {
        private IBackend backend;
        private NotificationFilter filter = new NotificationFilter()
        {
            UserID = 1,
            WithOwner = true,
            WithLikedUsers = true
        };

        public NotificationServiceTest()
        {
            // Arrange
            var repository = new RepositoryMock();
            repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            repository.Users.Insert(new User() { ID = 2, Role = Role.Student });
            repository.Users.AddFriend(1, 2);

            backend = new Backend(repository);
            backend.CurrentUser = repository.Users.ById(1);
        }

        [Fact]
        public void ByIdTest()
        {
            backend.Repository.Notifications.Insert(new Notification() { ID = 1, OwnerId = 1 });
            backend.Repository.Notifications.Insert(new Notification() { ID = 2, OwnerId = 2 });

            var result1 = backend.NotificationService.ById(1);
            var result2 = backend.NotificationService.ById(2);

            Assert.NotEqual(result1, result2);
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(1, result1.ID);
            Assert.Equal(2, result2.ID);
        }

        [Fact]
        public void UserAcceptedChallengeTest()
        {
            backend.Repository.Challenges.Insert(new Challenge() { ID = 1 });
            backend.NotificationService.UserAcceptedChallenge(backend.CurrentUser, backend.ChallengeService.GetById(1));

            var result = backend.NotificationService.GetAll(filter);

            Assert.Equal(1, result.Count);
            Assert.NotNull(result);
        }

        [Fact]
        public void UserReceivedBadgeTest()
        {
            backend.Repository.GameBadges.Insert(new GameBadge() {ID = 1 });
            backend.NotificationService.UserReceivedBadge(backend.CurrentUser, backend.GameBadgeService.GetById(1));

            var result = backend.NotificationService.GetAll(filter);

            Assert.Equal(1, result.Count);
            Assert.NotNull(result);
        }
        [Fact]
        public void UserMadeFriendTest()
        {
            backend.NotificationService.UserMadeFriend(backend.CurrentUser, backend.UserService.GetById(2));
            var result = backend.NotificationService.ById(1);

            Assert.Equal("Freundschaft geschlossen", result.Title);
        }

        [Fact]
        public void DeleteTest()
        {
            backend.Repository.Notifications.Insert(new Notification() { ID = 1 });
            backend.Repository.Notifications.Insert(new Notification() { ID = 2 });
            backend.Repository.Notifications.Insert(new Notification() { ID = 3 });
            var bevor = backend.NotificationService.GetAll(filter);

            backend.NotificationService.Delete(1);
            var danach = backend.NotificationService.GetAll(filter);

            Assert.Equal(3, bevor.Count);
            Assert.Equal(2, danach.Count);
        }

        [Fact]
        public void GetNotificationsForFriendsTest_With_Owner()
        {
            backend.Repository.Notifications.Insert(new Notification() { ID = 1, OwnerId = 1 });
            backend.Repository.Notifications.Insert(new Notification() { ID = 2, OwnerId = 2 });

            // Act
            var objects = backend.NotificationService.GetNotificationsForFriends(filter);

            // Assert
            Assert.NotNull(objects);
            var notifications = objects.ToList();
            Assert.Equal(1, notifications.Count);
            Assert.NotNull(notifications[0].Owner);
            Assert.Equal(2, notifications[0].Owner.ID);
            Assert.Empty(notifications[0].LikedUsers);
        }

        [Fact]
        public void GetNotificationsForFriendsTest_With_Liked_User()
        {
            backend.Repository.Notifications.Insert(new Notification() { ID = 1, OwnerId = 1 });
            backend.Repository.Notifications.Insert(new Notification() { ID = 2, OwnerId = 2 });

            // Act
            var nmd = new NotificationUserMetadata() { OwnerId = 1, NotificationId = 2, Liked = true };
            backend.NotificationUserMetadataService.Upsert(nmd);
            var objects = backend.NotificationService.GetNotificationsForFriends(filter);

            // Assert
            Assert.NotNull(objects);
            var notifications = objects.ToList();
            Assert.NotNull(notifications[0].LikedUsers);
            var users = notifications[0].LikedUsers.ToList();
            Assert.Equal(1, users[0].ID);
        }
    }
}