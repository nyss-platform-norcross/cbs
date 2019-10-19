using Microsoft.Extensions.DependencyInjection;
using Nyss.Web.Features.Reports.Data;

namespace Nyss.Web.Features.Reports
{
    public static class ReportServicesRegistration
    {
        public static void RegisterReportFeature(this IServiceCollection services)
        {
            services.AddScoped<IReportService, RandomReportService>();
            services.AddScoped<IReportRepository, InMemoryReportRepository>();
        }
    }
}
