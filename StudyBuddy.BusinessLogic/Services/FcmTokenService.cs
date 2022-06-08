using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public class FcmTokenService : IFcmTokenService
    {
        private readonly IBackend backend;

        public FcmTokenService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<FcmToken> GetAll()
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (backend.CurrentUser.IsAdmin)
                return backend.Repository.FcmTokens.GetAll();

            return backend.Repository.FcmTokens.ForUser(backend.CurrentUser.ID);
        }

        public FcmToken Save(FcmTokenSaveDto obj)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (obj == null)
                throw new Exception("Object is null!");

            var token = obj.ToFcmToken();
            token.UserID = backend.CurrentUser.ID;
            backend.Repository.FcmTokens.Save(token);
            return token;
        }

        public IEnumerable<FcmToken> GetForUser(int userId)
        {
            return backend.Repository.FcmTokens.ForUser(userId);
        }
    }
}