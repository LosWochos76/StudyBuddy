using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class ChallengeService
    {
        private IRepository repository;

        public ChallengeService(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Challenge> All(User current_user)
        {
            if (current_user.IsAdmin)
                return repository.Challenges.All();
            else
                return repository.Challenges.OfOwner(current_user.ID);
        }

        public Challenge GetById(int id)
        {
            return repository.Challenges.ById(id);
        }

        public IEnumerable<Challenge> GetByText(User current_user, string text)
        {
            if (current_user == null || !current_user.IsAdmin)
                return repository.Challenges.OfOwnerByText(current_user.ID, text);
            else
                return repository.Challenges.ByText(text);
        }

        public Challenge Update(User current_user, Challenge obj)
        {
            if (obj == null)
                throw new Exception("Object invalid!");

            if (!current_user.IsAdmin && obj.OwnerID != current_user.ID)
                throw new Exception("Unauthorized");

            repository.Challenges.Update(obj);
            return obj;
        }

        public IEnumerable<Challenge> OfBadge(int id)
        {
            return repository.Challenges.OfBadge(id);
        }

        public IEnumerable<Challenge> NotOfBadge(int id)
        {
            return repository.Challenges.NotOfBadge(id);
        }

        public Challenge Insert(Challenge obj)
        {
            repository.Challenges.Insert(obj);
            return obj;
        }

        public void Delete(User current_user, int id)
        {
            var obj = repository.Challenges.ById(id);
            if (!current_user.IsAdmin && obj != null && obj.OwnerID != current_user.ID)
                throw new Exception("Unauthorized");

            repository.Challenges.Delete(id);
        }

        public void CreateSeries(User current_user, CreateSeriesParameter param)
        {
            var parent = repository.Challenges.ById(param.ChallengeId);
            if (parent == null)
                throw new Exception("Object not found!");

            if (!current_user.IsAdmin && parent != null && parent.OwnerID != current_user.ID)
                throw new Exception("Unauthorized");

            for (int i = 0; i < param.Count; i++)
            {
                var clone = parent.Clone();
                clone.SeriesParentID = parent.ID;
                clone.ValidityStart = clone.ValidityStart.AddDays((i + 1) * param.DaysAdd);
                clone.ValidityEnd = clone.ValidityEnd.AddDays((i + 1) * param.DaysAdd);
                repository.Challenges.Insert(clone);
            }
        }
    }
}