using System;

namespace StudyBuddy.Model
{
    public abstract class Entity
    {
        public int ID { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Now.Date;
    }
}