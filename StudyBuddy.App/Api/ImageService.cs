using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

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

        public async Task<bool> SaveProfileImage(PersistentImageViewModel image)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var result = await rh.Load<PersistentImage>(base_url + "Image/", HttpMethod.Post, image);

            if (result != null)
            {
                image = PersistentImageViewModel.FromModel(result);
                return true;
            }

            return false;
        }

        public async Task<bool> GetProfileImage(UserViewModel user)
        {
            return false;
        }
    }
}