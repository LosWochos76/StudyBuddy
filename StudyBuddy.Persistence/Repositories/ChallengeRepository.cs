using System;
using System.Collections.Generic;
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

            var sql = "SELECT challenges.id, challenges.name, challenges.description, challenges.points, " +
                "challenges.validity_start, challenges.validity_end, challenges.category, challenges.owner_id, " +
                "challenges.created, challenges.prove, challenges.series_parent_id, " +
                "String_agg('#' || tags.name, ' ') as tags_list " + 
                "FROM challenges " +
                "LEFT OUTER JOIN tags_challenges ON challenges.id = tags_challenges.challenge_id " +
                "LEFT OUTER JOIN tags ON tags_challenges.tag_id = tags.id " +
                "where challenges.id=:id " +
                "group by challenges.id, challenges.name, challenges.description, challenges.points, " +
                "challenges.validity_start, challenges.validity_end, challenges.category, challenges.owner_id, " +
                "challenges.created, challenges.prove, challenges.series_parent_id";

            return qh.ExecuteQueryToSingleObject(sql);
        }

        public IEnumerable<Challenge> All(ChallengeFilter filter)
        {
            var qh = new QueryHelper<Challenge>(connection_string, FromReader);
            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":user_id", filter.CurrentUserId);

            var sql = "select * from (SELECT challenges.id, challenges.name, challenges.description, challenges.points, " +
                "challenges.validity_start, challenges.validity_end, challenges.category, challenges.owner_id, " +
                "challenges.created, challenges.prove, challenges.series_parent_id, " +
                "String_agg('#' || tags.name, ' ') as tags_list, " +
                "CASE WHEN challenges.id IN (" +
		        "SELECT challenge_acceptance.challenge_id FROM challenge_acceptance WHERE challenge_acceptance.user_id = :user_id" +
	            ") THEN 1 ELSE 0 END challenge_finished " +
                "FROM challenges " +
                "LEFT OUTER JOIN tags_challenges ON challenges.id = tags_challenges.challenge_id " +
                "LEFT OUTER JOIN tags ON tags_challenges.tag_id = tags.id " +
                "group by challenges.id, challenges.name, challenges.description, challenges.points, " +
                "challenges.validity_start, challenges.validity_end, challenges.category, challenges.owner_id, " +
                "challenges.created, challenges.prove, challenges.series_parent_id) b where true ";

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql += " and (name ilike :search_text or description ilike :search_text or tags_list ilike :search_text)";
            }

            if (filter.OwnerId.HasValue)
            {
                qh.AddParameter(":owner_id", filter.OwnerId.Value);
                sql += " and (owner_id=:owner_id)";
            }

            if (filter.ValidAt.HasValue)
            {
                qh.AddParameter(":valid_at", filter.ValidAt.Value);
                sql += " and (validity_start<=:valid_at and validity_end>=:valid_at)";
            }

            if (filter.OnlyUnacceped)
            {
                sql += " and (challenge_finished = 0)";
            }
            
            sql += " order by validity_start,validity_end,created limit :max offset :from";
            return qh.ExecuteQueryToObjectList(sql);
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
            qh.AddParameter(":category", (int) obj.Category);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":prove", (int) obj.Prove);
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
                "select distinct id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id from challenges " +
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
                      "category,owner_id,created,prove,series_parent_id from challenge_acceptance " +
                      "inner join challenges on challenge_id=id where user_id=:user_id " +
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
                    "valid_for_study_program_id int, " +
                    "valid_for_for_enrolled_since_term_id int)");

                rh.SetRevision(2);
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
            return obj;
        }
    }
}