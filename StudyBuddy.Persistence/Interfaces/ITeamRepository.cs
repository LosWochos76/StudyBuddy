using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface ITeamRepository
    {
        Team ById(int id);
        IEnumerable<Team> All(int from = 0, int max = 1000);
        int Count();
        void Save(Team obj);
        void Insert(Team obj);
        void Update(Team obj);
        void Delete(int id);
        Team ByName(string name);

        IEnumerable<TeamMember> GetMembers(int team_id);
        void DeleteMembers(int team_id);
        void AddMembers(IEnumerable<TeamMember> members);
        void AddMember(TeamMember member);
        void RemoveMember(TeamMember member);

        IEnumerable<Team> TeamsInWhichUserIsMember(int user_id);
    }
}