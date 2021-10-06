using System;
using System.Collections.Generic;
using Npgsql;
using SimpleHashing.Net;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class UserRepository : IUserRepository
    {
        private readonly string connection_string;
        private readonly SimpleHash simpleHash = new();

        public UserRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        private void CreateTable()
        {
            var rh = new RevisionHelper(connection_string, "users");
            var qh = new QueryHelper<User>(connection_string);

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
                    "role int not null)");

                Insert(new User
                {
                    Firstname = "Empty",
                    Lastname = "Empty",
                    Nickname = "Admin",
                    Email = "admin@admin.de",
                    Password = "secret",
                    Role = Role.Admin
                });

                rh.SetRevision(2);
            }

            if (!qh.TableExists("friends"))
                qh.ExecuteNonQuery(
                    "create table friends (" +
                    "user_a int not null, " +
                    "user_b int not null)");

            if (qh.TableExists("teams"))
                qh.ExecuteNonQuery("drop table teams");

            if (qh.TableExists("team_members"))
                qh.ExecuteNonQuery("drop table team_members");

            if (rh.GetRevision() == 1)
            {
                qh.ExecuteNonQuery("alter table users " +
                    "drop column if exists program_id," +
                    "drop column if exists enrolled_in_term_id");

                rh.SetRevision(2);
            }
        }

        public User ById(int id)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader, new {id});
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,firstname,lastname,nickname," +
                "email,password_hash,role FROM users where id=:id");
        }

        public IEnumerable<User> All(int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader, new {from, max});
            return qh.ExecuteQueryToObjectList(
                "SELECT id,firstname,lastname,nickname,email,password_hash,role " +
                "FROM users order by lastname,firstname,nickname limit :max offset :from");
        }

        public int Count()
        {
            var qh = new QueryHelper<User>(connection_string);
            return qh.GetCount("users");
        }

        public User ByEmailAndPassword(string email, string password)
        {
            var user = ByEmail(email);
            if (user != null && simpleHash.Verify(password, user.PasswordHash))
                return user;
            return null;
        }

        public User ByEmail(string email)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader, new {email});
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,firstname,lastname,nickname,email,password_hash,role " +
                "FROM users where lower(email)=lower(:email)");
        }

        public void Insert(User obj)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader);
            qh.AddParameter(":firstname", obj.Firstname);
            qh.AddParameter(":lastname", obj.Lastname);
            qh.AddParameter(":nickname", obj.Nickname.ToLower());
            qh.AddParameter(":email", obj.Email.ToLower());
            qh.AddParameter(":password_hash", simpleHash.Compute(obj.Password));
            qh.AddParameter(":role", (int) obj.Role);

            obj.ID = qh.ExecuteScalar(
                "insert into users " +
                "(firstname,lastname,nickname,email,password_hash,role) values " +
                "(:firstname,:lastname,:nickname,:email,:password_hash,:role) RETURNING id");
        }

        public void Update(User obj)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader);

            var sql = "update users set firstname=:firstname,lastname=:lastname," +
                      "nickname=:nickname,email=:email";

            if (!string.IsNullOrEmpty(obj.Password))
                sql += ",password_hash=:password_hash";

            sql += ",role=:role where id=:id";

            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":firstname", obj.Firstname);
            qh.AddParameter(":lastname", obj.Lastname);
            qh.AddParameter(":nickname", obj.Nickname.ToLower());
            qh.AddParameter(":email", obj.Email.ToLower());

            if (!string.IsNullOrEmpty(obj.Password))
                qh.AddParameter(":password_hash", simpleHash.Compute(obj.Password));

            qh.AddParameter(":role", (int) obj.Role);
            qh.ExecuteNonQuery(sql);
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
            var qh = new QueryHelper<User>(connection_string);
            qh.Delete("users", "id", id);
            qh.Delete("challenge_acceptance", "user_id", id);
        }

        public User ByNickname(string nickname)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader);
            qh.AddParameter(":nickname", nickname);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,firstname,lastname,nickname," +
                "email,password_hash,role FROM " +
                "users where lower(nickname)=lower(:nickname)");
        }

        public IEnumerable<User> GetFriends(int user_id, int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader, new {user_id, from, max});
            return qh.ExecuteQueryToObjectList(
                "select id,firstname,lastname,nickname,email,password_hash,role from friends " +
                "inner join users on user_b = id where user_a=:user_id " +
                "order by lastname,firstname,nickname limit :max offset :from");
        }

        public void AddFriend(int user_id, int friend_id)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader, new {user_id, friend_id});
            qh.ExecuteNonQuery(
                "insert into friends (user_a, user_b) values(:user_id, :friend_id), (:friend_id, :user_id);");
        }

        public void RemoveFriend(int user_id, int friend_id)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader, new {user_id, friend_id});
            qh.ExecuteNonQuery(
                "delete from friends where " +
                "(user_a=:user_id and user_b=:friend_id) or " +
                "(user_a=:friend_id and user_b=:user_id);");
        }

        public void RemoveFriends(int user_id)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader, new {user_id});
            qh.ExecuteNonQuery("delete from friends where user_a=:user_id or user_b=:user_id;");
        }

        public void AddFriends(int user_id, int[] friend_ids)
        {
            foreach (var friend_id in friend_ids)
                AddFriend(user_id, friend_id);
        }

        public IEnumerable<User> GetAllUsersThatAcceptedChallenge(int challenge_id)
        {
            var qh = new QueryHelper<User>(connection_string, FromReader);
            qh.AddParameter(":challenge_id", challenge_id);
            return qh.ExecuteQueryToObjectList(
                "SELECT id,firstname,lastname,nickname,email,password_hash,role FROM challenge_acceptance " +
                "inner join users on user_id=id " +
                "where challenge_id=:challenge_id " +
                "order by lastname,firstname,nickname");
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
            obj.Role = (Role) reader.GetInt32(6);
            return obj;
        }

        public int GetCountOfCommonFriends(int user_a_id, int user_b_id)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameters(new { user_a_id, user_b_id });
            return qh.ExecuteQueryToSingleInt(
                "select count(*) as count_of_common_friends from " +
                "(select user_b from friends where user_a=:user_a_id intersect " +
                "select user_b from friends where user_a=:user_b_id) as common_friends");
        }
    }
}