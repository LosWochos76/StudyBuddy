namespace StudyBuddy.App.Api
{
    public class RequestResult
    {
        public string Status { get; set; }

        public bool IsOk
        {
            get { return Status.ToLower() == "ok"; }
        }
    }
}
