using System.Collections.Generic;
using System.Text;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Persistence
{
    public class CommentsRepository
    {
        private readonly string connection_string;
        private readonly CommentConverter converter = new CommentConverter();

        public CommentsRepository(string connection_string)
        {
            this.connection_string = connection_string;
            CreateTable();
        }

        private void CreateTable()
        {
            var qh = new QueryHelper(connection_string);
            if (!qh.TableExists("comments"))
                qh.ExecuteNonQuery(
                    "create table comments (" +
                    "id serial primary key, " +
                    "notification_id int not null, " +
                    "owner_id int not null, " +
                    "text text, " +
                    "created timestamp default current_timestamp, " +
                    "updated timestamp default current_timestamp " +
                    ")");
        }

        public IEnumerable<Comment> All(CommentFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            var sql = new StringBuilder("select c.id, c.notification_id, c.owner_id, c.text, c.created, c.updated, " +
                      " u.id as user_id, u.firstname, u.lastname, u.nickname, u.email, u.role " +
                      " from comments as c left outer join users as u on u.id = c.owner_id where true ");

            if (filter.NotificationId.HasValue)
            {
                qh.AddParameter(":notification_id", filter.NotificationId.Value);
                sql.Append(" and (notification_id=:notification_id) ");
            }

            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);
            sql.Append(" limit :max offset :from ");

            var set = qh.ExecuteQuery(sql.ToString());
            return converter.Multiple(set);
        }

        public void Insert(CommentInsert insert)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":owner_id", insert.OwnerId);
            qh.AddParameter(":notification_id", insert.NotificationId);
            qh.AddParameter(":text", insert.Text);
            qh.ExecuteScalar(
                "insert into comments (owner_id, notification_id, text) values (:owner_id, :notification_id, :text)");
        }
    }
}