using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class TeamRepository : SqlRepositoryBase, ITeamRepository
    {
        public TeamRepository(NpgsqlConnection connection) : base(connection)
        {
            if (!TableExists("teams")) 
                CreateTeamTable();

            if (!TableExists("team_members")) 
                CreateTeamMemberTable();
        }

        private void CreateTeamTable() 
        {
            string sql = "create table teams (" +
                "id serial primary key, " +
                "name varchar(100) not null)";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.ExecuteNonQuery();
            }
        }
        
        private void CreateTeamMemberTable() 
        {
            string sql = "create table team_members (" +
                "team_id int not null, " +
                "member_id int not null)";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.ExecuteNonQuery();
            }
        }

        private Team FromReader(NpgsqlDataReader reader)
        {
            var obj = new Team();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            return obj;
        }

        public Team ById(int id)
        {
            string sql = "SELECT id,name FROM teams where id=:id";

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":id", id);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return FromReader(reader);
                    }
                }
            }

            return null;
        }

        public IEnumerable<Team> All(int from = 0, int max = 1000)
        {
            string sql = "SELECT id,name FROM teams limit :max offset :from";
            
            var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue(":from", from);
            cmd.Parameters.AddWithValue(":max", max);
            var result = new List<Team>();

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var obj = FromReader(reader);
                    result.Add(obj);
                }
            }
               
            return result;
        }

        public void Delete(int id)
        {
            string sql = "delete from teams where id=:id";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void Insert(Team obj)
        {
            string sql = "insert into teams " +
                    "(name) values " +
                    "(:name) RETURNING id";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":name", obj.Name);
                obj.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void Update(Team obj)
        {
            string sql = "update teams set name=:name where id=:id";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":id", obj.ID);
                cmd.Parameters.AddWithValue(":name", obj.Name);
                obj.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void Save(Team obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void AddMember(int team_id, int member_id)
        {
            string sql = "insert into team_members " +
                    "(team_id,member_id) values " +
                    "(:team_id,:member_id)";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":team_id", team_id);
                cmd.Parameters.AddWithValue(":member_id", member_id);
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveMember(int team_id, int member_id)
        {
            string sql = "delete from team_members where team_id=:team_id and member_id=:member_id";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":team_id", team_id);
                cmd.Parameters.AddWithValue(":member_id", member_id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}