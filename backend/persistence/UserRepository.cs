using Npgsql;
using SimpleHashing.Net;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudyBuddy.Persistence
{
    class UserRepository : SqlRepositoryBase, IUserRepository
    {
        private SimpleHash simpleHash = new SimpleHash();

        public UserRepository(NpgsqlConnection connection) : base(connection)
        {
            if (!TableExists("users")) 
            {
                CreateTable();

                Save(new User() { 
                    Firstname="Alexander", 
                    Lastname="Stuckenholz", 
                    Nickname="Stucki",
                    Email="alexander.stuckenholz@hshl.de",
                    PasswordHash=simpleHash.Compute("secret"),
                    Role=Role.Admin});
                
                Save(new User() { 
                    Firstname="Eva", 
                    Lastname="Ponick",
                    Nickname="Eva",
                    Email="eva.ponick@hshl.de",
                    PasswordHash=simpleHash.Compute("secret"),
                    Role=Role.Admin});
            }
        }

        private void CreateTable() 
        {
            string sql = "create table users (" +
                "id serial primary key, " +
                "firstname varchar(100) not null, " +
                "lastname varchar(100) not null, " +
                "nickname varchar(100) not null, " +
                "email varchar(100) not null, " +
                "password_hash varchar(100), " +
                "role int not null, " + 
                "program_id int, " + 
                "enrolled_in_term_id int)";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.ExecuteNonQuery();
            }
        }

        private User FromReader(NpgsqlDataReader reader)
        {
            var obj = new User();
            obj.ID = reader.GetInt32(0);
            obj.Firstname = reader.GetString(1);
            obj.Lastname = reader.GetString(2);
            obj.Nickname = reader.GetString(3);
            obj.Email = reader.GetString(4);
            obj.PasswordHash = reader.GetString(5);
            obj.Role = (Role)reader.GetInt32(6);
            obj.ProgramID = reader.IsDBNull(7) ? null : reader.GetInt32(7);
            obj.EnrolledInTermID = reader.IsDBNull(8) ? null : reader.GetInt32(8);
            return obj;
        }

        public User ById(int id)
        {
            string sql = "SELECT id,firstname,lastname,nickname," + 
                "email,password_hash,role,program_id,enrolled_in_term_id FROM users where id=:id";

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

        public IEnumerable<User> All(int from = 0, int max = 1000)
        {
            string sql = "SELECT id,firstname,lastname,nickname,email,password_hash,role," +
                "program_id,enrolled_in_term_id FROM users order by lastname,firstname,nickname limit :max offset :from";
            
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":from", from);
                cmd.Parameters.AddWithValue(":max", max);
                var result = new List<User>();

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
        }

        public User FindByEmailAndPassword(string email, string password)
        {
            var user = FindByEmail(email);
            if (user != null && simpleHash.Verify(password, user.PasswordHash))
                return user;
            else
                return null;
        }

        public User FindByEmail(string email)
        {
            string sql = "SELECT id,firstname,lastname,nickname," + 
                "email,password_hash,role,program_id,enrolled_in_term_id FROM users where email=:email";

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":email", email);

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

        private void Insert(User obj)
        {
            string sql = "insert into users " +
                    "(firstname,lastname,nickname,email,password_hash,role,program_id,enrolled_in_term_id) values " +
                    "(:firstname,:lastname,:nickname,:email,:password_hash,:role,:program_id,:enrolled_in_term_id) RETURNING id";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":firstname", obj.Firstname);
                cmd.Parameters.AddWithValue(":lastname", obj.Lastname);
                cmd.Parameters.AddWithValue(":nickname", obj.Nickname);
                cmd.Parameters.AddWithValue(":email", obj.Email);
                cmd.Parameters.AddWithValue(":password_hash", obj.PasswordHash);
                cmd.Parameters.AddWithValue(":role", (int)obj.Role);
                cmd.Parameters.AddWithValue(":program_id", obj.ProgramID.HasValue ? obj.ProgramID : DBNull.Value);
                cmd.Parameters.AddWithValue(":enrolled_in_term_id", obj.EnrolledInTermID.HasValue ? obj.EnrolledInTermID : DBNull.Value);
                obj.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void Update(User obj)
        {
            string sql = "update users set firstname=:firstname,lastname=:lastname,"+
                "nickname=:nickname,email=:email";
                
            if (!string.IsNullOrEmpty(obj.PasswordHash))
                sql += ",password_hash=:password_hash";
            
            sql += ",role=:role,program_id=:program_id,enrolled_in_term_id=:enrolled_in_term_id where id=:id";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":id", obj.ID);
                cmd.Parameters.AddWithValue(":firstname", obj.Firstname);
                cmd.Parameters.AddWithValue(":lastname", obj.Lastname);
                cmd.Parameters.AddWithValue(":nickname", obj.Nickname);
                cmd.Parameters.AddWithValue(":email", obj.Email);

                if (!string.IsNullOrEmpty(obj.PasswordHash))
                    cmd.Parameters.AddWithValue(":password_hash", obj.PasswordHash);

                cmd.Parameters.AddWithValue(":role", (int)obj.Role);
                cmd.Parameters.AddWithValue(":program_id", obj.ProgramID.HasValue ? obj.ProgramID : DBNull.Value);
                cmd.Parameters.AddWithValue(":enrolled_in_term_id", obj.EnrolledInTermID.HasValue ? obj.EnrolledInTermID : DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public void Save(User obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Delete(int id)
        {
            string sql = "delete from users where id=:id";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public User FindByNickname(string nickname)
        {
            string sql = "SELECT id,firstname,lastname,nickname," + 
                "email,password_hash,role,program_id,enrolled_in_term_id FROM users where nickname=:nickname";

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":nickname", nickname);

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

        public IEnumerable<User> MembersOfTeam(int team_id)
        {
            var sql = "select id,firstname,lastname,nickname,email,password_hash,role,program_id,enrolled_in_term_id from team_members " +
                "inner join users on users.id=member_id where team_id=:team_id order by lastname,firstname,nickname";

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":team_id", team_id);
                var result = new List<User>();

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
        }

        public IEnumerable<User> NotMembersOfTeam(int team_id)
        {
            var sql = "select id,firstname,lastname,nickname,email,password_hash,role,program_id,enrolled_in_term_id " +
                "from users where id not in (select member_id from team_members where team_id=:team_id)";

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":team_id", team_id);
                var result = new List<User>();

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
        }
    }
}