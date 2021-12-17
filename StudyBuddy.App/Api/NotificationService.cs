using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class NotificationService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public NotificationService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<IEnumerable<Notification>> GetAllMyNotifications()
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<IEnumerable<Notification>>(base_url + "Notification", HttpMethod.Get);
            
            return content;
        }
        
        public async Task<IEnumerable<NewsViewModel>> GetMyNotificationFeed()
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<IEnumerable<Notification>>(base_url + "Notification/Feed", HttpMethod.Get);
            if (content == null)
                return null;

            var result = new List<NewsViewModel>();
            foreach (var obj in content)
                result.Add(NewsViewModel.FromNotification(obj));

            return result;
        } 
    }
}