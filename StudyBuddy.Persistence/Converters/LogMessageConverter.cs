using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class LogMessageConverter : BaseConverter<LogMessage>
    {
        public override LogMessage Convert(DataSet set, int row)
        {
            var obj = new LogMessage();
            obj.Occurence = set.GetDateTime(row, "occurence");
            obj.Level = (LogLevel)set.GetInt(row, "level");
            obj.Source = (Component)set.GetInt(row, "source");
            obj.UserId = set.GetInt(row, "user_id");
            obj.Message = set.GetString(row, "message");
            return obj;
        }
    }
}

