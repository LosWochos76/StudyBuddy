using System;
using System.Collections.Generic;
using System.Linq;
using FirebaseAdmin.Messaging;
using StudyBuddy.BusinessLogic.Parameters;
using StudyBuddy.Model;
using StudyBuddy.Model.Enum;

namespace StudyBuddy.BusinessLogic
{
    internal class UserService : IUserService
    {
        private readonly IBackend backend;

        public UserService(IBackend backend)
        {
            this.backend = backend;
        }

        public UserList GetAll(UserFilter filter)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (filter == null)
                filter = new UserFilter();

            return new UserList
            {
                Count = backend.Repository.Users.GetCount(filter),
                Objects = backend.Repository.Users.All(filter)
            };
        }

        public User GetById(int user_id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            return backend.Repository.Users.ById(user_id);
        }

        public User Update(User obj)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != obj.ID)
                throw new Exception("Unauthorized!");

            backend.Repository.Users.Update(obj);
            return obj;
        }

        public LoginResult ResetPassword(ResetPasswordData data)
        {  
            if (data == null)
                throw new Exception("No data passed to resetpassword function!");

            User user = backend.Repository.Users.ByEmailAllAccounts(data.Email);
            if (user == null)
            {
                backend.Logging.LogDebug($"User with email {data.Email} not found!");
                return new LoginResult() { Status = LoginStatus.UserNotFound };
            }

            if (!backend.AuthenticationService.CheckPasswordResetToken(data.Token, user.PasswordHash))
            {
                backend.Logging.LogDebug($"Invalid password reset token for User with email {data.Email}!");
                return new LoginResult() { Status = LoginStatus.InvalidToken };
            }

            user.Password = data.Password;
            backend.Repository.Users.Update(user);
            return new LoginResult() { Status = LoginStatus.Success };
        }

        public LoginResult VerifyEmail(VerifyEmailData data)
        {
            if (data == null)
                throw new Exception("No data passed to verifyemail function!");

            User user = backend.Repository.Users.ByEmailAllAccounts(data.Email);
            if (user == null)
            {
                backend.Logging.LogDebug($"User with email {data.Email} not found!");
                return new LoginResult() { Status = LoginStatus.UserNotFound };
            }

            if (!backend.AuthenticationService.CheckPasswordResetToken(data.Token, user.PasswordHash))
            {
                backend.Logging.LogDebug($"Invalid verification token for user with email {data.Email}!");
                return new LoginResult() { Status = LoginStatus.InvalidToken };
            }
            
            user.EmailConfirmed = true;
            backend.Repository.Users.Update(user);
            return new LoginResult() { Status = LoginStatus.Success };
        }

        public User EnableAccount(User obj)
        {
            obj.AccountActive = true;
            backend.Repository.Users.Update(obj);
            return obj;
        }

        public User Insert(User obj)
        {
            if (backend.CurrentUser is null)
                obj.Role = Role.Student;

            backend.Repository.Users.Insert(obj);
            backend.AuthenticationService.SendMail(obj.Email, false);
            backend.BusinessEventService.TriggerEvent(this,
                new BusinessEventArgs(BusinessEventType.UserRegistered, obj));

            return obj;
        }

        public void Delete(int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id)
                throw new Exception("Unauthorized!");

            backend.Repository.Users.RemoveFriends(user_id);
            backend.Repository.Challenges.RemoveAllAcceptances(user_id);
            backend.Repository.GameBadges.RemoveAllBadgesFromUser(user_id);
            backend.Repository.Users.Delete(user_id);
        }

        public int GetUserIdByNickname(string nickname)
        {
            var obj = backend.Repository.Users.ByNickname(nickname);
            if (obj == null)
                return 0;

            return obj.ID;
        }

        public int GetUserIdByEmail(string email)
        {
            var obj = backend.Repository.Users.ByEmailActiveAccounts(email);
            if (obj == null)
                return 0;

            return obj.ID;
        }

        public UserList GetAllFriends(FriendFilter filter)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != filter.UserId)
                throw new Exception("Unauthorized!");

            return new UserList
            {
                Count = backend.Repository.Users.GetFriendsCount(filter),
                Objects = backend.Repository.Users.GetFriends(filter)
            };
        }

        public UserList GetAllNotFriends(FriendFilter filter)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != filter.UserId)
                throw new Exception("Unauthorized!");

            var objects = backend.Repository.Users.GetNotFriends(filter);
            var count = backend.Repository.Users.GetNotFriendsCount(filter);

            // ToDo: Evtl sehr inperformant!
            if (filter.WithFriendshipRequest)
                foreach (var user in objects)
                    user.FriendshipRequest = backend.Repository.Requests.FindFriendshipRequest(filter.UserId, user.ID);

            return new UserList
            {
                Count = count,
                Objects = objects
            };
        }

        public void AddFriend(int user_id, int friend_id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            backend.Repository.Users.AddFriend(user_id, friend_id);

            var user = backend.Repository.Users.ById(user_id);
            var friend = backend.Repository.Users.ById(friend_id);
            backend.BusinessEventService.TriggerEvent(this,
                new BusinessEventArgs(BusinessEventType.FriendAdded, friend) {CurrentUser = user});
        }

        public void RemoveFriend(int user_id, int friend_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id)
                throw new Exception("Unauthorized!");

            backend.Repository.Users.RemoveFriend(user_id, friend_id);
        }

        public void RemoveFriends(int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id)
                throw new Exception("Unauthorized!");

            backend.Repository.Users.RemoveFriends(user_id);
        }

        public void SetFriends(MultipleFriendsParameter parameter)
        {
            if (backend.CurrentUser == null ||
                !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != parameter.UserID)
                throw new Exception("Unauthorized!");

            backend.Repository.Users.RemoveFriends(parameter.UserID);
            if (parameter.Friends == null || parameter.Friends.Length == 0)
                return;

            backend.Repository.Users.AddFriends(parameter.UserID, parameter.Friends);
        }

        public UserList GetAllUsersThatAcceptedChallenge(int challenge_id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            var challenge = backend.Repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Challenge not found!");

            if (!backend.CurrentUser.IsAdmin && challenge.OwnerID != backend.CurrentUser.ID)
                throw new UnauthorizedAccessException("Unauthorized!");

            var objects = backend.Repository.Users.GetAllUsersThatAcceptedChallenge(challenge_id);
            return new UserList
            {
                Count = objects.Count(),
                Objects = objects
            };
        }

        public int GetCountOfCommonFriends(int user_a_id, int user_b_id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.Repository.Users.GetCountOfCommonFriends(user_a_id, user_b_id);
        }

        public UserList GetAllUsersHavingBadge(int badge_id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            var badge = backend.Repository.GameBadges.ById(badge_id);
            if (badge == null)
                throw new Exception("GameBadge not found!");

            if (!backend.CurrentUser.IsAdmin && badge.OwnerID != backend.CurrentUser.ID)
                throw new UnauthorizedAccessException("Unauthorized!");

            var objects = backend.Repository.Users.GetAllUsersHavingBadge(badge_id);
            return new UserList
            {
                Count = objects.Count(),
                Objects = objects
            };
        }

        public void SendMailToUser(MailDto mail)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (mail == null ||
                mail.RecipientID == 0 ||
                string.IsNullOrWhiteSpace(mail.Subject) ||
                string.IsNullOrWhiteSpace(mail.Message))
                throw new Exception("Data invalid!");

            var user = backend.Repository.Users.ById(mail.RecipientID);
            MailKitHelper.SendMailAsync(user.Email, mail.Subject, mail.Message);
        }

        public IEnumerable<User> GetAllLikersForNotification(int notification_id)
        {
            if (backend.CurrentUser is null)
                throw new Exception("Unauthorized!");

            var notification = backend.Repository.Notifications.ById(notification_id);
            if (notification is null)
                throw new Exception("Object is null!");

            return backend.Repository.Users.GetAllLikersForNotification(notification_id);
        }
    }
}