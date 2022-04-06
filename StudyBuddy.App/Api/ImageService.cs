using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.Api
{
    public class ImageService : IImageService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public ImageService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<bool> SaveProfileImage(UserViewModel user, MediaFile image)
        {
            var pi = new PersistentImage() { UserID = user.ID };
            using (var m = new MemoryStream())
            {
                image.GetStream().CopyTo(m);
                pi.Content = m.ToArray();
                pi.Length = pi.Content.Length;
                pi.Name = "profile_image.jpg";
            }

            var rh = new WebRequestHelper(api.Authentication.Token);
            var result = await rh.Load<PersistentImage>(base_url + "Image/OfUser", HttpMethod.Post, pi);
            user.ProfileImage = ImageSource.FromStream(() => new MemoryStream(result.Content));

            return true;
        }

        public async Task<bool> GetProfileImage(UserViewModel user)
        {
            if (user == null)
                return false;

            var rh = new WebRequestHelper(api.Authentication.Token);
            var result = await rh.Load<PersistentImage>(base_url + "Image/OfUser/" + user.ID, HttpMethod.Get);
            if (result == null)
                return false;

            user.ProfileImage = ImageSource.FromStream(() => new MemoryStream(result.Content));
            return true;
        }

        public async Task GetProfileImages(IEnumerable<UserViewModel> users)
        {
            foreach (var user in users)
                await GetProfileImage(user);
        }

        public async Task GetProfileImages(IEnumerable<RequestViewModel> requests)
        {
            foreach (var request in requests)
                await GetProfileImage(request.Sender);
        }
    }
}