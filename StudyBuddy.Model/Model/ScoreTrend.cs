using System;
using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class ScoreTrend
    {
        public Dictionary<string, Score> Values { get; private set; } = new Dictionary<string, Score>();

        public static ScoreTrend TestData()
        {
            var rnd = new Random();
            var result = new ScoreTrend();

            var date = DateTime.Now;
            for (int i=9; i>=0; i--)
            {
                var running_date = date.AddMonths(-i);
                var key = running_date.ToString("yyyy-MM");

                var score = new Score();
                score.Add(new Result() { Category = ChallengeCategory.Learning, Count = rnd.Next(1, 10), Points = rnd.Next(1, 100) });
                score.Add(new Result() { Category = ChallengeCategory.Organizing, Count = rnd.Next(1, 10), Points = rnd.Next(1, 100) });
                score.Add(new Result() { Category = ChallengeCategory.Networking, Count = rnd.Next(1, 10), Points = rnd.Next(1, 100) });
                result.Values.Add(key, score);
            }

            return result;
        }
    }
}