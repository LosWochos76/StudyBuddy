using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class LoggingRepository : ILoggingRepository
    {
        private readonly string connection_string;

        public LoggingRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        private void CreateTable()
        {
            var qh = new QueryHelper<LogMessage>(connection_string, FromReader);
            if (!qh.TableExists("logging"))
            {
                qh.ExecuteNonQuery(
                "create table logging (" +
                "occurence timestamp not null, " +
                "level smallint not null, " +
                "user_id int not null, " +
                "source smallint not null, " +
                "message varchar(200))");
            }
        }

        public void Log(LogMessage obj)
        {
            var qh = new QueryHelper<LogMessage>(connection_string);
            qh.AddParameter(":occurence", obj.Occurence);
            qh.AddParameter(":level", (int)obj.Level);
            qh.AddParameter(":user_id", obj.UserId);
            qh.AddParameter(":source", (int)obj.Source);
            qh.AddParameter(":message", obj.Message.Truncate(195));
            qh.ExecuteNonQuery(
                "insert into logging (occurence,level,source,user_id,message) values " +
                "(:occurence,:level,:source,:user_id,:message)");
        }

        public void Flush()
        {
            var qh = new QueryHelper<LogMessage>(connection_string, FromReader);
            qh.ExecuteNonQuery("truncate logging");
        }

        public IEnumerable<LogMessage> All(LogMessageFilter filter)
        {
            var qh = new QueryHelper<LogMessage>(connection_string, FromReader);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":min_log_level", (int)filter.MinLogLevel);

            var sql = "select occurence,level,source,user_id,message from logging " +
                "where level >= :min_log_level ";

            if (filter.UserId.HasValue)
            {
                qh.AddParameter(":user_id", filter.UserId.Value);
                sql += " AND user_id=:user_id";
            }

            sql += " order by occurence desc limit :max offset :from";

            return qh.ExecuteQueryToObjectList(sql);
        }

        private LogMessage FromReader(NpgsqlDataReader reader)
        {
            var obj = new LogMessage();
            obj.Occurence = reader.GetDateTime(0);
            obj.Level = (LogLevel)reader.GetInt16(1);
            obj.Source = (Component)reader.GetInt16(2);
            obj.UserId = reader.GetInt32(3);
            obj.Message = reader.GetString(4);
            return obj;
        }
    }
}