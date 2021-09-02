using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface ITermRepository
    {
        Term ById(int id);
        Term ByDate(DateTime date);
        IEnumerable<Term> All(int from = 0, int max = 1000);
        void Insert(Term obj);
        void Update(Term obj);
        void Save(Term obj);
        void Delete(int id);
        Term Current();
    }
}
