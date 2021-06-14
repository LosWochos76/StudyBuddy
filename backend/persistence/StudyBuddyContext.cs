using Microsoft.EntityFrameworkCore;
using SimpleHashing.Net;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class StudyBuddyContext : DbContext
    {
        private string connection_string;
        public DbSet<User> Users { get; set; }

        public StudyBuddyContext(string connection_string)
        {
            this.connection_string = connection_string;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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