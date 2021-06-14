using Microsoft.EntityFrameworkCore;
using SimpleHashing.Net;
using StudyBuddy.Model;
using System.Collections.Generic;
using System.Linq;

namespace StudyBuddy.Persistence
{
    class UserRepository : IUserRepository
    {
        private StudyBuddyContext context;
        private SimpleHash simpleHash = new SimpleHash();

        public UserRepository(StudyBuddyContext context)
        {
            this.context = context;
        }

        public User ById(int id)
        {
            var user = context.Users.Find(id);
            return user;
        }

        public IEnumerable<User> All()
        {
            return context.Users.AsNoTracking().ToList<User>();
        }

        public User FindByEmailAndPassword(string email, string password)
        {
            var list = (from obj in context.Users 
                where obj.Email.Equals(email) select obj).AsNoTracking().ToList();

            foreach (var obj in list)
                if (simpleHash.Verify(password, obj.PasswordHash))
                    return obj;
            
            return null;
        }
    }
}