using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nyss.Data.Models;

namespace Nyss.Web.Features.Locations.Data
{
    public class InMemoryLocationRepository : ILocationRepository
    {
        private static readonly List<Region> Regions = new List<Region>();
        private static readonly List<District> Districts = new List<District>();
        private static readonly List<Village> Villages = new List<Village>();

        private Task<IEnumerable<T>> Results<T>(IEnumerable<T> items) => Task.FromResult(items);
        private Task<IEnumerable<T>> Results<T>(List<T> items) => Results(items.AsEnumerable());

        public Task<IEnumerable<Region>> GetRegionsAsync()
        {
            return Results(Regions);
        }

        public Task<IEnumerable<District>> GetDistrictsAsync()
        {
            return Results(Districts);
        }

        public Task<IEnumerable<Village>> GetVillagesAsync()
        {
            return Results(Villages);
        }

        public Task<IEnumerable<District>> GetDistrictsByRegionAsync(int regionId)
        {
            return Results(Districts.Where(_ => _.Region.Id == regionId));
        }

        public Task<IEnumerable<Village>> GetVillagesByDistrictAsync(int districtId)
        {
            return Results(Villages.Where(_ => _.District.Id == districtId));
        }

        public Task<Region> GetRegionById(int id)
        {
            return Task.FromResult(Regions.FirstOrDefault(_ => _.Id == id));
        }

        public Task<District> GetDistrictById(int id)
        {
            return Task.FromResult(Districts.FirstOrDefault(_ => _.Id == id));
        }

        public Task<Village> GetVillageById(int id)
        {
            return Task.FromResult(Villages.FirstOrDefault(_ => _.Id == id));
        }

        public Task<Region> InsertRegionById(Region region)
        {
            region.Id = Regions.Count + 1;
            Regions.Add(region);
            return Task.FromResult(region);
        }

        public Task<District> InsertDistrictById(District district)
        {
            district.Id = Districts.Count + 1;
            Districts.Add(district);
            return Task.FromResult(district);
        }

        public Task<Village> InsertVillageById(Village village)
        {
            village.Id = Villages.Count + 1;
            Villages.Add(village);
            return Task.FromResult(village);
        }

        public async Task SaveChangesAsync()
        {
            await Task.Delay(0);
        }
    }
}
