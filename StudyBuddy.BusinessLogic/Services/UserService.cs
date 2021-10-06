using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class UserService
    {
        private readonly User current_user;
        private readonly IRepository repository;

        public UserService(IRepository repository, User current_user)
        {
            this.repository = repository;
            this.current_user = current_user;
        }

        public IEnumerable<User> GetAll()
        {
            if (current_user == null)
                throw new Exception("Unauthorized!");

            return repository.Users.All();
        }

        public int GetCount()
        {
            if (current_user == null)
                throw new Exception("Unauthorized!");

            return repository.Users.Count();
        }

        public User GetById(int user_id)
        {
            if (current_user == null || !current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");

            return repository.Users.ById(user_id);
        }

        public User Update(User obj)
        {
            if (current_user == null || !current_user.IsAdmin && current_user.ID != obj.ID)
                throw new Exception("Unauthorized!");

            repository.Users.Update(obj);
            return obj;
        }

        public User Insert(User obj)
        {
            repository.Users.Insert(obj);
            return obj;
        }

        public void Delete(int user_id)
        {
            if (current_user == null || !current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");

            repository.Users.RemoveFriends(user_id);
            repository.Users.Delete(user_id);
        }

        public int GetUserIdByNickname(string nickname)
        {
            var obj = repository.Users.ByNickname(nickname);
            if (obj == null)
                return 0;

            return obj.ID;
        }

        public int GetUserIdByEmail(string email)
        {
            var obj = repository.Users.ByEmail(email);
            if (obj == null)
                return 0;

            return obj.ID;
        }

        public IEnumerable<User> GetAllFriends(int user_id)
        {
            if (current_user == null || !current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");

            return repository.Users.GetFriends(user_id);
        }

        public void AddFriend(int user_id, int friend_id)
        {
            if (current_user == null || !current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");

            repository.Users.AddFriend(user_id, friend_id);
        }

        public void RemoveFriend(int user_id, int friend_id)
        {
            if (current_user == null || !current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");

            repository.Users.RemoveFriend(user_id, friend_id);
        }

        public void RemoveFriends(int user_id)
        {
            if (current_user == null || !current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");

            repository.Users.RemoveFriends(user_id);
        }

        public void SetFriends(MultipleFriendsParameter parameter)
        {
            if (current_user == null || !current_user.IsAdmin && current_user.ID != parameter.UserID)
                throw new Exception("Unauthorized!");

            repository.Users.RemoveFriends(parameter.UserID);
            if (parameter.Friends == null || parameter.Friends.Length == 0)
                return;

            repository.Users.AddFriends(parameter.UserID, parameter.Friends);
        }

        public IEnumerable<User> GetAllUsersThatAcceptedChallenge(int challenge_id)
        {
            if (current_user == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            var challenge = repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Challenge not found!");

            if (!current_user.IsAdmin && challenge.OwnerID != current_user.ID)
                throw new UnauthorizedAccessException("Unauthorized!");

            return repository.Users.GetAllUsersThatAcceptedChallenge(challenge_id);
        }

        public int GetCountOfCommonFriends(int user_a_id, int user_b_id)
        {
            if (current_user == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return repository.Users.GetCountOfCommonFriends(user_a_id, user_b_id);
        }
    }
}