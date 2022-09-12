using System;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.Persistence.Test
{
    public class NotificationUserMetadataRepositoryTest : BaseTest
    {
        [Fact]
        public void FindByNotificationAndOwnerTest()
        {
            Create();

            // Arrange
            repository.Users.Insert(new User() { ID = 2 });
            repository.Notifications.Insert(new Notification() { Id = 1, OwnerId = 1 });
            repository.Notifications.Insert(new Notification() { Id = 1, OwnerId = 2 });
            repository.NotificationUserMetadata.Insert(new NotificationUserMetadata() { Id = 1, OwnerId = 1, NotificationId = 1 });
            repository.NotificationUserMetadata.Insert(new NotificationUserMetadata() { Id = 1, OwnerId = 2, NotificationId = 2 });

            // Act
            var obj = repository.NotificationUserMetadata.FindByNotificationAndOwner(1, 1);

            // Asert
            Assert.NotNull(obj);
            Assert.Equal(1, obj.Id);
            Assert.Equal(false, obj.Liked);
            Assert.Equal(false, obj.Shared);
            Assert.Equal(false, obj.Seen);

            Drop();
        }

        [Fact]
        public void UpdateTest()
        {
            Create();

            // Arrange
            repository.Notifications.Insert(new Notification() { Id = 1, OwnerId = 1 });
            repository.NotificationUserMetadata.Insert(new NotificationUserMetadata() { Id = 1, OwnerId = 1, NotificationId = 1 });
            var obj = repository.NotificationUserMetadata.FindByNotificationAndOwner(1, 1);

            // Act
            obj.Shared = true;
            repository.NotificationUserMetadata.Update(obj);

            // Asert
            Assert.NotNull(obj);
            Assert.Equal(1, obj.Id);
            Assert.Equal(false, obj.Liked);
            Assert.Equal(true, obj.Shared);
            Assert.Equal(false, obj.Seen);

            Drop();
        }
    }
}

