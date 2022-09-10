using System;
using StudyBuddy.Model;
using Xunit;

namespace StudyBuddy.Persistence.Test
{
    public class UserRepositoryTest : BaseTest
    {
        [Fact]
        public void CountOfCommonFriendsTest()
        {
            Create();

            // Arrange
            repository.Users.Insert(new User() { ID = 1 });
            repository.Users.Insert(new User() { ID = 2 });
            repository.Users.Insert(new User() { ID = 3 });
            repository.Users.Insert(new User() { ID = 4 });
            repository.Users.Insert(new User() { ID = 5 });
            repository.Users.AddFriend(1, 2);
            repository.Users.AddFriend(1, 3);
            repository.Users.AddFriend(1, 4);
            repository.Users.AddFriend(5, 2);
            repository.Users.AddFriend(5, 3);
            repository.Users.AddFriend(5, 4);

            // Assert
            Assert.Equal(3, repository.Users.GetCountOfCommonFriends(1, 5));
            Assert.Equal(0, repository.Users.GetCountOfCommonFriends(1, 2));
            Assert.Equal(0, repository.Users.GetCountOfCommonFriends(1, 3));
            Assert.Equal(0, repository.Users.GetCountOfCommonFriends(3, 1));
            Assert.Equal(2, repository.Users.GetCountOfCommonFriends(3, 2));

            Drop();
        }
    }
}

