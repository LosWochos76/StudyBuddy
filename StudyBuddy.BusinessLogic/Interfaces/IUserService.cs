using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IUserService
    {
        // Basic CRUD-Methods for Users
        User GetById(int user_id);
        int GetUserIdByEmail(string email);
        int GetUserIdByNickname(string nickname);
        IEnumerable<User> GetAll(UserFilter filter);
        int GetCount();
        User Insert(User obj);
        User Update(User obj);
        void Delete(int user_id);

        // Friendship
        void AddFriend(int user_id, int friend_id);
        IEnumerable<User> GetAllFriends(FriendFilter filter);
        int GetAllFriendsCount(int user_id);
        IEnumerable<User> GetAllNotFriends(FriendFilter filter);
        int GetCountOfCommonFriends(int user_a_id, int user_b_id);
        void RemoveFriend(int user_id, int friend_id);
        void RemoveFriends(int user_id);
        void SetFriends(MultipleFriendsParameter parameter);
        
        // Challenges and Badges
        IEnumerable<User> GetAllUsersThatAcceptedChallenge(int challenge_id);
        IEnumerable<User> GetAllUsersHavingBadge(int badge_id);
    }
}