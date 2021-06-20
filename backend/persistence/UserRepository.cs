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
            return (from obj in context.Users where obj.ID == id select obj)
                .AsNoTracking().FirstOrDefault();
        }

        public IEnumerable<User> All()
        {
            return context.Users
                .OrderBy(x =>x.Lastname)
                .ThenBy(x => x.Firstname)
                .AsNoTracking().ToList<User>();
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

        public User FindByEmail(string email)
        {
            return (from obj in context.Users 
                where obj.Email.Equals(email) select obj).AsNoTracking().FirstOrDefault();
        }

        public void Save(User obj)
        {
            if (obj.ID == 0) 
                context.Add(obj);
            else
                context.Users.Attach(obj).State = EntityState.Modified;

            context.SaveChanges();
            context.Entry(obj).State = EntityState.Detached;
        }

        public void Delete(int id)
        {
            var obj = context.Users.Find(id);
            if (obj != null) 
            {
                context.Users.Remove(obj);
                context.SaveChanges();
            }
        }

        public User FindByNickname(string nickname)
        {
           return (from obj in context.Users 
                where obj.Nickname.Equals(nickname) select obj).AsNoTracking().FirstOrDefault();
        }
    }
}