using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class Repository : IRepository
    {
        private string connection_string;
        public IUserRepository Users { get; private set; }
        public IStudyProgramRepository StudyPrograms { get; private set; }
        public ITermRepository Terms { get; private set; }
        public IChallengeRepository Challenges { get; private set; }
        public IGameBadgeRepository GameBadges { get; private set; }
        public IRequestRepository Requests { get; private set; }
        public ITagRepository Tags { get; private set; }

        public Repository()
        {
            this.connection_string = string.Format("Host={0};Username={1};Password={2};Database={3}",
                Environment.GetOrDefault("POSTGRESQL_HOST", "localhost"),
                Environment.GetOrDefault("POSTGRESQL_USER", "postgres"),
                Environment.GetOrDefault("POSTGRESQL_PASSWORD", "secret"),
                Environment.GetOrDefault("POSTGRESQL_DATABASE", "postgres"));

            this.CreateTablesTable();
            this.CreateRepositories();
        }

        private void CreateTablesTable()
        {
            var qh = new QueryHelper<Repository>(connection_string, null);
            qh.CreateTablesTable();
        }

        private void CreateRepositories()
        {
            this.Users = new UserRepository(connection_string);
            this.StudyPrograms = new StudyProgramRepository(connection_string);
            this.Terms = new TermRepository(connection_string);
            this.Challenges = new ChallengeRepository(connection_string);
            this.GameBadges = new GameBadgeRepository(connection_string);
            this.Requests = new RequestRepository(connection_string);
            this.Tags = new TagRepository(connection_string);
        }
    }
}