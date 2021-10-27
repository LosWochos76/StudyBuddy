using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class TagListViewModel
    {
        public IEnumerable<Tag> Tags { get; set; }

        public override string ToString()
        {
            return string.Join(" ", (from tag in Tags select "#" + tag.Name).ToArray());
        }
    }
}
