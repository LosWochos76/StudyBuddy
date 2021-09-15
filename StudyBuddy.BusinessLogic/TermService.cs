using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class TermService
    {
        private IRepository repository;

        public TermService(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Term> All()
        {
            return repository.Terms.All();
        }

        public Term GetById(int id)
        {
            return repository.Terms.ById(id);
        }

        public Term Update(Term obj)
        {
            repository.Terms.Update(obj);
            return obj;
        }

        public Term Insert(Term obj)
        {
            repository.Terms.Insert(obj);
            return obj;
        }

        public void Delete(int id)
        {
            repository.Terms.Delete(id);
        }

        public Term ByDate(DateTime date)
        {
            return repository.Terms.ByDate(date);
        }
    }
}
