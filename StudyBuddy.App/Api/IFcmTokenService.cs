using System.Net.Http;
using System.Threading.Tasks;

namespace StudyBuddy.App.Api
{
    public interface IFcmTokenService
    { 
        Task<HttpResponseMessage> Save(string fcmToken);
    }
}