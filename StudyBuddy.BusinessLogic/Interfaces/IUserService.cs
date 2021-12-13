using System.Collections.Generic;
using System.Drawing;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IUserService
    {
        void AddFriend(int user_id, int friend_id);
        void Delete(int user_id);
        IEnumerable<User> GetAll(UserFilter filter);
        IEnumerable<User> GetAllFriends(FriendFilter filter);
        IEnumerable<User> GetAllNotFriends(FriendFilter filter);
        User GetById(int user_id);
        int GetCount();
        int GetCountOfCommonFriends(int user_a_id, int user_b_id);
        int GetUserIdByEmail(string email);
        int GetUserIdByNickname(string nickname);
        User Insert(User obj);
        void RemoveFriend(int user_id, int friend_id);
        void RemoveFriends(int user_id);
        void SetFriends(MultipleFriendsParameter parameter);
        User Update(User obj);

        IEnumerable<User> GetAllUsersThatAcceptedChallenge(int challenge_id);
        IEnumerable<User> GetAllUsersHavingBadge(int badge_id);
    }
}