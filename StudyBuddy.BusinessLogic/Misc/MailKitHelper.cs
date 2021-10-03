﻿using System;
using NETCore.MailKit.Infrastructure.Internal;
using Environment = StudyBuddy.Model.Environment;

namespace StudyBuddy.BusinessLogic
{
    internal class MailKitHelper
    {
        public static MailKitOptions GetMailKitOptions()
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
    }
}