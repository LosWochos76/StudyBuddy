using System.Threading.Tasks;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public interface ILoggingService
    {
        Task Log(LogMessage message);
        Task LogDebug(string message);
        Task LogError(string message);
        Task LogInfo(string message);
    }
}