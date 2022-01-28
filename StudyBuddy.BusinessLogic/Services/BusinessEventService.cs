using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
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

        public BusinessEventList All(BusinessEventFilter filter)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (filter == null)
                filter = new BusinessEventFilter();

            return new BusinessEventList()
            {
                Count = backend.Repository.BusinessEvents.GetCount(filter),
                Objects = backend.Repository.BusinessEvents.All(filter)
            };  
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
                    Execute(bi, args);
            }
        }

        public void Execute(int id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            var obj = backend.Repository.BusinessEvents.GetById(id);
            if (obj == null)
                throw new Exception("Object is null!");

            var args = new BusinessEventArgs(obj.Type, null);
            args.CurrentUser = backend.CurrentUser;

            Execute(obj, args);
        }

        public IEnumerable<string> Compile(BusinessEvent obj)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            var args = new BusinessEventArgs(obj.Type, null) { CurrentUser = backend.CurrentUser };
            var script = BuildScript(obj, args);

            var messages = new List<string>();
            foreach (var msg in script.Compile())
                messages.Add(msg.ToString());

            return messages;
        }

        private async void Execute(BusinessEvent bi, BusinessEventArgs args)
        {
            backend.Logging.LogDebug("Executing code of BusinessEvent " + bi.ID);

            try
            {
                var script = BuildScript(bi, args);
                if (script != null)
                    await script.RunAsync();
            }
            catch (Exception e)
            {
                backend.Logging.LogError("Error executing code of BusinessEvent "
                    + bi.ID + ": " + e.ToString()); ;
            }
        }

        private Script BuildScript(BusinessEvent bi, BusinessEventArgs args)
        {
            try
            {
                var options = ScriptOptions.Default;
                options = options.AddReferences("StudyBuddy.Model.dll");
                options = options.AddReferences("StudyBuddy.Persistence.dll");
                options = options.AddReferences("StudyBuddy.BusinessLogic.dll");
                options = options.AddImports("System");
                options = options.AddImports("StudyBuddy.Model");
                options = options.AddImports("StudyBuddy.Persistence");
                options = options.AddImports("StudyBuddy.BusinessLogic");

                var code = new StringBuilder("var backend = new Backend();\n" +
                    "backend.CurrentUser = backend.Repository.Users.ById(" + bi.OwnerID + ");\n" +
                    "var args = new BusinessEventArgs(BusinessEventType." + args.Type.ToString() + ");\n");

                if (args.CurrentUser != null)
                {
                    code.Append("args.CurrentUser = backend.Repository.Users.ById(" + args.CurrentUser.ID + ");\n");
                }

                if (args.Payload is User)
                {
                    var user = args.Payload as User;
                    code.Append("args.Payload = backend.Repository.Users.ById(" + user.ID + ");\n");
                }

                if (args.Payload is Challenge)
                {
                    var challenge = args.Payload as Challenge;
                    code.Append("args.Payload = backend.Repository.Challenges.ById(" + challenge.ID + ");\n");
                }

                if (args.Payload is GameBadge)
                {
                    var badge = args.Payload as GameBadge;
                    code.Append("args.Payload = backend.Repository.GameBadges.ById(" + badge.ID + ");\n");
                }

                code.Append(bi.Code);
                return CSharpScript.Create(code.ToString(), options);
            }
            catch (Exception e)
            {
                backend.Logging.LogError("Error compiling code of BusinessEvent "
                    + bi.ID + ": " + e.ToString()); ;
            }

            return null;
        }
    }
}