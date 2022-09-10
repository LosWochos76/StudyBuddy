using System.Linq;
using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
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
            repository.Notifications.Insert(new Notification() { Id = 1, OwnerId = 1 });
            repository.Notifications.Insert(new Notification() { Id = 2, OwnerId = 2 });
            backend = new Backend(repository);
            backend.CurrentUser = repository.Users.ById(1);
        }

        [Fact]
        public void GetNotificationsForFriendsTest_With_Owner()
        {
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
        public void GetNotificationsForFriendsTest_With_Comment()
        {
            // Act
            backend.CommentService.Insert(new Comment() { Id = 1, NotificationId = 1 });
            var objects = backend.NotificationService.GetNotificationsForFriends(filter);

            // Assert
            Assert.NotNull(objects);
            var notifications = objects.ToList();
            Assert.NotNull(notifications[0].Comments);
        }

        [Fact]
        public void GetNotificationsForFriendsTest_With_Liked_User()
        {
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