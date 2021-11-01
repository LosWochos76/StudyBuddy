using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    class TagService : ITagService
    {
        private readonly IBackend backend;

        public TagService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<Tag> GetAll()
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            return backend.Repository.Tags.All();
        }

        public int GetCount()
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            return backend.Repository.Tags.Count();
        }

        public Tag GetById(int id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            return backend.Repository.Tags.ById(id);
        }

        public Tag Update(Tag obj)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            backend.Repository.Tags.Update(obj);
            return obj;
        }

        public Tag Insert(Tag obj)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            backend.Repository.Tags.Insert(obj);
            return obj;
        }

        public void Delete(int id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            backend.Repository.Tags.Delete(id);
        }

        private IEnumerable<string> SplitTags(string tags)
        {
            return tags
                .Replace("#", "")
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private Tag CreateOrFindSingle(string tag_name)
        {
            tag_name = tag_name.ToLower();
            var obj = backend.Repository.Tags.ByName(tag_name);

            if (obj == null)
            {
                obj = new Tag { Name = tag_name };
                Insert(obj);
            }

            return obj;
        }

        public IEnumerable<Tag> CreateOrFindMultiple(string tags)
        {
            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new Exception("Unauthorized!");

            var tag_list = SplitTags(tags);
            var object_list = new List<Tag>();

            foreach (var tag in tag_list)
                object_list.Add(CreateOrFindSingle(tag));

            return object_list;
        }
    }
}