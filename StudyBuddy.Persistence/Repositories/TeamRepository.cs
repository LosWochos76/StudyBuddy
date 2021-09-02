using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class TeamRepository : SqlRepositoryBase, ITeamRepository
    {
        public TeamRepository(string connection_string) : base(connection_string)
        {
            if (!TableExists("teams")) 
                CreateTeamTable();

            if (!TableExists("team_members")) 
                CreateTeamMemberTable();
        }

        private void CreateTeamTable() 
        {
            var qh = new QueryHelper<Team>(connection_string);
            qh.ExecuteNonQuery("create table teams (" +
                "id serial primary key, " +
                "name varchar(100) not null," +
                "owner_id int not null)");
        }
        
        private void CreateTeamMemberTable() 
        {
            var qh = new QueryHelper<TeamMember>(connection_string);
            qh.ExecuteNonQuery("create table team_members (" +
                "team_id int not null, " +
                "member_id int not null)");
        }

        private Team TeamFromReader(NpgsqlDataReader reader)
        {
            var obj = new Team();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            obj.OwnerID = reader.GetInt32(2);
            return obj;
        }

        private TeamMember TeamMemberFromReader(NpgsqlDataReader reader)
        {
            var obj = new TeamMember();
            obj.TeamId = reader.GetInt32(0);
            obj.MemberId = reader.GetInt32(1);
            return obj;
        }

        public Team ById(int id)
        {
            var qh = new QueryHelper<Team>(connection_string);
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,name,owner_id FROM teams where id=:id",
                TeamFromReader);
        }

        public IEnumerable<Team> All(int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Team>(connection_string);
            qh.AddParameter(":max", max);
            qh.AddParameter(":from", from);
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,owner_id FROM teams order by name limit :max offset :from",
                TeamFromReader);
        }

        public int Count()
        {
            var qh = new QueryHelper<Team>(connection_string);
            return qh.GetCount("teams");
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper<Team>(connection_string);
            qh.Delete("teams", "id", id);
        }

        public void Insert(Team obj)
        {
            var qh = new QueryHelper<Team>(connection_string);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            obj.ID = qh.ExecuteScalar("insert into teams " +
                    "(name, owner_id) values " +
                    "(:name, :owner_id) RETURNING id");
        }

        public void Update(Team obj)
        {
            var qh = new QueryHelper<Team>(connection_string);
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.ExecuteNonQuery("update teams set name=:name, owner_id=:owner_id where id=:id");
        }

        public void Save(Team obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public IEnumerable<TeamMember> GetMembers(int team_id)
        {
            var qh = new QueryHelper<TeamMember>(connection_string);
            qh.AddParameter(":team_id", team_id);
            return qh.ExecuteQueryToObjectList(
                "select team_id,member_id FROM team_members where team_id=:team_id",
                TeamMemberFromReader);
        }

        public void DeleteMembers(int team_id)
        {
            var qh = new QueryHelper<TeamMember>(connection_string);
            qh.Delete("team_members", "team_id", team_id);
        }

        // ToDo: Es gibt wahrscheinlich einen besseren Weg, mehrer Zeilen in die Datenbank zu schreiben
        public void AddMembers(IEnumerable<TeamMember> members)
        {
            foreach (var member in members)
                AddMember(member);
        }

        public void AddMember(TeamMember member)
        {
            var qh = new QueryHelper<TeamMember>(connection_string);
            qh.AddParameter(":team_id", member.TeamId);
            qh.AddParameter(":member_id", member.MemberId);
            qh.ExecuteNonQuery("insert into team_members (team_id,member_id) values (:team_id,:member_id)");
        }

        public void RemoveMember(TeamMember member)
        {
            var qh = new QueryHelper<TeamMember>(connection_string);
            qh.AddParameter(":team_id", member.TeamId);
            qh.AddParameter(":member_id", member.MemberId);
            qh.ExecuteNonQuery("delete from team_members where team_id=:team_id and member_id=:member_id");
        }

        public Team ByName(string name)
        {
            var qh = new QueryHelper<Team>(connection_string);
            qh.AddParameter(":name", name);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,name,owner_id FROM teams where lower(name)=lower(:name)",
                TeamFromReader);
        }

        public IEnumerable<Team> TeamsInWhichUserIsMember(int user_id)
        {
            var qh = new QueryHelper<Team>(connection_string);
            qh.AddParameter(":member_id", user_id);
            return qh.ExecuteQueryToObjectList(
                "select id, name, owner_id from teams " +
                "inner join team_members on team_id = id " +
                "where member_id = :member_id order by name",
                TeamFromReader);
        }
    }
}