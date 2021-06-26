using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class StudyProgramRepository : SqlRepositoryBase, IStudyProgramRepository
    {
        public StudyProgramRepository(NpgsqlConnection connection) : base(connection)
        {
            if (!TableExists("study_programs")) 
                CreateTable();
        }

        private void CreateTable() 
        {
            string sql = "create table study_programs (" +
                "id serial primary key, " +
                "acronym varchar(10) not null," +
                "name varchar(100) not null)";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.ExecuteNonQuery();
            }
        }

        private StudyProgram FromReader(NpgsqlDataReader reader)
        {
            var obj = new StudyProgram();
            obj.ID = reader.GetInt32(0);
            obj.Acronym = reader.GetString(1);
            obj.Name = reader.GetString(2);
            return obj;
        }

        public IEnumerable<StudyProgram> All(int from = 0, int max = 1000)
        {
           string sql = "SELECT id, acronym, name from study_programs order by acronym, name limit :max offset :from";

            var result = new List<StudyProgram>();
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

            return result;
        }

        public StudyProgram ById(int id)
        {
            string sql = "SELECT id,acronym,name FROM study_programs where id=:id";

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

        public void Delete(int id)
        {
            string sql = "delete from study_programs where id=:id";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void Insert(StudyProgram obj)
        {
            string sql = "insert into study_programs " +
                    "(acronym,name) values (:acronym,:name) RETURNING id";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":acronym", obj.Acronym);
                cmd.Parameters.AddWithValue(":name", obj.Name);
                obj.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void Update(StudyProgram obj)
        {
            string sql = "update study_programs set acronym=:acronym,name=:name where id=:id";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":id", obj.ID);
                cmd.Parameters.AddWithValue(":acronym", obj.Acronym);
                cmd.Parameters.AddWithValue(":name", obj.Name);
                cmd.ExecuteNonQuery();
            }
        }

        public void Save(StudyProgram obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }
    }
}