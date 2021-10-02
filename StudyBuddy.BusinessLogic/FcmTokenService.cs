using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class FcmTokenService
    {
        private readonly IRepository repository;

        public FcmTokenService(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<FcmToken> All(User current_user)
        {
            if (current_user.IsAdmin)
                return repository.FcmTokens.All();
            return repository.FcmTokens.OfUser(current_user.ID);
        }

        public FcmToken Save(FcmToken fcmToken)
        {
            repository.FcmTokens.Save(fcmToken);
            return fcmToken;
        }
    }
}