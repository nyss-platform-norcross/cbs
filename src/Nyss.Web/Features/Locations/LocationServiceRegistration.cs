using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nyss.Web.Features.FakeData;
using Nyss.Web.Features.Locations.Data;

namespace Nyss.Web.Features.Locations
{
    public static class LocationServiceRegistration
    {
        public static void RegisterLocationFeature(this IServiceCollection services)
        {
            services.AddSingleton<ILocationRepository, InMemoryLocationRepository>();
        }
    }
}
