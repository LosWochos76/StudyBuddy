using System.Collections.Generic;
using SimpleHashing.Net;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class UserRepository : IUserRepository
    {
        private readonly string connection_string;
        private readonly SimpleHash simpleHash = new SimpleHash();
        private UserConverter converter = new UserConverter();

        public UserRepository(string connection_string)
        {
            this.connection_string = connection_string;
            CreateTable();
        }

        public User ById(int id)
        {
            var qh = new QueryHelper<User>(connection_string, new { id });
            var set = qh.ExecuteQueryToDataSet(
                "SELECT id,firstname,lastname,nickname," +
                "email,password_hash,role,emailconfirmed,accountactive FROM users where id=:id");

            return converter.Single(set);
        }

        public IEnumerable<User> All(UserFilter filter)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            var set = qh.ExecuteQueryToDataSet(
                "SELECT id,firstname,lastname,nickname,email,password_hash,role,emailconfirmed,accountactive " +
                "FROM users order by lastname,firstname,nickname limit :max offset :from");

            return converter.Multiple(set);
        }

        public int GetCount(UserFilter filter)
        {
            var qh = new QueryHelper<User>(connection_string);
            return qh.GetCount("users");
        }

        public User ByEmailActiveAccounts(string email)
        {
            var qh = new QueryHelper<User>(connection_string, new {email});
            var set = qh.ExecuteQueryToDataSet(
                "SELECT id,firstname,lastname,nickname,email,password_hash,role,emailconfirmed,accountactive " +
                "FROM users where accountactive = true and lower(email)=lower(:email)");

            return converter.Single(set);
        }

        public User ByEmailAllAccounts(string email)
        {
            var qh = new QueryHelper<User>(connection_string, new { email });
            var set = qh.ExecuteQueryToDataSet(
                "SELECT id,firstname,lastname,nickname,email,password_hash,role,emailconfirmed,accountactive " +
                "FROM users where lower(email)=lower(:email)");

            return converter.Single(set);
        }

        public void Insert(User obj)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":firstname", obj.Firstname);
            qh.AddParameter(":lastname", obj.Lastname);
            qh.AddParameter(":nickname", obj.Nickname.ToLower());
            qh.AddParameter(":email", obj.Email.ToLower());
            qh.AddParameter(":password_hash", simpleHash.Compute(obj.Password));
            qh.AddParameter(":role", (int) obj.Role);
            qh.AddParameter(":emailconfirmed", false);
            qh.AddParameter(":accountactive", true);

            obj.ID = qh.ExecuteScalar(
                "insert into users " +
                "(firstname,lastname,nickname,email,password_hash,role,emailconfirmed,accountactive) values " +
                "(:firstname,:lastname,:nickname,:email,:password_hash,:role,:emailconfirmed,:accountactive) RETURNING id");
        }

        public void Update(User obj)
        {
            var qh = new QueryHelper<User>(connection_string);

            var sql = "update users set firstname=:firstname,lastname=:lastname," +
                      "nickname=:nickname,email=:email";

            if (!string.IsNullOrEmpty(obj.Password))
                sql += ",password_hash=:password_hash";

            sql += ",role=:role,emailconfirmed=:emailconfirmed,accountactive=:accountactive where id=:id";

            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":firstname", obj.Firstname);
            qh.AddParameter(":lastname", obj.Lastname);
            qh.AddParameter(":nickname", obj.Nickname.ToLower());
            qh.AddParameter(":email", obj.Email.ToLower());
            qh.AddParameter(":emailconfirmed", obj.EmailConfirmed);
            qh.AddParameter(":accountactive", obj.AccountActive);

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
        }

        public User ByNickname(string nickname)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":nickname", nickname);

            var set = qh.ExecuteQueryToDataSet(
                "SELECT id,firstname,lastname,nickname," +
                "email,password_hash,role,emailconfirmed,accountactive FROM " +
                "users where lower(nickname)=lower(:nickname)");

            return converter.Single(set);
        }

        public IEnumerable<User> GetFriends(FriendFilter filter)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":user_id", filter.UserId);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            var sql = "select id,firstname,lastname,nickname,email,password_hash,role,emailconfirmed,accountactive,common_friends(id, :user_id) from friends" +
                " inner join users on user_b = id where accountactive = true and user_a=:user_id ";

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql +=
                    " and (firstname ilike :search_text or lastname ilike :search_text or email ilike :search_text) ";
            }

            sql += "order by lastname,firstname,nickname limit :max offset :from";

            var set = qh.ExecuteQueryToDataSet(sql);
            return converter.Multiple(set);
        }

        public int GetFriendsCount(FriendFilter filter)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":user_id", filter.UserId);
            var sql = "select count(*) from friends inner join users on user_b=id where accountactive = true and user_a=:user_id";

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql +=
                    " and (firstname ilike :search_text or lastname ilike :search_text or email ilike :search_text) ";
            }

            return qh.ExecuteQueryToSingleInt(sql);
        }

        public IEnumerable<User> GetNotFriends(FriendFilter filter)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":user_id", filter.UserId);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            var sql =
                "select id,firstname,lastname,nickname,email,password_hash,role,emailconfirmed,accountactive,common_friends(id,:user_id) from users " +
                "where accountactive = true and " +
                "id not in (select user_b from friends where user_a=:user_id) and id!=:user_id ";

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql += " and (firstname ilike :search_text or lastname ilike :search_text or email ilike :search_text)";
            }

            sql += " order by lastname,firstname,nickname limit :max offset :from";

            var set = qh.ExecuteQueryToDataSet(sql);
            return converter.Multiple(set);
        }

        public int GetNotFriendsCount(FriendFilter filter)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":user_id", filter.UserId);

            var sql = "select count(*) from users where accountactive = true and id not in (select user_b from friends where user_a=:user_id) and id!=:user_id ";

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql += " and (firstname ilike :search_text or lastname ilike :search_text or email ilike :search_text)";
            }

            return qh.ExecuteQueryToSingleInt(sql);
        }

        public void AddFriend(int user_id, int friend_id)
        {
            var qh = new QueryHelper<User>(connection_string, new {user_id, friend_id});
            qh.ExecuteNonQuery(
                "insert into friends (user_a, user_b) values (:user_id, :friend_id), (:friend_id, :user_id);");
        }

        public void RemoveFriend(int user_id, int friend_id)
        {
            var qh = new QueryHelper<User>(connection_string, new {user_id, friend_id});
            qh.ExecuteNonQuery(
                "delete from friends where " +
                "(user_a=:user_id and user_b=:friend_id) or " +
                "(user_a=:friend_id and user_b=:user_id);");
        }

        public void RemoveFriends(int user_id)
        {
            var qh = new QueryHelper<User>(connection_string, new {user_id});
            qh.ExecuteNonQuery("delete from friends where user_a=:user_id or user_b=:user_id;");
        }

        public void AddFriends(int user_id, int[] friend_ids)
        {
            foreach (var friend_id in friend_ids)
                AddFriend(user_id, friend_id);
        }

        public IEnumerable<User> GetAllUsersThatAcceptedChallenge(int challenge_id)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":challenge_id", challenge_id);
            var sql = "SELECT id,firstname,lastname,nickname,email,password_hash,role,emailconfirmed,accountactive FROM challenge_acceptance " +
                "inner join users on user_id=id " +
                "where challenge_id=:challenge_id " +
                "order by lastname,firstname,nickname";

            var set = qh.ExecuteQueryToDataSet(sql);
            return converter.Multiple(set);
        }

        public IEnumerable<User> GetAllUsersHavingBadge(int badge_id)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":badge_id", badge_id);
            var sql = "SELECT id,firstname,lastname,nickname,email,password_hash,role,emailconfirmed,accountactive FROM users_badges " +
                "inner join users on user_id=id " +
                "where badge_id=:badge_id " +
                "order by lastname,firstname,nickname";

            var set = qh.ExecuteQueryToDataSet(sql);
            return converter.Multiple(set);
        }

        public int GetCountOfCommonFriends(int user_a_id, int user_b_id)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameters(new {user_a_id, user_b_id});
            return qh.ExecuteQueryToSingleInt("select common_friends(:user_a_i, :user_b_id)");
        }

        public IEnumerable<User> GetAllLikersForNotification(int notificationId)
        {
            var qh = new QueryHelper<User>(connection_string);
            qh.AddParameter(":notification_id", notificationId);

            var sql = "select u.id,u.firstname,u.lastname,u.nickname,u.email,u.password_hash,u.role,u.emailconfirmed,u.accountactive " +
                "from users as u  " +
                "inner join notification_user_metadata as md on u.id = md.owner_id and liked = true and md.notification_id = :notification_id ";

            var set = qh.ExecuteQueryToDataSet(sql);
            return converter.Multiple(set);
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
                    Role = Role.Admin,
                    EmailConfirmed = true,
                    AccountActive = true
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
            
            if (rh.GetRevision() == 2)
            {
                qh.ExecuteNonQuery(
                    "ALTER TABLE users ADD COLUMN emailconfirmed BOOLEAN DEFAULT false");

                rh.SetRevision(3);
            }
            if (rh.GetRevision() == 3)
            {
                qh.ExecuteNonQuery(
                    "ALTER TABLE users ADD COLUMN accountactive BOOLEAN DEFAULT true");
                rh.SetRevision(4);
            }

            qh.ExecuteNonQuery("begin;\n" +
                               "SELECT pg_advisory_xact_lock(2142616474639426746);\n" +
                               "create or replace function common_friends(user_a_param int, user_b_param int)\n" +
                               "returns int\n" +
                               "language plpgsql\n" +
                               "as $$\n" +
                               "declare\n" +
                               "friend_count int;\n" +
                               "begin\n" +
                               "   if (user_a_param = user_b_param) then\n" +
                               "       return 0;\n" +
                               "   end if;\n\n" +
                               "select count(*) into friend_count from\n" +
                               "(select user_b from friends where user_a = user_a_param intersect\n" +
                               "select user_b from friends where user_a = user_b_param) as common_friends;\n" +
                               "return friend_count;\n" +
                               "end;$$;\n" +
                               "commit;");

            qh.ExecuteNonQuery("begin;\n" +
                               "SELECT pg_advisory_xact_lock(5667323614921139925);\n" +
                               "create or replace function challenge_accepted(user_id_param int, challenge_id_param int)\n" +
                               "returns boolean\n" +
                               "language plpgsql\n" +
                               "as $$\n" +
                               "declare\n" +
                               "has_accepted boolean;\n" +
                               "begin\n" +
                               "select case When(select count(*) as count from challenge_acceptance\n" +
                               "where user_id = user_id_param and challenge_id = challenge_id_param) > 0 Then True else False end into has_accepted;\n" +
                               "return has_accepted;\n" +
                               "end;$$;\n" +
                               "commit;");

            qh.ExecuteNonQuery("begin;\n" +
                               "SELECT pg_advisory_xact_lock(8446020628481340546);\n" +
                               "create or replace function badge_received(user_id_param int, badge_id_param int)\n" +
                               "returns boolean\n" +
                               "language plpgsql\n" +
                               "as $$\n" +
                               "declare\n" +
                               "has_received boolean;\n" +
                               "begin\n" +
                               "select case When(select count(*) as count from users_badges\n" +
                               "where user_id = user_id_param and badge_id = badge_id_param) > 0 Then True else False end into has_received;\n" +
                               "return has_received;\n" +
                               "end;$$;\n" +
                               "commit;");
        }
    }
}