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

        public FcmTokenList GetAll(FcmTokenFilter filter)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            return new FcmTokenList()
            {
                Objects = backend.Repository.FcmTokens.GetAll(filter),
                Count = backend.Repository.FcmTokens.GetCount(filter)
            };  
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

        public void DeleteOldTokens()
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            backend.Repository.FcmTokens.DeleteOldTokens();
        }
    }
}