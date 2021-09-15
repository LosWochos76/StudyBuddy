using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class StudyProgramRepository : IStudyProgramRepository
    {
        private string connection_string;

        public StudyProgramRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        private void CreateTable() 
        {
            var qh = new QueryHelper<StudyProgram>(connection_string, FromReader);

            if (!qh.TableExists("study_programs"))
            {
                qh.ExecuteNonQuery(
                    "create table study_programs (" +
                    "id serial primary key, " +
                    "acronym varchar(10) not null," +
                    "name varchar(100) not null)");
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
            var qh = new QueryHelper<StudyProgram>(connection_string, FromReader);
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            return qh.ExecuteQueryToObjectList(
                "SELECT id, acronym, name from study_programs order by acronym, name limit :max offset :from");
        }

        public StudyProgram ById(int id)
        {
            var qh = new QueryHelper<StudyProgram>(connection_string, FromReader);
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,acronym,name FROM study_programs where id=:id");
        }

        public StudyProgram ByAcronym(string acronym)
        {
            var qh = new QueryHelper<StudyProgram>(connection_string, FromReader);
            qh.AddParameter(":acronym", acronym);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,acronym,name FROM study_programs where lower(acronym)=lower(:acronym)");
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper<StudyProgram>(connection_string, FromReader);
            qh.Delete("study_programs", "id", id);
        }

        public void Insert(StudyProgram obj)
        {
            var qh = new QueryHelper<StudyProgram>(connection_string, FromReader);
            qh.AddParameter(":acronym", obj.Acronym);
            qh.AddParameter(":name", obj.Name);
            obj.ID = qh.ExecuteScalar(
                "insert into study_programs (acronym,name) values (:acronym,:name) RETURNING id");
        }

        public void Update(StudyProgram obj)
        {
            var qh = new QueryHelper<StudyProgram>(connection_string, FromReader);
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":acronym", obj.Acronym);
            qh.AddParameter(":name", obj.Name);
            qh.ExecuteNonQuery("update study_programs set acronym=:acronym,name=:name where id=:id");
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