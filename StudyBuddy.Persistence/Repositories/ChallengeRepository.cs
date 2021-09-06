using Npgsql;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StudyBuddy.Persistence
{
    class ChallengeRepository : IChallengeRepository
    {
        private string connection_string;
        private QueryHelper<Challenge> qh;

        public ChallengeRepository(string connection_string)
        {
            this.connection_string = connection_string;
            this.qh = new QueryHelper<Challenge>(connection_string, FromReader);

            CreateTable();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CreateTable() 
        {
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

                qh.SetRevision("challenges", 2);
            }

            int revision = qh.GetRevision("challenges");
            if (revision == 1)
            {
                qh.ExecuteNonQuery(
                    "alter table challenges " +
                    "drop column if exists target_audience, " +
                    "add column valid_for_study_program_id int, " +
                    "add column valid_for_for_enrolled_since_term_id int");

                qh.SetRevision("challenges", 2);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
            obj.ValidForStudyProgramID = reader.IsDBNull(11) ? null : reader.GetInt32(11);
            obj.ValidForEnrolledSinceTermID = reader.IsDBNull(12) ? null : reader.GetInt32(12);
            return obj;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Challenge ById(int id)
        {
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id,valid_for_study_program_id,valid_for_for_enrolled_since_term_id " +
                "FROM challenges where id=:id");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Challenge> All(int from = 0, int max = 1000)
        {
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id,valid_for_study_program_id,valid_for_for_enrolled_since_term_id " +
                "FROM challenges order by created limit :max offset :from");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Challenge> ByText(string text, int from = 0, int max = 1000)
        {
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            qh.AddParameter(":text", "%" + text + "%");
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id,valid_for_study_program_id,valid_for_for_enrolled_since_term_id " +
                "FROM challenges where name like :text or description like :text " +
                "order by created limit :max offset :from");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Challenge> OfOwner(int owner_id, int from = 0, int max = 1000)
        {
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            qh.AddParameter(":owner_id", owner_id);
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id,valid_for_study_program_id,valid_for_for_enrolled_since_term_id " +
                "FROM challenges where owner_id=:owner_id order by created limit :max offset :from");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Challenge> OfOwnerByText(int owner_id, string text, int from = 0, int max = 1000)
        {
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            qh.AddParameter(":owner_id", owner_id);
            qh.AddParameter(":text", "%" + text + "%");
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,series_parent_id,valid_for_study_program_id,valid_for_for_enrolled_since_term_id " +
                "FROM challenges where owner_id=:owner_id and " +
                "(name like :text or description like :text) order by created limit :max offset :from");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Insert(Challenge obj)
        {
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
            qh.AddParameter(":valid_for_study_program_id", obj.ValidForStudyProgramID.HasValue ? obj.ValidForStudyProgramID : DBNull.Value);
            qh.AddParameter(":valid_for_for_enrolled_since_term_id", obj.ValidForEnrolledSinceTermID.HasValue ? obj.ValidForEnrolledSinceTermID : DBNull.Value);

            obj.ID = qh.ExecuteScalar(
                "insert into challenges (name,description,points," +
                "validity_start,validity_end,category,owner_id,created,prove," +
                "series_parent_id,valid_for_study_program_id,valid_for_for_enrolled_since_term_id) values " +
                "(:name,:description,:points,:validity_start,:validity_end,:category," +
                ":owner_id,:created,:prove,:series_parent_id,:valid_for_study_program_id,:valid_for_for_enrolled_since_term_id) RETURNING id");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update(Challenge obj)
        {
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
            qh.AddParameter(":valid_for_study_program_id", obj.ValidForStudyProgramID.HasValue ? obj.ValidForStudyProgramID : DBNull.Value);
            qh.AddParameter(":valid_for_for_enrolled_since_term_id", obj.ValidForEnrolledSinceTermID.HasValue ? obj.ValidForEnrolledSinceTermID : DBNull.Value);

            qh.ExecuteNonQuery("update challenges set name=:name,description=:description,points=:points," + 
                "validity_start=:validity_start,validity_end=:validity_end,category=:category," + 
                "owner_id=:owner_id,created=:created,prove=:prove,series_parent_id=:series_parent_id," +
                "valid_for_study_program_id=:valid_for_study_program_id," +
                "valid_for_for_enrolled_since_term_id=:valid_for_for_enrolled_since_term_id where id=:id");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Save(Challenge obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Delete(int id)
        {
            qh.Delete("challenges", "id", id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Challenge> OfBadge(int badge_id)
        {
            qh.AddParameter(":badge_id", badge_id);
            return qh.ExecuteQueryToObjectList(
                "select id,name,description,points,validity_start," +
                "validity_end,category,owner_id,created,prove,series_parent_id," +
                "valid_for_study_program_id,valid_for_for_enrolled_since_term_id " +
                "from game_badge_challenges " +
                "inner join challenges on challenge=id where game_badge=:badge_id " +
                "order by created desc");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Challenge> NotOfBadge(int badge_id)
        {
            qh.AddParameter(":badge_id", badge_id);
            return qh.ExecuteQueryToObjectList(
                "select id,name,description,points,validity_start," +
                "validity_end,category,owner_id,created,prove,series_parent_id " +
                "valid_for_study_program_id,valid_for_for_enrolled_since_term_id," +
                "from challenges where id not in " +
                "(select challenge from game_badge_challenges where game_badge=:badge_id) " +
                "order by created desc");
        }

        // ToDo: Eine der wichtigsten Methoden! Herausfinden, welche Challenge für den übergebenenen Benutzer sichtbar ist
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Challenge> VisibleForUser(int user_id)
        {
            return new List<Challenge>();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateSeries(int challenge_id, int days_add, int count)
        {
            var parent = ById(challenge_id);
            if (parent == null)
                return;

            for (int i=0; i<count; i++)
            {
                var clone = parent.Clone();
                clone.SeriesParentID = parent.ID;
                clone.ValidityStart = clone.ValidityStart.AddDays((i+1) * days_add);
                clone.ValidityEnd = clone.ValidityEnd.AddDays((i+1) * days_add);
                Insert(clone);
            }
        }
    }
}