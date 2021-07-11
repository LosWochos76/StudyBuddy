using Npgsql;
using SimpleHashing.Net;
using StudyBuddy.Model;
using System;
using System.Linq;

namespace StudyBuddy.Persistence
{
    public class Repository : IRepository, IDisposable
    {
        private NpgsqlConnection connection;
        
        public IUserRepository Users { get; private set; }
        public IStudyProgramRepository StudyPrograms { get; private set; }
        public ITeamRepository Teams { get; private set; }
        public ITermRepository Terms { get; private set; }
        public IChallengeRepository Challenges { get; private set; }
        public IGameBadgeRepository GameBadges { get; private set; }

        public Repository()
        {
            var connection_string = string.Format("Host={0};Username={1};Password={2};Database={3}",
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_HOST", "localhost"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_USER", "postgres"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_PASSWORD", "secret"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_DATABASE", "postgres"));

            this.connection = new NpgsqlConnection(connection_string);
            connection.Open();

            this.Users = new UserRepository(this.connection);
            this.StudyPrograms = new StudyProgramRepository(this.connection);
            this.Teams = new TeamRepository(this.connection);
            this.Terms = new TermRepository(this.connection);
            this.Challenges = new ChallengeRepository(this.connection);
            this.GameBadges = new GameBadgeRepository(this.connection);
        }

        public void Dispose()
        {
            this.connection.Close();
        }
    }
}