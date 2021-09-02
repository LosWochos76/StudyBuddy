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

        public UserRepository(string connection_string) : base(connection_string)
        {
            if (!TableExists("users")) 
            {
                CreateTable();

                Insert(new User() { 
                    Firstname="Empty", 
                    Lastname="Empty", 
                    Nickname="Admin",
                    Email="admin@admin.de",
                    Password= "secret",
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

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
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

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
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
            }

            return null;
        }

        public IEnumerable<User> All(int from = 0, int max = 1000)
        {
            string sql = "SELECT id,firstname,lastname,nickname,email,password_hash,role," +
                "program_id,enrolled_in_term_id FROM users order by lastname,firstname,nickname limit :max offset :from";

            var result = new List<User>();

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":from", from);
                    cmd.Parameters.AddWithValue(":max", max);
                    
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var obj = FromReader(reader);
                            result.Add(obj);
                        }
                    }
                }
            }

            return result;
        }

        public int Count()
        {
            string sql = "SELECT count(*) as count FROM users";
            int result = 0;

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            result = reader.GetInt32(0);
                    }
                }
            }

            return result;
        }

        public User ByEmailAndPassword(UserCredentials credentials)
        {
            var user = ByEmail(credentials.EMail);
            if (user != null && simpleHash.Verify(credentials.Password, user.PasswordHash))
                return user;
            else
                return null;
        }

        public User ByEmail(string email)
        {
            email = email.ToLower();
            string sql = "SELECT id,firstname,lastname,nickname," + 
                "email,password_hash,role,program_id,enrolled_in_term_id FROM users where lower(email)=lower(:email)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
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
            }

            return null;
        }

        public void Insert(User obj)
        {
            string sql = "insert into users " +
                    "(firstname,lastname,nickname,email,password_hash,role,program_id,enrolled_in_term_id) values " +
                    "(:firstname,:lastname,:nickname,:email,:password_hash,:role,:program_id,:enrolled_in_term_id) RETURNING id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":firstname", obj.Firstname);
                    cmd.Parameters.AddWithValue(":lastname", obj.Lastname);
                    cmd.Parameters.AddWithValue(":nickname", obj.Nickname.ToLower());
                    cmd.Parameters.AddWithValue(":email", obj.Email.ToLower());
                    cmd.Parameters.AddWithValue(":password_hash", simpleHash.Compute(obj.Password));
                    cmd.Parameters.AddWithValue(":role", (int)obj.Role);
                    cmd.Parameters.AddWithValue(":program_id", obj.ProgramID.HasValue ? obj.ProgramID : DBNull.Value);
                    cmd.Parameters.AddWithValue(":enrolled_in_term_id", obj.EnrolledInTermID.HasValue ? obj.EnrolledInTermID : DBNull.Value);
                    obj.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(User obj)
        {
            string sql = "update users set firstname=:firstname,lastname=:lastname,"+
                "nickname=:nickname,email=:email";
                
            if (!string.IsNullOrEmpty(obj.Password))
                sql += ",password_hash=:password_hash";
            
            sql += ",role=:role,program_id=:program_id,enrolled_in_term_id=:enrolled_in_term_id where id=:id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":id", obj.ID);
                    cmd.Parameters.AddWithValue(":firstname", obj.Firstname);
                    cmd.Parameters.AddWithValue(":lastname", obj.Lastname);
                    cmd.Parameters.AddWithValue(":nickname", obj.Nickname.ToLower());
                    cmd.Parameters.AddWithValue(":email", obj.Email.ToLower());

                    if (!string.IsNullOrEmpty(obj.Password))
                        cmd.Parameters.AddWithValue(":password_hash", simpleHash.Compute(obj.Password));

                    cmd.Parameters.AddWithValue(":role", (int)obj.Role);
                    cmd.Parameters.AddWithValue(":program_id", obj.ProgramID.HasValue ? obj.ProgramID : DBNull.Value);
                    cmd.Parameters.AddWithValue(":enrolled_in_term_id", obj.EnrolledInTermID.HasValue ? obj.EnrolledInTermID : DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
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

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public User ByNickname(string nickname)
        {
            nickname = nickname.ToLower();
            string sql = "SELECT id,firstname,lastname,nickname," + 
                "email,password_hash,role,program_id,enrolled_in_term_id FROM users where lower(nickname)=lower(:nickname)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
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
            }

            return null;
        }

        public IEnumerable<User> MembersOfTeam(int team_id)
        {
            var sql = "select id,firstname,lastname,nickname,email,password_hash,role,program_id,enrolled_in_term_id from team_members " +
                "inner join users on users.id=member_id where team_id=:team_id order by lastname,firstname,nickname";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
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

        public IEnumerable<User> NotMembersOfTeam(int team_id)
        {
            var sql = "select id,firstname,lastname,nickname,email,password_hash,role,program_id,enrolled_in_term_id " +
                "from users where id not in (select member_id from team_members where team_id=:team_id)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
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
}