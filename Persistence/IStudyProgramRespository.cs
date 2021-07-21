using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IStudyProgramRepository
    {
        StudyProgram ById(int id);
        IEnumerable<StudyProgram> All(int from = 0, int max = 1000);
        StudyProgram ByAcronym(string acronym);
        void Save(StudyProgram obj);
        void Delete(int id);
    }
}