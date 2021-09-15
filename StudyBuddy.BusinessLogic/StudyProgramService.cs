using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class StudyProgramService
    {
        private IRepository repository;

        public StudyProgramService(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<StudyProgram> All()
        {
            return repository.StudyPrograms.All();
        }

        public StudyProgram GetById(int id)
        {
            return repository.StudyPrograms.ById(id);
        }

        public StudyProgram Update(StudyProgram obj)
        {
            repository.StudyPrograms.Update(obj);
            return obj;
        }

        public StudyProgram Insert(StudyProgram obj)
        {
            repository.StudyPrograms.Insert(obj);
            return obj;
        }

        public void Delete(int id)
        {
            repository.StudyPrograms.Delete(id);
        }
    }
}
