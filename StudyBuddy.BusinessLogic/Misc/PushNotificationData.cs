using System.Text.Json;

namespace StudyBuddy.BusinessLogic
{ 
    public class PushNotificationData
    {
        public string PagePath { get; set; }
        public string toJson()
        {
            return JsonSerializer.Serialize(new
            {
                PagePath = this.PagePath
            });
        }
        
    }
}