using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IUserRepository
    {
        User ById(int id);
        IEnumerable<User> All(int from = 0, int max = 1000);
        int Count();
        User ByEmailAndPassword(string email, string password);
        User ByEmail(string email);
        User ByNickname(string nickname);
        void Save(User obj);
        void Insert(User obj);
        void Update(User obj);
        void Delete(int id);

        IEnumerable<User> GetFriends(int user_id, int from = 0, int max = 1000);
        void AddFriend(int user_id, int friend_id);
        void RemoveFriend(int user_id, int friend_id);
        void RemoveFriends(int user_id);
        void AddFriends(int user_id, int[] friend_ids);
        int GetCountOfCommonFriends(int user_a_id, int user_b_id);

        IEnumerable<User> GetAllUsersThatAcceptedChallenge(int challenge_id);
    }
}