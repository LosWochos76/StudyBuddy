using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface ITeamRepository
    {
        Team ById(int id);
        IEnumerable<Team> All();
        void Save(Team obj);
    }
}