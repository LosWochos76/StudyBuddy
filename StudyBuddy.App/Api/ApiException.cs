using System;

namespace StudyBuddy.App.Api
{
    public class ApiException : Exception
    {
        public int Code { get; set; }
        public string Error { get; set; }
    }
}
