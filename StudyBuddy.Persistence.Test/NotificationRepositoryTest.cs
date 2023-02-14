using System;
using StudyBuddy.Model;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace StudyBuddy.Persistence.Test
{
    public class NotificationRepositoryTest : BaseTest
    {
        [Fact]
        public void GetUserNotificationsFeedTest()
        {
            Create();

            // Arrange
            repository.Users.Insert(new User() { ID = 2, Role = Role.Student });
            repository.Users.Insert(new User() { ID = 3, Role = Role.Student });
            repository.Users.AddFriend(1, 2);
            repository.Notifications.Insert(new Notification() { ID = 1, OwnerId = 1 });
            repository.Notifications.Insert(new Notification() { ID = 2, OwnerId = 1 });
            repository.Notifications.Insert(new Notification() { ID = 1, OwnerId = 2 });
            repository.Notifications.Insert(new Notification() { ID = 2, OwnerId = 2 });
            repository.Notifications.Insert(new Notification() { ID = 1, OwnerId = 3 });
            repository.Notifications.Insert(new Notification() { ID = 2, OwnerId = 3 });

            // Act
            var filter = new NotificationFilter() { UserID = 1 };
            var objects = repository.Notifications.GetNotificationsForFriends(filter).ToList();

            // Assert
            Assert.NotEmpty(objects);
            Assert.Equal(2, objects.Count);
            Assert.Equal(2, objects[0].OwnerId);
            Assert.Equal(2, objects[1].OwnerId);

            Drop();
        }
    }
}