using System.Threading;
using System.Threading.Tasks;
using EasyCronJob.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace StudyBuddy.BusinessLogic.Cron
{
    public class UnseenNotificationsCron : BaseCron
    {
        private readonly IBackend backend;

        public UnseenNotificationsCron(ICronConfiguration<UnseenNotificationsCron> cronConfiguration,
            IServiceScopeFactory factory) : base(cronConfiguration, factory)
        {
        }


        public override Task DoWork(CancellationToken cancellationToken)
        {
            backend.PushNotificationService.SendNewNotificationsAvailable();
            return base.DoWork(cancellationToken);
        }
    }
}