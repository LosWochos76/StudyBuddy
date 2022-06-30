using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class ImageRepository : IImageRepository
    {
        private readonly string connection_string;
        private readonly PersistentImageConverter converter = new PersistentImageConverter();

        public ImageRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        public PersistentImage ById(int id)
        {
            var qh = new QueryHelper<PersistentImage>(connection_string);
            qh.AddParameter(":id", id);
            var set = qh.ExecuteQueryToDataSet("select id,user_id,name,length,content from images where id=:id");
            return converter.Single(set);
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper<PersistentImage>(connection_string);
            qh.Delete("images", "id", id);
        }

        public PersistentImage Insert(PersistentImage obj)
        {
            var qh = new QueryHelper<PersistentImage>(connection_string);
            qh.AddParameter(":user_id", obj.UserID);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":length", obj.Length);
            qh.AddParameter(":content", obj.Content);

            obj.ID = qh.ExecuteScalar(
                "insert into images (user_id,name,length,content) values " +
                "(:user_id,:name,:length,:content) RETURNING id");

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
            var qh = new QueryHelper<PersistentImage>(connection_string);
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":user_id", obj.UserID);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":length", obj.Length);
            qh.AddParameter(":content", obj.Content);

            qh.ExecuteScalar(
                "update images set user_id=:user_id," +
                "name=:name,length=:length,content=:content where id=:id");

            return obj;
        }

        public PersistentImage OfUser(int user_id)
        {
            var qh = new QueryHelper<PersistentImage>(connection_string);
            qh.AddParameter(":user_id", user_id);
            var set = qh.ExecuteQueryToDataSet("select id,user_id,name,length,content from images where user_id=:user_id");
            return converter.Single(set);
        }

        private void CreateTable()
        {
            var qh = new QueryHelper<PersistentImage>(connection_string);
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
    }
}