using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IUserRepository
    {
        // Basic CRUD-methods
        User ById(int id);
        IEnumerable<User> All(UserFilter filter);
        int Count();
        User ByEmail(string email);
        User ByNickname(string nickname);
        void Save(User obj);
        void Insert(User obj);
        void Update(User obj);
        void Delete(int id);

        // Friendship
        IEnumerable<User> GetFriends(FriendFilter filter);
        int GetFriendsCount(int user_id);
        IEnumerable<User> GetNotFriends(FriendFilter filter);
        void AddFriend(int user_id, int friend_id);
        void RemoveFriend(int user_id, int friend_id);
        void RemoveFriends(int user_id);
        void AddFriends(int user_id, int[] friend_ids);
        int GetCountOfCommonFriends(int user_a_id, int user_b_id);

        // Challenges and badges
        IEnumerable<User> GetAllUsersThatAcceptedChallenge(int challenge_id);
        IEnumerable<User> GetAllUsersHavingBadge(int badge_id);
    }
}