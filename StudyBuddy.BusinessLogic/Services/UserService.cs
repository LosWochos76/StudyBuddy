using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    class UserService : IUserService
    {
        private readonly IBackend backend;

        public UserService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<User> GetAll(UserFilter filter)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (filter == null)
                filter = new UserFilter();

            return backend.Repository.Users.All(filter);
        }

        public int GetCount()
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            return backend.Repository.Users.Count();
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

        public User Insert(User obj)
        {
            backend.Repository.Users.Insert(obj);
            backend.BusinessEvent.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.UserRegistered, obj));
            return obj;
        }

        public void Delete(int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id)
                throw new Exception("Unauthorized!");

            backend.Repository.Users.RemoveFriends(user_id);
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
            var obj = backend.Repository.Users.ByEmail(email);
            if (obj == null)
                return 0;

            return obj.ID;
        }

        public IEnumerable<User> GetAllFriends(int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id)
                throw new Exception("Unauthorized!");

            return backend.Repository.Users.GetFriends(user_id);
        }

        public IEnumerable<User> GetAllNotFriends(int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id)
                throw new Exception("Unauthorized!");

            return backend.Repository.Users.GetNotFriends(user_id);
        }

        public void AddFriend(int user_id, int friend_id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            backend.Repository.Users.AddFriend(user_id, friend_id);

            var user = backend.Repository.Users.ById(user_id);
            var friend = backend.Repository.Users.ById(friend_id);
            backend.BusinessEvent.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.FriendAdded, friend) { CurrentUser = user });
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
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != parameter.UserID)
                throw new Exception("Unauthorized!");

            backend.Repository.Users.RemoveFriends(parameter.UserID);
            if (parameter.Friends == null || parameter.Friends.Length == 0)
                return;

            backend.Repository.Users.AddFriends(parameter.UserID, parameter.Friends);
        }

        public IEnumerable<User> GetAllUsersThatAcceptedChallenge(int challenge_id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            var challenge = backend.Repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Challenge not found!");

            if (!backend.CurrentUser.IsAdmin && challenge.OwnerID != backend.CurrentUser.ID)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.Repository.Users.GetAllUsersThatAcceptedChallenge(challenge_id);
        }

        public int GetCountOfCommonFriends(int user_a_id, int user_b_id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.Repository.Users.GetCountOfCommonFriends(user_a_id, user_b_id);
        }
    }
}