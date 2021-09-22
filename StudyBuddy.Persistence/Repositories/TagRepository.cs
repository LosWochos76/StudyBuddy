using System;
using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class TagRepository : ITagRepository
    {
        private string connection_string;

        public TagRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        private void CreateTable()
        {
            var qh = new QueryHelper<Tag>(connection_string, FromReader);

            if (!qh.TableExists("tags"))
            {
                qh.ExecuteNonQuery(
                    "create table tags (" +
                    "id serial primary key, " +
                    "name varchar(50) not null);");
            }
        }

        private Tag FromReader(NpgsqlDataReader reader)
        {
            var obj = new Tag();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            return obj;
        }

        public Tag ById(int id)
        {
            var qh = new QueryHelper<Tag>(connection_string, FromReader);
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,name FROM tags where id=:id");
        }

        public Tag ByName(string name)
        {
            var qh = new QueryHelper<Tag>(connection_string, FromReader);
            qh.AddParameter(":name", name.ToLower());
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,name FROM tags where name=:name");
        }

        public int Count() 
        {
            var qh = new QueryHelper<Tag>(connection_string);
            return qh.GetCount("tags");
        }

        public IEnumerable<Tag> All(int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Tag>(connection_string, FromReader);
            qh.AddParameters(new { from, max });
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name FROM tags order by name limit :max offset :from");
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.Delete("tags", "id", id);
        }

        public void Insert(Tag obj)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new { name = obj.Name.ToLower().Replace("#","") });
            obj.ID = qh.ExecuteScalar(
                "insert into tags (name) values (:name) RETURNING id");
        }

        public void Update(Tag obj)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new { id = obj.ID, name = obj.Name.ToLower().Replace("#", "") });
            qh.ExecuteNonQuery("update tags set name=:name where id=:id");
        }

        public void Save(Tag obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }
    }
}