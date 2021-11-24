using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class BusinessEventRepository : IBusinessEventRepository
    {
        private readonly string connection_string;

        public BusinessEventRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        private void CreateTable()
        {

        }

        private BusinessEvent FromReader(NpgsqlDataReader reader)
        {
            var obj = new BusinessEvent();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            obj.OwnerID = reader.GetInt32(2);
            obj.Type = (BusinessEventType)reader.GetInt32(3);
            obj.Code = reader.GetString(4);
            return obj;
        }
    }
}
