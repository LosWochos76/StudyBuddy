using System;
using System.Collections.Generic;
using System.IO;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class ImageRepository : IImageRepository
    {
        private readonly string connection_string;

        public ImageRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        public PersistentImage ById(int id)
        {
            var qh = new QueryHelper<PersistentImage>(connection_string, FromReader);
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject("select id,user_id,badge_id,name,length,content from images where id=:id");
        }

        public IEnumerable<PersistentImage> All(ImageFilter filter)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper<PersistentImage>(connection_string, FromReader);
            qh.Delete("images", "id", id);
        }

        public PersistentImage Insert(PersistentImage obj)
        {
            var qh = new QueryHelper<PersistentImage>(connection_string, FromReader);
            qh.AddParameter(":user_id", obj.UserID.HasValue ? obj.UserID : DBNull.Value);
            qh.AddParameter(":badge_id", obj.BadgeId.HasValue ? obj.BadgeId : DBNull.Value);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":length", obj.Length);
            qh.AddParameter(":content", obj.Content);

            obj.ID = qh.ExecuteScalar(
                "insert into images (user_id,badge_id,name,length,content) values " +
                "(:user_id,:badge_id,:name,:length,:content) RETURNING id");

            return obj;
        }

        public PersistentImage Save(PersistentImage obj)
        {
            if (obj.ID == 0)
                return Insert(obj);
            else
                return Update(obj);
        }

        public PersistentImage Update(PersistentImage obj)
        {
            var qh = new QueryHelper<PersistentImage>(connection_string, FromReader);
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":user_id", obj.UserID.HasValue ? obj.UserID : DBNull.Value);
            qh.AddParameter(":badge_id", obj.BadgeId.HasValue ? obj.BadgeId : DBNull.Value);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":length", obj.Length);
            qh.AddParameter(":content", obj.Content);

            qh.ExecuteScalar(
                "update images set user_id=:user_id,badge_id=:badge_id," +
                "name=:name,length=:length,content=:content where id=:id");

            return obj;
        }

        public PersistentImage OfUser(int user_id)
        {
            var qh = new QueryHelper<PersistentImage>(connection_string, FromReader);
            qh.AddParameter(":user_id", user_id);
            return qh.ExecuteQueryToSingleObject("select id,user_id,badge_id,name,length,content from images where user_id=:user_id");
        }

        private void CreateTable()
        {
            var qh = new QueryHelper<PersistentImage>(connection_string, FromReader);
            if (!qh.TableExists("images"))
            {
                qh.ExecuteNonQuery(
                "create table images (" +
                "id serial not null, " +
                "user_id int, " +
                "badge_id int, " +
                "name varchar(100), " +
                "length int not null," +
                "content bytea)");
            }
        }

        private PersistentImage FromReader(NpgsqlDataReader reader)
        {
            var obj = new PersistentImage();
            obj.ID = reader.GetInt32(0);
            obj.UserID = reader.IsDBNull(1) ? null : reader.GetInt32(1);
            obj.BadgeId = reader.IsDBNull(2) ? null : reader.GetInt32(2);
            obj.Name = reader.GetString(3);
            obj.Length = reader.GetInt32(4);

            using (var mem = new MemoryStream())
            {
                var s = reader.GetStream(5);
                s.CopyTo(mem);
                obj.Content = mem.ToArray();
            }
            
            return obj;
        }
    }
}