namespace StudyBuddy.Model
{
    public abstract class BaseFilter
    {
        public int Start { get; set; } = 0;
        public int Count { get; set; } = int.MaxValue;
    }
}