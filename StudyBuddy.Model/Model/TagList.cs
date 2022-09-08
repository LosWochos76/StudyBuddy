using System.Collections.Generic;
using System.Linq;

namespace StudyBuddy.Model
{
    public class TagList
    {
        public int Count { get; set; }
        public IEnumerable<Tag> Objects { get; set; }

        public TagList()
        {
        }

        public TagList(IEnumerable<Tag> objects) : this(objects, objects.Count())
        {
        }

        public TagList(IEnumerable<Tag> objects, int count)
        {
            Objects = objects;
            Count = count;
        }

        public static string ToString(IEnumerable<Tag> objects)
        {
            return string.Join(" ", objects.Select(obj => "#" + obj.Name.ToLower()));
        }
    }
}