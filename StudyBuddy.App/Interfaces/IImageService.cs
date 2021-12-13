using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IImageService
    {
        Task<bool> GetProfileImage(UserViewModel user);
        Task<bool> SaveProfileImage(PersistentImageViewModel user);
    }
}