using EasyCronJob.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace StudyBuddy.BusinessLogic.Cron
{
    public abstract class BaseCron : CronJobService
    {
        protected readonly IBackend backend;

        protected BaseCron(ICronConfiguration<UnseenNotificationsCron> cronConfiguration,
            IServiceScopeFactory factory)
            : base(cronConfiguration.CronExpression, cronConfiguration.TimeZoneInfo, cronConfiguration.CronFormat)
        {
            backend = factory.CreateScope().ServiceProvider.GetRequiredService<IBackend>();
        }
    }
}