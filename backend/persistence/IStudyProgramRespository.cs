using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IStudyProgramRepository
    {
        StudyProgram ById(int id);
        IEnumerable<StudyProgram> All();
        void Save(StudyProgram obj);
        void Delete(int id);
    }
}