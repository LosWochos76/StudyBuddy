using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class UserRepositoryMock : IUserRepository
    {
        private List<User> objects = new List<User>();

        public UserRepositoryMock()
        {
        }

        public void AddFriend(int user_id, int friend_id)
        {
            throw new NotImplementedException();
        }

        public void AddFriends(int user_id, int[] friend_ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> All(UserFilter filter)
        {
            return objects;
        }

        public User ByEmailActiveAccounts(string email)
        {
            return objects.Where(obj => obj.Email.ToLower().Equals(email.ToLower()) && obj.AccountActive).FirstOrDefault();
        }

        public User ByEmailAllAccounts(string email)
        {
            return objects.Where(obj => obj.Email.ToLower().Equals(email.ToLower())).FirstOrDefault();
        }

        public User ById(int id)
        {
            return objects.Where(obj => obj.ID == id).FirstOrDefault();
        }

        public User ByNickname(string nickname)
        {
            return objects.Where(obj => obj.Nickname.ToLower().Equals(nickname.ToLower())).FirstOrDefault();
        }

        public void Delete(int id)
        {
            objects.RemoveAll(obj => obj.ID == id);
        }

        public IEnumerable<User> GetAllLikersForNotification(int notification_id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsersHavingBadge(int badge_id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsersThatAcceptedChallenge(int challenge_id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(UserFilter filter)
        {
            return objects.Count;
        }

        public int GetCountOfCommonFriends(int user_a_id, int user_b_id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetFriends(FriendFilter filter)
        {
            throw new NotImplementedException();
        }

        public int GetFriendsCount(FriendFilter filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetNotFriends(FriendFilter filter)
        {
            throw new NotImplementedException();
        }

        public int GetNotFriendsCount(FriendFilter filter)
        {
            throw new NotImplementedException();
        }

        public void Insert(User obj)
        {
            obj.ID = GetCount(null) + 1;
            objects.Add(obj);
        }

        public void RemoveFriend(int user_id, int friend_id)
        {
            throw new NotImplementedException();
        }

        public void RemoveFriends(int user_id)
        {
            throw new NotImplementedException();
        }

        public void Save(User obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Update(User obj)
        {
            int pos = objects.FindIndex(o => o.ID == obj.ID);
            objects[pos] = obj;
        }
    }
}