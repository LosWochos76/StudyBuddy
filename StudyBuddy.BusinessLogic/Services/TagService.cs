using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.BusinessLogic
{
    class TagService : ITagService
    {
        private readonly IBackend backend;

        public TagService(IBackend backend)
        {
            this.backend = backend;
        }

        public TagList GetAll(TagFilter filter)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            if (filter == null)
                filter = new TagFilter();

            return new TagList()
            {
                Count = backend.Repository.Tags.GetCount(filter),
                Objects = backend.Repository.Tags.All(filter)
            };
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
                .Split(new[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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

        public TagList CreateOrFindMultiple(string tags)
        {
            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new Exception("Unauthorized!");

            if (string.IsNullOrWhiteSpace(tags))
                return new TagList() { Count = 0, Objects = new List<Tag>() };

            var tag_list = SplitTags(tags);
            var object_list = new List<Tag>();

            foreach (var tag in tag_list)
                object_list.Add(CreateOrFindSingle(tag));

            return new TagList()
            {
                Count = object_list.Count,
                Objects = object_list
            };
        }
    }
}