using System;
using System.Collections.Generic;
using System.Linq;
using SimpleHashing.Net;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class UserRepositoryMock : IUserRepository
    {
        private ChallengeRepositoryMock challenges;
        private NotificationUserMetadataRepositoryMock meta_data;
        private List<User> objects = new List<User>();
        private HashSet<Tuple<int, int>> friends = new HashSet<Tuple<int, int>>();
        private readonly SimpleHash simpleHash = new SimpleHash();

        public UserRepositoryMock(
            ChallengeRepositoryMock challenges,
            NotificationUserMetadataRepositoryMock meta_data)
        {
            this.challenges = challenges;
            this.meta_data = meta_data;
        }

        public void AddFriend(int user_id, int friend_id)
        {
            friends.Add(new Tuple<int, int>(user_id, friend_id));
            friends.Add(new Tuple<int, int>(friend_id, user_id));
        }

        public void AddFriends(int user_id, int[] friend_ids)
        {
            foreach (var friend_id in friend_ids)
                AddFriend(user_id, friend_id);
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
            foreach (var n in meta_data.AllForNotification(notification_id))
                if (n.Liked.HasValue && n.Liked.Value)
                    yield return ById(n.OwnerId);
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

        private List<User> AllFriends(int user_id)
        {
            return friends
                .Where(obj => obj.Item1 == user_id)
                .Select(obj => ById(obj.Item2))
                .ToList();
        }

        public IEnumerable<User> GetFriends(FriendFilter filter)
        {
            return AllFriends(filter.UserId)
                .Skip(filter.Start)
                .Take(filter.Count);
        }

        public int GetFriendsCount(FriendFilter filter)
        {
            return AllFriends(filter.UserId).Count;
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
            if (obj.ID == 0)
                obj.ID = GetCount(null) + 1;

            obj.PasswordHash = simpleHash.Compute(obj.Password);
            objects.Add(obj);
        }

        public void RemoveFriend(int user_id, int friend_id)
        {
            
            throw new NotImplementedException();
        }

        public void RemoveFriends(int user_id)
        {
            //todo
            //throw new NotImplementedException();
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