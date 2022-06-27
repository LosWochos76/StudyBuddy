using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public abstract class BaseConverter<T>
    {
        public abstract T Convert(DataSet set, int row);

        public T Single(DataSet set)
        {
            if (set.HasRows)
                return Convert(set, 0);
            else
                return default(T);
        }

        public IEnumerable<T> Multiple(DataSet set)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < set.RowCount; i++)
                result.Add(Convert(set, i));

            return result;
        }
    }
}

