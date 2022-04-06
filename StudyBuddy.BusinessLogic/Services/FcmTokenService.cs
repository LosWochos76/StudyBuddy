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

        public IEnumerable<FcmToken> All()
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (backend.CurrentUser.IsAdmin)
                return backend.Repository.FcmTokens.All();

            return backend.Repository.FcmTokens.OfUser(backend.CurrentUser.ID);
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
    }
}