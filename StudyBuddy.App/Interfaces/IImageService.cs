using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IImageService
    {
        Task<bool> GetProfileImage(UserViewModel user);
        Task<bool> SaveProfileImage(UserViewModel user, MediaFile file);
        Task GetProfileImages(IEnumerable<UserViewModel> users);
        Task GetProfileImages(IEnumerable<RequestViewModel> requests);
    }
}