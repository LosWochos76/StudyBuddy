using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class UserService
    {
        private IRepository repository;

        public UserService(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<User> All()
        {
            return repository.Users.All();
        }

        public int GetCount()
        {
            return repository.Users.Count();
        }

        public User GetById(User current_user, int user_id)
        {
            if (current_user == null || (!current_user.IsAdmin && current_user.ID != user_id))
                throw new Exception("Unauthorized!");

            return repository.Users.ById(user_id);
        }

        public User Update(User current_user, User obj)
        {
            if (current_user == null || (!current_user.IsAdmin && current_user.ID != obj.ID))
                throw new Exception("Unauthorized!");

            repository.Users.Update(obj);
            return obj;
        }

        public User Insert(User obj)
        {
            repository.Users.Insert(obj);
            return obj;
        }

        public void Delete(User current_user, int id)
        {
            if (current_user == null || (!current_user.IsAdmin && current_user.ID != id))
                throw new Exception("Unauthorized!");

            repository.Users.RemoveFriends(id);
            repository.Users.Delete(id);
        }

        public int UserIdByNickname(string nickname)
        {
            var obj = repository.Users.ByNickname(nickname);
            if (obj == null)
                return 0;

            return obj.ID;
        }

        public int UserIdByEmail(string email)
        {
            var obj = repository.Users.ByEmail(email);
            if (obj == null)
                return 0;

            return obj.ID;
        }

        public IEnumerable<User> AllFriends(User current_user, int user_id)
        {
            if (!current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");

            return repository.Users.GetFriends(user_id);
        }

        public void AddFriend(SingleFriendParameter parameter)
        {
            repository.Users.AddFriend(parameter.UserID, parameter.FriendID);
        }

        public void RemoveFriend(User current_user, SingleFriendParameter parameter)
        {
            if (!current_user.IsAdmin && current_user.ID != parameter.UserID)
                throw new Exception("Unauthorized!");

            repository.Users.RemoveFriend(parameter.UserID, parameter.FriendID);
        }

        public void RemoveFriends(User current_user, int id)
        {
            if (!current_user.IsAdmin)
                throw new Exception("Unauthorized!");

            repository.Users.RemoveFriends(id);
        }

        public void SetFriends(User current_user, MultipleFriendsParameter parameter)
        {
            if (!current_user.IsAdmin)
                throw new Exception("Unauthorized!");

            repository.Users.RemoveFriends(parameter.UserID);

            if (parameter.Friends == null)
                return;

            repository.Users.AddFriends(parameter.UserID, parameter.Friends);
        }
    }
}