using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class Score
    {
        public Dictionary<ChallengeCategory, Result> Values { get; private set; } = new Dictionary<ChallengeCategory, Result>();

        public void Add(Result value)
        {
            Values.Add(value.Category, value);
        }

        public void Add(IEnumerable<Result> results)
        {
            foreach (var result in results)
                Add(result);
        }

        public Score()
        {
        }

        public Score(IEnumerable<Result> results)
        {
            Add(results);
        }
    }
}