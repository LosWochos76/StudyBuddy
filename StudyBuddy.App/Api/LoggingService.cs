using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class LoggingService : ILoggingService
    {
        private readonly IApi api;
        private readonly HttpClient client;

        public LoggingService(IApi api)
        {
            this.api = api;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task Log(LogMessage message)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(Settings.ApiUrl + "Logging/", HttpMethod.Post, message);
        }

        public async Task LogDebug(string message)
        {
            var current_user = api.Authentication.CurrentUser;
            await Log(new LogMessage()
            {
                Source = Component.App,
                Level = LogLevel.Debug,
                UserId = current_user == null ? 0 : current_user.ID,
                Message = message
            });
        }

        public async Task LogInfo(string message)
        {
            var current_user = api.Authentication.CurrentUser;
            await Log(new LogMessage()
            {
                Source = Component.App,
                Level = LogLevel.Information,
                UserId = current_user == null ? 0 : current_user.ID,
                Message = message
            });
        }

        public async Task LogError(string message)
        {
            var current_user = api.Authentication.CurrentUser;
            await Log(new LogMessage()
            {
                Source = Component.App,
                Level = LogLevel.Information,
                UserId = current_user == null ? 0 : current_user.ID,
                Message = message
            });
        }
    }
}