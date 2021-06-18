using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class TeamRepository : ITeamRepository
    {
        private StudyBuddyContext context;

        public TeamRepository(StudyBuddyContext context)
        {
            this.context = context;
        }

        public IEnumerable<Team> All()
        {
            return context.Teams.AsNoTracking().ToList<Team>();
        }

        public Team ById(int id)
        {
            return (from obj in context.Teams where obj.ID == id select obj)
                .AsNoTracking().FirstOrDefault();
        }

        public void Delete(int id)
        {
            var obj = context.Teams.Find(id);
            if (obj != null) 
            {
                context.Teams.Remove(obj);
                context.SaveChanges();
            }
        }

        public void Save(Team obj)
        {
            if (obj.ID == 0) 
                context.Add(obj);
            else
                context.Teams.Attach(obj).State = EntityState.Modified;

            context.SaveChanges();
            context.Entry(obj).State = EntityState.Detached;
        }
    }
}