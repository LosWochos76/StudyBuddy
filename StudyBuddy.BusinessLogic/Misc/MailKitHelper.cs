using System;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using Environment = StudyBuddy.Model.Environment;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace StudyBuddy.BusinessLogic
{
    public class MailKitHelper
    {
        private MailKitOptions options;
        private readonly ILogger logger;

        public MailKitHelper(ILogger logger)
        {
            this.logger = logger;
            this.options = GetOptionsFromEnvironment();

            logger.LogInformation("Creating MailKitHelper");
        }

        private MailKitOptions GetOptionsFromEnvironment()
        {
            var options = new MailKitOptions();
            options.Server = Environment.GetOrDefault("SMTP_SERVER", "localhost");
            options.Port = Convert.ToInt32(Environment.GetOrDefault("SMTP_PORT", "587"));
            options.SenderName = Environment.GetOrDefault("SMTP_SENDERNAME", "admin");
            options.SenderEmail = Environment.GetOrDefault("SMTP_SENDEREMAIL", "admin@admin.de");
            options.Account = Environment.GetOrDefault("SMTP_ACCOUNT", "admin@admin.de");
            options.Password = Environment.GetOrDefault("SMTP_PASSWORD", "secret");
            options.Security = Convert.ToBoolean(Environment.GetOrDefault("SMTP_SECURITY", "True"));
            return options;
        }

        public bool SendMail(string mailTo, string subject, string message)
        {
            logger.LogInformation("MailKitHelper.SendMail");

            try
            {
                var provider = new MailKitProvider(options);
                var mail = new EmailService(provider);
                mail.Send(mailTo, subject, message, true);
            }
            catch (Exception e)
            {
                logger.LogError("Error sending mal!");
                logger.LogError(e.ToString());
                return false;
            }

            return true;
        }

        public Task<bool> SendMailAsync(string mailTo, string subject, string message)
        {
            return Task.Factory.StartNew(() =>
            {
                return SendMail(mailTo, subject, message);
            });
        }
    }
}