using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Persistence
{
    public class CommentsRepository
    {
        private readonly string connection_string;

        public CommentsRepository(string connection_string)
        {
            this.connection_string = connection_string;
            CreateTable();
        }


        private void CreateTable()
        {
            var qh = new QueryHelper<Comment>(connection_string, FromNotificationReader);
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

        private Comment FromNotificationReader(NpgsqlDataReader reader)
        {
            return new Comment
            {
                Id = reader.GetInt32(0),
                NotificationId = reader.GetInt32(1),
                OwnerId = reader.GetInt32(2),
                Text = reader.GetString(3),
                Created = reader.GetDateTime(4),
                Updated = reader.GetDateTime(5),
                Owner = new User
                {
                    ID = reader.GetInt32(6),
                    Firstname = reader.GetString(7),
                    Lastname = reader.GetString(8),
                    Nickname = reader.GetString(9),
                    Email = reader.GetString(10),
                    PasswordHash = reader.GetString(11),
                    Role = (Role) reader.GetInt32(12)
                }
            };
        }

        public IEnumerable<Comment> All(CommentFilter filter)
        {
            var qh = new QueryHelper<Comment>(connection_string, FromNotificationReader);
            var sql = "select c.id, c.notification_id, c.owner_id, c.text, c.created, c.updated, " +
                      " u.id, u.firstname, u.lastname, u.nickname, u.email, u.password_hash, u.role " +
                      " from comments as c left outer join users as u on u.id = c.owner_id  where true ";

            if (filter.NotificationId.HasValue)
            {
                qh.AddParameter(":notification_id", filter.NotificationId.Value);
                sql += " and (notification_id=:notification_id) ";
            }

            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);
            sql += " limit :max offset :from ";


            return qh.ExecuteQueryToObjectList(sql);
        }

        public void Insert(CommentInsert insert)
        {
            var qh = new QueryHelper<Comment>(connection_string, FromNotificationReader);
            qh.AddParameter(":owner_id", insert.OwnerId);
            qh.AddParameter(":notification_id", insert.NotificationId);
            qh.AddParameter(":text", insert.Text);
            qh.ExecuteScalar(
                "insert into comments (owner_id, notification_id, text) values (:owner_id, :notification_id, :text)");
        }
    }
}