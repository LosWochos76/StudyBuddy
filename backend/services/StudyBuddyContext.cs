using System.Linq;
using Microsoft.EntityFrameworkCore;
using SimpleHashing.Net;
using StudyBuddy.Model;

namespace StudyBuddy.Services
{
    public class StudyBuddyContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection_string = string.Format("Host={0};Username={1};Password={2};Database={3}",
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_HOST", "localhost"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_USER", "postgres"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_PASSWORD", "secret"),
                Helper.GetFromEnvironmentOrDefault("POSTGRESQL_DATABASE", "postgres"));

            optionsBuilder.UseNpgsql(connection_string);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var simpleHash = new SimpleHash();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasData(new User { 
                    ID=1, 
                    Firstname="Alexander", 
                    Lastname="Stuckenholz",
                    Nickname="Stucki",
                    Email="alexander.stuckenholz@hshl.de",
                    PasswordHash=simpleHash.Compute("secret"),
                    Role = Role.Admin });
        }
    }
}