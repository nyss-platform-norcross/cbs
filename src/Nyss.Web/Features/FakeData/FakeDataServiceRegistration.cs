using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nyss.Web.Features.Reports;
using Nyss.Web.Features.Reports.Data;

namespace Nyss.Web.Features.FakeData
{
    public static class FakeDataServiceRegistration
    {
        public static void RegisterFakeDataFeature(this IServiceCollection services)
        {
            services.AddSingleton<IFakeDataService, FakeDataService>();
        }
    }
}
