﻿using System.Collections.Generic;
using StudyBuddy.BusinessLogic.Parameters;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IUserService
    {
        // Basic CRUD-Methods for Users
        User GetById(int user_id);
        UserList GetAll(UserFilter filter);
        User Insert(User obj);
        User Update(User obj);
        LoginResult ResetPassword(ResetPasswordData data);
        LoginResult GeneratePassword(ResetPasswordData data);
        LoginResult VerifyEmail(VerifyEmailData data);
        User EnableAccount(User obj);
        void Delete(int user_id);

        // Find specific users
        int GetUserIdByEmail(string email);
        int GetUserIdByNickname(string nickname);

        // Friendship
        UserList GetAllFriends(FriendFilter filter);
        UserList GetAllNotFriends(FriendFilter filter);
        void AddFriend(int user_id, int friend_id);
        void RemoveFriend(int user_id, int friend_id);
        void RemoveFriends(int user_id);
        void SetFriends(MultipleFriendsParameter parameter);
        int GetCountOfCommonFriends(int user_a_id, int user_b_id);

        // Challenges and Badges
        UserList GetAllUsersThatAcceptedChallenge(int challenge_id);
        UserList GetAllUsersHavingBadge(int badge_id);

        // Misc
        void SendMailToUser(MailDto mail);
        IEnumerable<User> GetAllLikersForNotification(int notification_id);
    }
}