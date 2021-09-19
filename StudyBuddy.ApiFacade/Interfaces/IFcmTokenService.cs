using System.Net.Http;
using System.Threading.Tasks;

namespace StudyBuddy.ApiFacade
{
    public interface IFcmTokenService
    { 
        Task<HttpResponseMessage> Save(string fcmToken);
    }
    
    
}