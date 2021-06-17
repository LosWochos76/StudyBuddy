using Microsoft.EntityFrameworkCore;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class StudyBuddyContext : DbContext
    {
        private string connection_string;
        public DbSet<User> Users { get; set; }
        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<Team> Teams { get; set; }

        public StudyBuddyContext() 
        {
            this.connection_string = string.Format("Host={0};Username={1};Password={2};Database={3}",
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_HOST", "localhost"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_USER", "postgres"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_PASSWORD", "secret"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_DATABASE", "postgres"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connection_string);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>();
            modelBuilder.Entity<StudyProgram>();
            modelBuilder.Entity<Team>();
        }
    }
}