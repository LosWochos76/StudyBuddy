using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class TagService
    {
        private IRepository repository;
        private User current_user;

        public TagService(IRepository repository, User current_user)
        {
            this.repository = repository;
            this.current_user = current_user;
        }

        public IEnumerable<Tag> GetAll()
        {
            TagAuthorization.CheckGetAll(current_user);
            return repository.Tags.All();
        }

        public int GetCount()
        {
            TagAuthorization.CheckGetCount(current_user);
            return repository.Tags.Count();
        }

        public Tag GetById(int id)
        {
            TagAuthorization.CheckGetById(current_user);
            return repository.Tags.ById(id);
        }

        public Tag Update(Tag obj)
        {
            TagAuthorization.CheckUpdate(current_user);
            repository.Tags.Update(obj);
            return obj;
        }

        public Tag Insert(Tag obj)
        {
            TagAuthorization.CheckInsert(current_user);
            repository.Tags.Insert(obj);
            return obj;
        }

        public void Delete(int id)
        {
            TagAuthorization.CheckDelete(current_user);
            repository.Tags.Delete(id);
        }

        private IEnumerable<string> SplitTags(string tags)
        {
            return
                tags.Replace("#", "").
                Split(new char[] {' ','\t'}, StringSplitOptions.RemoveEmptyEntries);
        }

        public Tag CreateOrFindSingle(string tag_name)
        {
            TagAuthorization.CheckCreateOrFindSigle(current_user);

            tag_name = tag_name.ToLower();
            var obj = repository.Tags.ByName(tag_name);

            if (obj == null)
            {
                obj =  new Tag(){ Name = tag_name };
                Insert(obj);
            }

            return obj;
        }

        public IEnumerable<Tag> CreateOrFindMultiple(string tags)
        {
            var tag_list = SplitTags(tags);
            var object_list = new List<Tag>();

            foreach (var tag in tag_list) 
                object_list.Add(CreateOrFindSingle(tag));

            return object_list;
        }
    }
}
