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
        IEnumerable<User> NotMembersOfTeam(int team_id);
        IEnumerable<User> MembersOfTeam(int team_id);
    }
}
