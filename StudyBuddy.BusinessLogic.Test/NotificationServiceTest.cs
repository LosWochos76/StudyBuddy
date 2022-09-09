using System;
using System.Linq;
using StudyBuddy.BusinessLogic.Test.Mocks;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test
{
    public class NotificationServiceTest
    {
        [Fact]
        public void GetNotificationsForFriendsTest()
        {
            // Arrange
            var repository = new RepositoryMock();
            repository.Users.Insert(new User() { ID = 1, Role = Role.Admin });
            repository.Users.Insert(new User() { ID = 2, Role = Role.Student });
            repository.Users.AddFriend(1, 2);
            repository.Notifications.Insert(new Notification() { Id = 1, OwnerId = 1 });
            repository.Notifications.Insert(new Notification() { Id = 2, OwnerId = 2 });
            var backend = new Backend(repository);
            backend.CurrentUser = repository.Users.ById(1);

            // Act
            var filter = new NotificationFilter()
            {
                UserID = 1,
                WithOwner = true,
                WithLikedUsers=true
            };
            var objects = backend.NotificationService.GetNotificationsForFriends(filter);

            // Assert
            Assert.NotNull(objects);
            var notifications = objects.ToList();
            Assert.Equal(1, notifications.Count);
            Assert.NotNull(notifications[0].Owner);
            Assert.Empty(notifications[0].LikedUsers);

            // Act2
            backend.CommentService.Insert(new Comment() { Id = 1, NotificationId = 1 });
            objects = backend.NotificationService.GetNotificationsForFriends(filter);

            // Assert2
            Assert.NotNull(objects);
            notifications = objects.ToList();
            Assert.NotNull(notifications[0].Comments);

            // Act3
            var nmd = new NotificationUserMetadata() { OwnerId = 1, NotificationId = 2, Liked = true };
            backend.NotificationUserMetadataService.Upsert(nmd);
            objects = backend.NotificationService.GetNotificationsForFriends(filter);

            // Assert3
            Assert.NotNull(objects);
            notifications = objects.ToList();
            Assert.NotNull(notifications[0].LikedUsers);
            var users = notifications[0].LikedUsers.ToList();
            Assert.Equal(1, users[0].ID);
        }
    }
}