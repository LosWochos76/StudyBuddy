using System;
using NETCore.MailKit.Infrastructure.Internal;

namespace StudyBuddy.BusinessLogic
{
    class MailKitHelper
    {
        public static MailKitOptions GetMailKitOptions()
        {
            var options = new MailKitOptions();
            options.Server = Model.Environment.GetOrDefault("SMTP_SERVER", "localhost");
            options.Port = Convert.ToInt32(Model.Environment.GetOrDefault("SMTP_PORT", "587"));
            options.SenderName = Model.Environment.GetOrDefault("SMTP_SENDERNAME", "admin");
            options.SenderEmail = Model.Environment.GetOrDefault("SMTP_SENDEREMAIL", "admin@admin.de");
            options.Account = Model.Environment.GetOrDefault("SMTP_ACCOUNT", "admin@admin.de");
            options.Password = Model.Environment.GetOrDefault("SMTP_PASSWORD", "secret");
            options.Security = Convert.ToBoolean(Model.Environment.GetOrDefault("SMTP_SECURITY", "True"));
            return options;
        }
    }
}
