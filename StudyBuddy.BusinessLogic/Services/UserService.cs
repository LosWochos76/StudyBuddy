using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class UserService
    {
        private IRepository repository;
        private User current_user;

        public UserService(IRepository repository, User current_user)
        {
            this.repository = repository;
            this.current_user = current_user;
        }

        public IEnumerable<User> GetAll()
        {
            UserAuthorization.CheckGetAll(current_user);
            return repository.Users.All();
        }

        public int GetCount()
        {
            UserAuthorization.CheckGetCount(current_user);
            return repository.Users.Count();
        }

        public User GetById(int user_id)
        {
            UserAuthorization.CheckGetById(current_user, user_id);
            return repository.Users.ById(user_id);
        }

        public User Update(User obj)
        {
            UserAuthorization.CheckUpdate(current_user, obj);
            repository.Users.Update(obj);
            return obj;
        }

        public User Insert(User obj)
        {
            UserAuthorization.CheckInsert(current_user, obj);
            repository.Users.Insert(obj);
            return obj;
        }

        public void Delete(int user_id)
        {
            UserAuthorization.CheckDelete(current_user, user_id);
            repository.Users.RemoveFriends(user_id);
            repository.Users.Delete(user_id);
        }

        public int GetUserIdByNickname(string nickname)
        {
            UserAuthorization.CheckGetUserIdByNickname(current_user, nickname);

            var obj = repository.Users.ByNickname(nickname);
            if (obj == null)
                return 0;

            return obj.ID;
        }

        public int GetUserIdByEmail(string email)
        {
            UserAuthorization.CheckGetUserIdByNickname(current_user, email);

            var obj = repository.Users.ByEmail(email);
            if (obj == null)
                return 0;

            return obj.ID;
        }

        public IEnumerable<User> GetAllFriends(int user_id)
        {
            UserAuthorization.CheckGetAllFriends(current_user, user_id);
            return repository.Users.GetFriends(user_id);
        }

        public void AddFriend(SingleFriendParameter parameter)
        {
            UserAuthorization.CheckAddFriend(current_user, parameter);
            repository.Users.AddFriend(parameter.UserID, parameter.FriendID);
        }

        public void RemoveFriend(SingleFriendParameter parameter)
        {
            UserAuthorization.CheckRemoveFriend(current_user, parameter);
            repository.Users.RemoveFriend(parameter.UserID, parameter.FriendID);
        }

        public void RemoveFriends(int user_id)
        {
            UserAuthorization.CheckRemoveFriends(current_user, user_id);
            repository.Users.RemoveFriends(user_id);
        }

        public void SetFriends(MultipleFriendsParameter parameter)
        {
            UserAuthorization.CheckSetFriends(current_user, parameter);

            repository.Users.RemoveFriends(parameter.UserID);
            if (parameter.Friends == null || parameter.Friends.Length == 0)
                return;

            repository.Users.AddFriends(parameter.UserID, parameter.Friends);
        }
    }
}