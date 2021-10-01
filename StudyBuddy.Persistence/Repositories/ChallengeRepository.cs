using Npgsql;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;

namespace StudyBuddy.Persistence
{
    class ChallengeRepository : IChallengeRepository
    {
        private string connection_string;

        public ChallengeRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateChallengesTable();
            CreateChallengeAcceptanceTable();
        }

        private void CreateChallengesTable() 
        {
            var rh = new RevisionHelper(connection_string, "challenges");
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);

            if (!qh.TableExists("challenges"))
            {
                qh.ExecuteNonQuery(
                    "create table challenges (" +
                    "id serial primary key, " +
                    "name varchar(100) not null, " +
                    "description text, " +
                    "points smallint not null, " +
                    "validity_start date not null, " +
                    "validity_end date not null, " +
                    "category smallint not null, " +
                    "owner_id int not null, " +
                    "created date not null, " +
                    "prove smallint not null, " +
                    "series_parent_id int, " +
                    "valid_for_study_program_id int, " +
                    "valid_for_for_enrolled_since_term_id int)");

                rh.SetRevision(2);
            }

            int revision = rh.GetRevision();
            if (revision == 1)
            {
                qh.ExecuteNonQuery(
                    "alter table challenges " +
                    "drop column if exists target_audience, " +
                    "add column valid_for_study_program_id int, " +
                    "add column valid_for_for_enrolled_since_term_id int");

                rh.SetRevision(2);
            }

            if (revision == 2)
            {
                qh.ExecuteNonQuery("alter table challenges " +
                    "drop column if exists valid_for_study_program_id," +
                    "drop column if exists valid_for_for_enrolled_since_term_id");

                rh.SetRevision(3);
            }
        }

        private void CreateChallengeAcceptanceTable()
        {
            var rh = new RevisionHelper(connection_string, "challenge_acceptance");
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);

            if (!qh.TableExists("challenge_acceptance"))
            {
                qh.ExecuteNonQuery(
                    "create table challenge_acceptance (" +
                    "user_id int not null," +
                    "challenge_id int not null, " +
                    "created date not null," +
                    "unique (user_id, challenge_id))");
            }
        }

        private Challenge FromReader(NpgsqlDataReader reader)
        {
            var obj = new Challenge();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            obj.Description = reader.IsDBNull(2) ? null : reader.GetString(2);
            obj.Points = reader.GetInt32(3);
            obj.ValidityStart = reader.GetDateTime(4);
            obj.ValidityEnd = reader.GetDateTime(5);
            obj.Category = (ChallengeCategory)reader.GetInt32(6);
            obj.OwnerID = reader.GetInt32(7);
            obj.Created = reader.GetDateTime(8);
            obj.Prove = (ChallengeProve)reader.GetInt32(9);
            obj.SeriesParentID = reader.IsDBNull(10) ? null : reader.GetInt32(10);
            return obj;
        }

        public Challenge ById(int id)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader, new { id });
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id " +
                "FROM challenges where id=:id");
        }

        public IEnumerable<Challenge> All(int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader, new { from, max });
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id " +
                "FROM challenges order by created limit :max offset :from");
        }

        public IEnumerable<Challenge> ForToday(DateTime today, int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader, new { today, from, max });
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,challenges.created,prove,series_parent_id " +
                "FROM challenges " +
                "left outer join challenge_acceptance on id=challenge_id " +
                "where challenge_id is null and :today>=validity_start and :today<=validity_end " +
                "order by challenges.created limit :max offset :from");
        }

        public IEnumerable<Challenge> ByText(string text, int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            qh.AddParameter(":text", "%" + text + "%");
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id " +
                "FROM challenges where name like :text or description like :text " +
                "order by created limit :max offset :from");
        }

        public IEnumerable<Challenge> OfOwner(int owner_id, int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            qh.AddParameter(":owner_id", owner_id);
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id " +
                "FROM challenges where owner_id=:owner_id order by created limit :max offset :from");
        }

        public IEnumerable<Challenge> OfOwnerByText(int owner_id, string text, int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            qh.AddParameter(":owner_id", owner_id);
            qh.AddParameter(":text", "%" + text + "%");
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id " +
                "FROM challenges where owner_id=:owner_id and " +
                "(name like :text or description like :text) order by created limit :max offset :from");
        }

        public void Insert(Challenge obj)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":description", string.IsNullOrEmpty(obj.Description) ? DBNull.Value : obj.Description);
            qh.AddParameter(":points", obj.Points);
            qh.AddParameter(":validity_start", obj.ValidityStart);
            qh.AddParameter(":validity_end", obj.ValidityEnd);
            qh.AddParameter(":category", (int)obj.Category);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":prove", (int)obj.Prove);
            qh.AddParameter(":series_parent_id", obj.SeriesParentID.HasValue ? obj.SeriesParentID.Value : DBNull.Value);

            obj.ID = qh.ExecuteScalar(
                "insert into challenges (name,description,points," +
                "validity_start,validity_end,category,owner_id,created,prove,series_parent_id) values " +
                "(:name,:description,:points,:validity_start,:validity_end,:category," +
                ":owner_id,:created,:prove,:series_parent_id) RETURNING id");
        }

        public void Update(Challenge obj)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":description", string.IsNullOrEmpty(obj.Description) ? DBNull.Value : obj.Description);
            qh.AddParameter(":points", obj.Points);
            qh.AddParameter(":validity_start", obj.ValidityStart);
            qh.AddParameter(":validity_end", obj.ValidityEnd);
            qh.AddParameter(":category", (int)obj.Category);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":prove", (int)obj.Prove);
            qh.AddParameter(":series_parent_id", obj.SeriesParentID.HasValue ? obj.SeriesParentID.Value : DBNull.Value);

            qh.ExecuteNonQuery("update challenges set name=:name,description=:description,points=:points," + 
                "validity_start=:validity_start,validity_end=:validity_end,category=:category," + 
                "owner_id=:owner_id,created=:created,prove=:prove,series_parent_id=:series_parent_id where id=:id");
        }

        public void Save(Challenge obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.Delete("challenges", "id", id);
        }

        public IEnumerable<Challenge> OfBadge(int badge_id)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":badge_id", badge_id);
            return qh.ExecuteQueryToObjectList(
                "select id,name,description,points,validity_start," +
                "validity_end,category,owner_id,created,prove,series_parent_id," +
                "from game_badge_challenges " +
                "inner join challenges on challenge=id where game_badge=:badge_id " +
                "order by created desc");
        }

        public IEnumerable<Challenge> NotOfBadge(int badge_id)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":badge_id", badge_id);
            return qh.ExecuteQueryToObjectList(
                "select id,name,description,points,validity_start," +
                "validity_end,category,owner_id,created,prove,series_parent_id " +
                "from challenges where id not in " +
                "(select challenge from game_badge_challenges where game_badge=:badge_id) " +
                "order by created desc");
        }

        public void AddAcceptance(int challenge_id, int user_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new { challenge_id, user_id, created=DateTime.Now.Date });
            qh.ExecuteNonQuery(
                "insert into challenge_acceptance(challenge_id, user_id, created) " +
                "values(:challenge_id, :user_id, :created) ON CONFLICT DO NOTHING;");
        }

        public void DeleteAcceptance(int challenge_id, int user_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new { challenge_id, user_id });
            qh.ExecuteNonQuery(
                "delete from challenge_acceptance where challenge_id=:challenge_id and user_id=:user_id");
        }

        public IEnumerable<Challenge> Accepted(int user_id)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":user_id", user_id);
            string sql = "select id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id from challenge_acceptance " +
                "inner join challenges on challenge_id=id where user_id=:user_id " +
                "order by challenge_acceptance.created desc";
            return qh.ExecuteQueryToObjectList(sql);
        }
    }
}