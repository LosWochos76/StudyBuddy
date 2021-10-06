using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using StudyBuddy.App.Misc;
using TinyIoC;

namespace StudyBuddy.App.Api
{
    public class ApiFacade : IApi
    {
        //public static string base_url = "https://localhost:5001/";
        private readonly string base_url = "https://studybuddy.hshl.de/";
        private Version app_version = new Version(0, 0, 13, 0);

        public IAuthenticationService Authentication { get; }
        public IChallengeService Challenges { get; }
        public IFcmTokenService FcmTokens { get; }
        public IUserService Users { get; }
        public ITagService Tags { get; set; }

        public ApiFacade()
        {
            Authentication = new AuthenticationService(this, base_url);
            Challenges = new ChallengeService(this, base_url);
            FcmTokens = new FcmTokenService(this, base_url);
            Users = new UserService(this, base_url);
            Tags = new TagService(this, base_url);

            CheckVersion();
        }

        private async void CheckVersion()
        {
            var rh = new RequestHelper("");
            var result = await rh.DropRequest(base_url + "ApiVersion", HttpMethod.Get);
            if (result == null)
                return;

            var jtoken = JToken.Parse(result);
            var api_version = jtoken.ToObject<Version>();

            if (api_version > app_version)
            {
                var dialog = TinyIoCContainer.Current.Resolve<IDialogService>();
                await dialog.ShowError("App zu alt!", "Diese Version der App ist zu alt! Bitte updaten!", "Ok", null);
                throw new Exception("App is too old! Please update!");
            }
        }
    }
}