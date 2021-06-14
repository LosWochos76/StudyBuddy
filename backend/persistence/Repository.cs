using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Persistence
{
    public class Repository : IRepository
    {
        private StudyBuddyContext context;
        
        public IUserRepository Users { get; private set; }

        public Repository()
        {
            var connection_string = string.Format("Host={0};Username={1};Password={2};Database={3}",
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_HOST", "localhost"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_USER", "postgres"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_PASSWORD", "secret"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_DATABASE", "postgres"));

            this.context = new StudyBuddyContext(connection_string);
            this.context.Database.Migrate();

            this.Users = new UserRepository(this.context);
        }
    }
}