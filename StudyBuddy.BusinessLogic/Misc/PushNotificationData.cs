using System.Collections.Generic;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using StudyBuddy.Model.Enum;

namespace StudyBuddy.BusinessLogic
{ 
    public class PushNotificationData
    {
        public PushNotificationTypes PushNotificationType { get; set; } = PushNotificationTypes.Normal;
        public Optional<string> PagePath { get; set; }
        public Dictionary<string, string> toData()
        {
            var dictonary = new Dictionary<string, string>();
            dictonary.Add("PushNotificationType", this.PushNotificationType.ToString());
            if (!string.IsNullOrEmpty(PagePath.Value))
            {
                dictonary.Add("PagePath", this.PagePath.Value);
            }

            return dictonary;
        }
        
    }
}