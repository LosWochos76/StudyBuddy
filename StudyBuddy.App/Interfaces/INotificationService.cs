using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetAllMyNotifications();
        Task<IEnumerable<NewsViewModel>> GetMyNotificationFeed();
    }
}