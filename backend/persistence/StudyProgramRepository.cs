using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class StudyProgramRepository : IStudyProgramRepository
    {
        private StudyBuddyContext context;

        public StudyProgramRepository(StudyBuddyContext context)
        {
            this.context = context;
        }

        public IEnumerable<StudyProgram> All()
        {
            return context.StudyPrograms
                .OrderBy(x =>x.Acronym)
                .ThenBy(x => x.Name)
                .AsNoTracking().ToList<StudyProgram>();
        }

        public StudyProgram ById(int id)
        {
            return (from obj in context.StudyPrograms where obj.ID == id select obj)
                .AsNoTracking().FirstOrDefault();
        }

        public void Delete(int id)
        {
            var obj = context.StudyPrograms.Find(id);
            if (obj != null) 
            {
                context.StudyPrograms.Remove(obj);
                context.SaveChanges();
            }
        }

        public void Save(StudyProgram obj)
        {
            if (obj.ID == 0) 
                context.Add(obj);
            else
                context.StudyPrograms.Attach(obj).State = EntityState.Modified;

            context.SaveChanges();
            context.Entry(obj).State = EntityState.Detached;
        }
    }
}