using System;

namespace StudyBuddy.Model
{
    public class Term : Entity
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Term()
        {
            Start = DateTime.Now.Date;
            End = DateTime.Now.Date;
        }
    }
}