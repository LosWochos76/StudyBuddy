using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class TeamRepository : ITeamRepository
    {
        private string connection_string;
        private QueryHelper<Team> team_qh;
        private QueryHelper<TeamMember> members_qh;

        public TeamRepository(string connection_string)
        {
            this.connection_string = connection_string;
            this.team_qh = new QueryHelper<Team>(connection_string, TeamFromReader);
            this.members_qh = new QueryHelper<TeamMember>(connection_string, TeamMemberFromReader);

            CreateTeamTable();
            CreateTeamMemberTable();
        }

        private void CreateTeamTable() 
        {
            if (!team_qh.TableExists("teams"))
            {
                team_qh.ExecuteNonQuery(
                    "create table teams (" +
                    "id serial primary key, " +
                    "name varchar(100) not null," +
                    "owner_id int not null)");
            }
        }
        
        private void CreateTeamMemberTable() 
        {
            if (!members_qh.TableExists("team_members"))
            {
                members_qh.ExecuteNonQuery(
                    "create table team_members (" +
                    "team_id int not null, " +
                    "member_id int not null)");
            }
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
            team_qh.AddParameter(":id", id);
            return team_qh.ExecuteQueryToSingleObject(
                "SELECT id,name,owner_id FROM teams where id=:id");
        }

        public Team ByName(string name)
        {
            team_qh.AddParameter(":name", name);
            return team_qh.ExecuteQueryToSingleObject(
                "SELECT id,name,owner_id FROM teams where lower(name)=lower(:name)");
        }

        public IEnumerable<Team> All(int from = 0, int max = 1000)
        {
            team_qh.AddParameter(":max", max);
            team_qh.AddParameter(":from", from);
            return team_qh.ExecuteQueryToObjectList(
                "SELECT id,name,owner_id FROM teams order by name limit :max offset :from");
        }

        public IEnumerable<Team> TeamsInWhichUserIsMember(int user_id)
        {
            team_qh.AddParameter(":member_id", user_id);
            return team_qh.ExecuteQueryToObjectList(
                "select id, name, owner_id from teams " +
                "inner join team_members on team_id = id " +
                "where member_id = :member_id order by name");
        }

        public int Count()
        {
            return team_qh.GetCount("teams");
        }

        public void Delete(int id)
        {
            team_qh.Delete("teams", "id", id);
        }

        public void Insert(Team obj)
        {
            team_qh.AddParameter(":name", obj.Name);
            team_qh.AddParameter(":owner_id", obj.OwnerID);
            obj.ID = team_qh.ExecuteScalar("insert into teams " +
                    "(name, owner_id) values " +
                    "(:name, :owner_id) RETURNING id");
        }

        public void Update(Team obj)
        {
            team_qh.AddParameter(":id", obj.ID);
            team_qh.AddParameter(":name", obj.Name);
            team_qh.AddParameter(":owner_id", obj.OwnerID);
            team_qh.ExecuteNonQuery("update teams set name=:name, owner_id=:owner_id where id=:id");
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
            members_qh.AddParameter(":team_id", team_id);
            return members_qh.ExecuteQueryToObjectList(
                "select team_id,member_id FROM team_members where team_id=:team_id");
        }

        public void DeleteMembers(int team_id)
        {
            members_qh.Delete("team_members", "team_id", team_id);
        }

        // ToDo: Es gibt wahrscheinlich einen besseren Weg, mehrer Zeilen in die Datenbank zu schreiben
        public void AddMembers(IEnumerable<TeamMember> members)
        {
            foreach (var member in members)
                AddMember(member);
        }

        public void AddMember(TeamMember member)
        {
            members_qh.AddParameter(":team_id", member.TeamId);
            members_qh.AddParameter(":member_id", member.MemberId);
            members_qh.ExecuteNonQuery("insert into team_members (team_id,member_id) values (:team_id,:member_id)");
        }

        public void RemoveMember(TeamMember member)
        {
            members_qh.AddParameter(":team_id", member.TeamId);
            members_qh.AddParameter(":member_id", member.MemberId);
            members_qh.ExecuteNonQuery("delete from team_members where team_id=:team_id and member_id=:member_id");
        }
    }
}