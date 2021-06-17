using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IUserRepository
    {
        User ById(int id);
        IEnumerable<User> All();
        User FindByEmailAndPassword(string email, string password);
        User FindByEmail(string email);
        void Save(User obj);
    }
}
