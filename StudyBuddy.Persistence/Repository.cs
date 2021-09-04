using Npgsql;
using SimpleHashing.Net;
using StudyBuddy.Model;
using System;
using System.Linq;

namespace StudyBuddy.Persistence
{
    public class Repository : IRepository
    {
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



            this.Users = new UserRepository(connection_string);
            this.StudyPrograms = new StudyProgramRepository(connection_string);
            this.Teams = new TeamRepository(connection_string);
            this.Terms = new TermRepository(connection_string);
            this.Challenges = new ChallengeRepository(connection_string);
            this.GameBadges = new GameBadgeRepository(connection_string);
        }
    }
}