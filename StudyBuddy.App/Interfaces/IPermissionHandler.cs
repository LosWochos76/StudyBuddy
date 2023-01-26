using System.Threading.Tasks;

namespace StudyBuddy.App.Interfaces
{
    public interface IPermissionHandler
    {
        Task<bool> CheckCameraPermission();
        Task<bool> CheckStoragePermission();
    }
}