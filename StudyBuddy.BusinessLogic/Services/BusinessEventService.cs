using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    class BusinessEventService : IBusinessEventService
    {
        private readonly IBackend backend;
        private IEnumerable<BusinessEvent> cache = null;

        public BusinessEventService(IBackend backend)
        {
            this.backend = backend;
            ReloadCache();
        }

        private void ReloadCache()
        {
            this.cache = backend.Repository.BusinessEvents.All(new BusinessEventFilter());
        }

        public BusinessEvent GetById(int id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.Repository.BusinessEvents.GetById(id);
        }

        public IEnumerable<BusinessEvent> All(BusinessEventFilter filter)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (filter == null)
                filter = new BusinessEventFilter();

            return backend.Repository.BusinessEvents.All(filter);
        }

        public void Delete(int id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            backend.Repository.BusinessEvents.Delete(id);
            ReloadCache();
        }

        public BusinessEvent Insert(BusinessEvent obj)
        {
            if (obj == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            var result = backend.Repository.BusinessEvents.Insert(obj);
            ReloadCache();
            return result;
        }

        public BusinessEvent Update(BusinessEvent obj)
        {
            if (obj == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            var result = backend.Repository.BusinessEvents.Update(obj);
            ReloadCache();
            return result;
        }

        public void TriggerEvent(object sender, BusinessEventArgs args)
        {
            if (args.CurrentUser == null)
                args.CurrentUser = backend.CurrentUser;

            foreach (var bi in this.cache)
            {
                if (bi.Type == args.Type)
                    Execute(bi);
            }
        }

        private void Execute(BusinessEvent bi)
        {
            // Hier nun den Code des Events ausführen.
        }
    }
}
