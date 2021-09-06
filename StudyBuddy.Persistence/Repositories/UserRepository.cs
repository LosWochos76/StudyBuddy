using Npgsql;
using SimpleHashing.Net;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StudyBuddy.Persistence
{
    class UserRepository : IUserRepository
    {
        private string connection_string;
        private QueryHelper<User> qh;
        private SimpleHash simpleHash = new SimpleHash();

        public UserRepository(string connection_string)
        {
            this.connection_string = connection_string;
            this.qh = new QueryHelper<User>(connection_string, FromReader);

            CreateTable();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CreateTable() 
        {
            if (!qh.TableExists("users"))
            {
                qh.ExecuteNonQuery(
                    "create table users (" +
                    "id serial primary key, " +
                    "firstname varchar(100) not null, " +
                    "lastname varchar(100) not null, " +
                    "nickname varchar(100) not null, " +
                    "email varchar(100) not null, " +
                    "password_hash varchar(100), " +
                    "role int not null, " +
                    "program_id int, " +
                    "enrolled_in_term_id int)");

                Insert(new User()
                {
                    Firstname = "Empty",
                    Lastname = "Empty",
                    Nickname = "Admin",
                    Email = "admin@admin.de",
                    Password = "secret",
                    Role = Role.Admin
                });
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User ById(int id)
        {
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,firstname,lastname,nickname," +
                "email,password_hash,role,program_id,enrolled_in_term_id FROM users where id=:id");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> All(int from = 0, int max = 1000)
        {
            qh.AddParameters(new { from, max });
            return qh.ExecuteQueryToObjectList(
                "SELECT id,firstname,lastname,nickname,email,password_hash,role," +
                "program_id,enrolled_in_term_id FROM users order by lastname,firstname,nickname limit :max offset :from");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Count()
        {
            return qh.GetCount("users");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User ByEmailAndPassword(string email, string password)
        {
            var user = ByEmail(email);
            if (user != null && simpleHash.Verify(password, user.PasswordHash))
                return user;
            else
                return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User ByEmail(string email)
        {
            qh.AddParameter(":email", email);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,firstname,lastname,nickname," +
                "email,password_hash,role,program_id,enrolled_in_term_id FROM users where lower(email)=lower(:email)");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Insert(User obj)
        {
            qh.AddParameter(":firstname", obj.Firstname);
            qh.AddParameter(":lastname", obj.Lastname);
            qh.AddParameter(":nickname", obj.Nickname.ToLower());
            qh.AddParameter(":email", obj.Email.ToLower());
            qh.AddParameter(":password_hash", simpleHash.Compute(obj.Password));
            qh.AddParameter(":role", (int)obj.Role);
            qh.AddParameter(":program_id", obj.ProgramID.HasValue ? obj.ProgramID : DBNull.Value);
            qh.AddParameter(":enrolled_in_term_id", obj.EnrolledInTermID.HasValue ? obj.EnrolledInTermID : DBNull.Value);

            obj.ID = qh.ExecuteScalar(
                "insert into users " +
                "(firstname,lastname,nickname,email,password_hash,role,program_id,enrolled_in_term_id) values " +
                "(:firstname,:lastname,:nickname,:email,:password_hash,:role,:program_id,:enrolled_in_term_id) RETURNING id");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update(User obj)
        {
            string sql = "update users set firstname=:firstname,lastname=:lastname,"+
                "nickname=:nickname,email=:email";
                
            if (!string.IsNullOrEmpty(obj.Password))
                sql += ",password_hash=:password_hash";
            
            sql += ",role=:role,program_id=:program_id,enrolled_in_term_id=:enrolled_in_term_id where id=:id";

            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":firstname", obj.Firstname);
            qh.AddParameter(":lastname", obj.Lastname);
            qh.AddParameter(":nickname", obj.Nickname.ToLower());
            qh.AddParameter(":email", obj.Email.ToLower());

            if (!string.IsNullOrEmpty(obj.Password))
                qh.AddParameter(":password_hash", simpleHash.Compute(obj.Password));

            qh.AddParameter(":role", (int)obj.Role);
            qh.AddParameter(":program_id", obj.ProgramID.HasValue ? obj.ProgramID : DBNull.Value);
            qh.AddParameter(":enrolled_in_term_id", obj.EnrolledInTermID.HasValue ? obj.EnrolledInTermID : DBNull.Value);

            qh.ExecuteNonQuery(sql);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Save(User obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Delete(int id)
        {
            qh.Delete("users", "id", id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User ByNickname(string nickname)
        {
            qh.AddParameter(":nickname", nickname);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,firstname,lastname,nickname," +
                "email,password_hash,role,program_id,enrolled_in_term_id FROM " +
                "users where lower(nickname)=lower(:nickname)");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> MembersOfTeam(int team_id)
        {
            qh.AddParameter(":team_id", team_id);
            return qh.ExecuteQueryToObjectList(
                "select id,firstname,lastname,nickname,email,password_hash,role,program_id,enrolled_in_term_id from team_members " +
                "inner join users on users.id=member_id where team_id=:team_id order by lastname,firstname,nickname");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> NotMembersOfTeam(int team_id)
        {
            qh.AddParameter(":team_id", team_id);
            return qh.ExecuteQueryToObjectList(
                "select id,firstname,lastname,nickname,email,password_hash,role,program_id,enrolled_in_term_id " +
                "from users where id not in (select member_id from team_members where team_id=:team_id)");
        }
    }
}