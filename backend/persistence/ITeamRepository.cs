using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface ITeamRepository
    {
        Team ById(int id);
        IEnumerable<Team> All(int from = 0, int max = 1000);
        void Save(Team obj);
        void Delete(int id);
        void AddMember(int team_id, int member_id);
        void RemoveMember(int team_id, int member_id);
    }
}