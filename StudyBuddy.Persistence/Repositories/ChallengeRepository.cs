using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class ChallengeRepository : IChallengeRepository
    {
        private readonly string connection_string;

        public ChallengeRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateChallengesTable();
            CreateChallengeAcceptanceTable();
        }

        public Challenge ById(int id)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":id", id);

            return qh.ExecuteQueryToSingleObject("select id,name,description,points,validity_start,validity_end,category," +
                "owner_id,created,prove,series_parent_id,tags_of_challenge(id),prove_addendum from challenges where id=:id");
        }

        public IEnumerable<Challenge> All(ChallengeFilter filter)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);
            
            var sql = new StringBuilder("select id,name,description,points,validity_start,validity_end,category," +
                "owner_id,created,prove,series_parent_id,tags_of_challenge(id),prove_addendum from challenges where true ");

            ApplyFilter(qh, sql, filter);
            sql.Append(" order by validity_start,validity_end,created,name limit :max offset :from");
            return qh.ExecuteQueryToObjectList(sql.ToString());
        }

        public int GetCount(ChallengeFilter filter)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            var sql = new StringBuilder("select count(*) from challenges where true ");
            ApplyFilter(qh, sql, filter);
            return qh.ExecuteQueryToSingleInt(sql.ToString());
        }

        private void ApplyFilter(QueryHelper<Challenge> qh, StringBuilder sql, ChallengeFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql.Append(" and (name ilike :search_text or description ilike :search_text or tags_of_challenge(id) ilike :search_text)");
            }

            if (filter.OwnerId.HasValue)
            {
                qh.AddParameter(":owner_id", filter.OwnerId.Value);
                sql.Append(" and (owner_id=:owner_id)");
            }

            if (!filter.includeSystemProve)
            {
                sql.Append(" and (prove!=6)");
            }

            if (filter.ValidAt.HasValue)
            {
                qh.AddParameter(":valid_at", filter.ValidAt.Value);
                sql.Append(" and (validity_start<=:valid_at and validity_end>=:valid_at)");
            }

            if (filter.OnlyUnacceped)
            {
                qh.AddParameter(":user_id", filter.CurrentUserId);
                sql.Append(" and not challenge_accepted(:user_id, id)");
            }
        }

        public void Insert(Challenge obj)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":description", string.IsNullOrEmpty(obj.Description) ? DBNull.Value : obj.Description);
            qh.AddParameter(":points", obj.Points);
            qh.AddParameter(":validity_start", obj.ValidityStart);
            qh.AddParameter(":validity_end", obj.ValidityEnd);
            qh.AddParameter(":category", (int) obj.Category);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":prove", (int) obj.Prove);
            qh.AddParameter(":series_parent_id", obj.SeriesParentID.HasValue ? obj.SeriesParentID.Value : DBNull.Value);
            qh.AddParameter(":prove_addendum", obj.ProveAddendum);

            obj.ID = qh.ExecuteScalar(
                "insert into challenges (name,description,points," +
                "validity_start,validity_end,category,owner_id,created,prove,series_parent_id,prove_addendum) values " +
                "(:name,:description,:points,:validity_start,:validity_end,:category," +
                ":owner_id,:created,:prove,:series_parent_id,:prove_addendum) RETURNING id");
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
            qh.AddParameter(":category", (int) obj.Category);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":prove", (int) obj.Prove);
            qh.AddParameter(":series_parent_id", obj.SeriesParentID.HasValue ? obj.SeriesParentID.Value : DBNull.Value);
            qh.AddParameter(":prove_addendum", obj.ProveAddendum);

            qh.ExecuteNonQuery("update challenges set name=:name,description=:description,points=:points," +
                "validity_start=:validity_start,validity_end=:validity_end,category=:category," +
                "owner_id=:owner_id,created=:created,prove=:prove,series_parent_id=:series_parent_id," +
                "prove_addendum=:prove_addendum where id=:id");
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

        public IEnumerable<Challenge> GetChallengesOfBadge(int badge_id)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":badge_id", badge_id);
            return qh.ExecuteQueryToObjectList(
                "select distinct id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id,prove_addendum from challenges " +
                "inner join tags_challenges tc on id = tc.challenge_id " +
                "inner join tags_badges tb on tc.tag_id = tb.tag_id " +
                "where tb.badge_id=:badge_id order by created desc,name");
        }

        public void AddAcceptance(int challenge_id, int user_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new {challenge_id, user_id, created = DateTime.Now.Date});
            qh.ExecuteNonQuery(
                "insert into challenge_acceptance(challenge_id, user_id, created) " +
                "values(:challenge_id, :user_id, :created) ON CONFLICT DO NOTHING;");
        }

        public void RemoveAcceptance(int challenge_id, int user_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new {challenge_id, user_id});
            qh.ExecuteNonQuery(
                "delete from challenge_acceptance where challenge_id=:challenge_id and user_id=:user_id");
        }

        public IEnumerable<Challenge> Accepted(int user_id)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":user_id", user_id);
            var sql = "select id,name,description,points,validity_start,validity_end," +
                      "category,owner_id,ch.created,prove,series_parent_id,tags,prove_addendum from challenge_acceptance " +
                      "inner join challenges as ch on challenge_id=id where user_id=:user_id " +
                      "order by challenge_acceptance.created desc";

            return qh.ExecuteQueryToObjectList(sql);
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
                    "prove_addendum varchar(100))");

                rh.SetRevision(4);
            }

            var revision = rh.GetRevision();
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

            if (revision == 3)
            {
                qh.ExecuteNonQuery(
                    "alter table challenges " +
                    "add column prove_addendum varchar(100)");

                rh.SetRevision(4);
            }
        }

        private void CreateChallengeAcceptanceTable()
        {
            var rh = new RevisionHelper(connection_string, "challenge_acceptance");
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);

            if (!qh.TableExists("challenge_acceptance"))
                qh.ExecuteNonQuery(
                    "create table challenge_acceptance (" +
                    "user_id int not null," +
                    "challenge_id int not null, " +
                    "created date not null," +
                    "unique (user_id, challenge_id))");
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
            obj.Category = (ChallengeCategory) reader.GetInt32(6);
            obj.OwnerID = reader.GetInt32(7);
            obj.Created = reader.GetDateTime(8);
            obj.Prove = (ChallengeProve) reader.GetInt32(9);
            obj.SeriesParentID = reader.IsDBNull(10) ? null : reader.GetInt32(10);
            obj.Tags = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);
            obj.ProveAddendum = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
            return obj;
        }
    }
}