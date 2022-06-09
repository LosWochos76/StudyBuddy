using System;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using Environment = StudyBuddy.Model.Environment;

namespace StudyBuddy.BusinessLogic
{
    public class MailKitHelper
    {
        public static MailKitOptions GetMailKitOptions()
        {
            var options = new MailKitOptions();
            options.Server = Environment.GetOrDefault("SMTP_SERVER", "smtp.ethereal.email");
            options.Port = Convert.ToInt32(Environment.GetOrDefault("SMTP_PORT", "587"));
            options.SenderName = Environment.GetOrDefault("SMTP_SENDERNAME", "admin");
            options.SenderEmail = Environment.GetOrDefault("SMTP_SENDEREMAIL", "admin@admin.de");
            options.Account = Environment.GetOrDefault("SMTP_ACCOUNT", "armand.okon36@ethereal.email");
            options.Password = Environment.GetOrDefault("SMTP_PASSWORD", "2kMgT71gNB4dB5rzbP");
            options.Security = Convert.ToBoolean(Environment.GetOrDefault("SMTP_SECURITY", "True"));
            return options;
        }

        public static bool SendMail(string mailTo, string subject, string message)
        {
            try
            {
                var options = GetMailKitOptions();
                var provider = new MailKitProvider(options);
                var mail = new EmailService(provider);
                mail.Send(mailTo, subject, message, true);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}