using StudyBuddy.BusinessLogic.Interfaces;
using StudyBuddy.Model.Model;

namespace StudyBuddy.BusinessLogic.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IBackend backend;
                
        public StatisticsService (IBackend backend)
        {
            this.backend = backend;
        }

        public UserStatistics GetUserStatistics(int user_id)
        {
            // Prüfen, ob user eingeloggt etc.

            var result = new UserStatistics();
            var challenges = backend.Repository.Challenges.Accepted(user_id);

            // Hier nun die die Punkte in den Kategorien berechnen und das Ergebnis-Objekt bestücken

            return result;
        }
    }
}
