using Microsoft.Extensions.DependencyInjection;

namespace Nyss.Web.Features.SmsGateway.Logic
{
    public static class ReportServicesRegistration
    {
        public static void RegisterSmsGatewayFeature(this IServiceCollection services)
        {
            services.AddScoped<ISmsGatewayService, InMemorySmsGatewayService>();
            services.AddScoped<ISmsParser, SmsParser>();
        }
    }
}