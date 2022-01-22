namespace StudyBuddy.Model.Filter
{
    public class RequestFilter : BaseFilter
    {
        public int? OnlyForSender { get; set; }
        public int? OnlyForRecipient { get; set; }
        public RequestType? OnlyForType { get; set; }

        public bool WithSender { get; set; }
        public bool WithRecipient { get; set; }
        public bool WithChallenge { get; set; }
    }
}