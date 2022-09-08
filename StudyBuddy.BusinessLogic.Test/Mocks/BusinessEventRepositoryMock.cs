using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class BusinessEventRepositoryMock : IBusinessEventRepository
    {
        private List<BusinessEvent> objects = new List<BusinessEvent>();

        public BusinessEventRepositoryMock()
        {
        }

        public IEnumerable<BusinessEvent> All(BusinessEventFilter filter)
        {
            return objects;
        }

        public void Delete(int id)
        {
            objects.RemoveAll(obj => obj.ID == id);
        }

        public BusinessEvent GetById(int id)
        {
            return objects.Where(obj => obj.ID == id).FirstOrDefault();
        }

        public int GetCount(BusinessEventFilter filter)
        {
            return objects.Count;
        }

        public BusinessEvent Insert(BusinessEvent obj)
        {
            if (obj.ID == 0)
                obj.ID = GetCount(null) + 1;

            objects.Add(obj);
            return obj;
        }

        public BusinessEvent Save(BusinessEvent obj)
        {
            return obj.ID == 0 ? Insert(obj) : Update(obj);
        }

        public BusinessEvent Update(BusinessEvent obj)
        {
            int pos = objects.FindIndex(o => o.ID == obj.ID);
            objects[pos] = obj;
            return obj;
        }
    }
}