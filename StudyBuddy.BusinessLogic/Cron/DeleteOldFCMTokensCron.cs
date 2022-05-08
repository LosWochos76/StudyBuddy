using System.Threading;
using System.Threading.Tasks;
using EasyCronJob.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace StudyBuddy.BusinessLogic.Cron
{
    public class DeleteOldFCMTokensCron : BaseCron
    {
        public DeleteOldFCMTokensCron(ICronConfiguration<UnseenNotificationsCron> cronConfiguration,
            IServiceScopeFactory factory) : base(cronConfiguration, factory)
        {
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            backend.Repository.FcmTokens.DeleteOldTokens();
            return base.DoWork(cancellationToken);
        }
    }
}