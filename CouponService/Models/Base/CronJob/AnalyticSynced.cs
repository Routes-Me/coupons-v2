using CouponService.Helper.CronJobServices.CronJobExtensionMethods;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CouponService.Models.Base.CronJob
{
    public class AnalyticSynced : CronJobService, IDisposable
    {
        private readonly IServiceScope _scope;
        public AnalyticSynced(IScheduleConfig<AnalyticSynced> config, IServiceProvider scope) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _scope = scope.CreateScope();
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override Task DoWork(CancellationToken cancellationToken)
        {
            IAnalyticsRepository _analyticsRepository = _scope.ServiceProvider.GetRequiredService<IAnalyticsRepository>();
            try
            {
                _analyticsRepository.InsertAnalytics();
            }
            catch (Exception) { }
            return Task.CompletedTask;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
