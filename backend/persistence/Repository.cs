using Microsoft.EntityFrameworkCore;
using SimpleHashing.Net;
using StudyBuddy.Model;
using System.Linq;

namespace StudyBuddy.Persistence
{
    public class Repository : IRepository
    {
        private StudyBuddyContext context;
        
        public IUserRepository Users { get; private set; }
        public IStudyProgramRepository StudyPrograms { get; private set; }
        public ITeamRepository Teams { get; private set; }

        public Repository()
        {
            this.context = new StudyBuddyContext();
            //this.context.Database.Migrate();

            this.Users = new UserRepository(this.context);
            this.StudyPrograms = new StudyProgramRepository(this.context);
            this.Teams = new TeamRepository(this.context);

            this.InitializeData();
        }

        private void InitializeData()
        {
            var simpleHash = new SimpleHash();
            
            if (Users.FindByEmail("alexander.stuckenholz@hshl.de") == null)
            {
               Users.Save(new User {
                    Firstname="Alexander", 
                    Lastname="Stuckenholz",
                    Nickname="Stucki",
                    Email="alexander.stuckenholz@hshl.de",
                    PasswordHash=simpleHash.Compute("secret"),
                    Role = Role.Admin });
            }

            if (Users.FindByEmail("eva.ponick@hshl.de") == null)
            {
               Users.Save(new User {
                    Firstname="Eva", 
                    Lastname="Ponick",
                    Nickname="Eva",
                    Email="eva.ponick@hshl.de",
                    PasswordHash=simpleHash.Compute("secret"),
                    Role = Role.Admin });
            }
        }
    }
}