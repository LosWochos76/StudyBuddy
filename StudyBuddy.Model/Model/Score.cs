namespace StudyBuddy.Model
{
    public class Score
    {
        public Result Learning { get; set; } = new Result() { Category = ChallengeCategory.Learning };
        public Result Organizing { get; set; } = new Result() { Category = ChallengeCategory.Organizing };
        public Result Networking { get; set; } = new Result() { Category = ChallengeCategory.Networking };

        public Result Total
        {
            get
            {
                return new Result()
                {
                    Points = Learning.Points + Organizing.Points + Networking.Points,
                    Count = Learning.Count + Organizing.Count + Networking.Count
                };
            }
        }
    }
}