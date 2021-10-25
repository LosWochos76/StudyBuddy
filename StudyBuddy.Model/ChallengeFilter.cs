namespace StudyBuddy.BusinessLogic
{
    public class PaginationParmeter
    {
        public int Start { get; set; } = 0;
        public int Count { get; set; } = 1000;
        public string Search { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
    }
}