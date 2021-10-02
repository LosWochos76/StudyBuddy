using System;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public class UserAuthorization
    {
        public static void CheckGetAll(User current_user)
        {
            if (current_user == null || !current_user.IsAdmin)
                throw new Exception("Unauthorized!");
        }

        public static void CheckGetCount(User current_user)
        {
            CheckGetAll(current_user);
        }

        public static void CheckGetById(User current_user, int user_id)
        {
            if (current_user == null || !current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");
        }

        public static void CheckUpdate(User current_user, User user)
        {
            CheckGetById(current_user, user.ID);
        }

        public static void CheckInsert(User current_user, User user)
        {
        }

        public static void CheckDelete(User current_user, int user_id)
        {
            CheckGetById(current_user, user_id);
        }

        public static void CheckGetUserIdByNickname(User current_user, string nickname)
        {
        }

        public static void CheckGetUserIdByEmail(User current_user, string email)
        {
        }

        public static void CheckGetAllFriends(User current_user, int user_id)
        {
            CheckGetById(current_user, user_id);
        }

        public static void CheckAddFriend(User current_user, SingleFriendParameter parameter)
        {
            CheckGetById(current_user, parameter.UserID);
        }

        public static void CheckRemoveFriend(User current_user, SingleFriendParameter parameter)
        {
            CheckGetById(current_user, parameter.UserID);
        }

        public static void CheckRemoveFriends(User current_user, int user_id)
        {
            CheckGetById(current_user, user_id);
        }

        public static void CheckSetFriends(User current_user, MultipleFriendsParameter parameter)
        {
            CheckGetById(current_user, parameter.UserID);
        }
    }
}