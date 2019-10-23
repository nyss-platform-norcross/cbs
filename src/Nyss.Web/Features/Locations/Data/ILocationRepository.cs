using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nyss.Data.Models;

namespace Nyss.Web.Features.Locations.Data
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Region>> GetRegionsAsync();
        Task<IEnumerable<District>> GetDistrictsAsync();
        Task<IEnumerable<Village>> GetVillagesAsync();

        Task<IEnumerable<District>> GetDistrictsByRegionAsync(int regionId);
        Task<IEnumerable<Village>> GetVillagesByDistrictAsync(int districtId);

        Task<Region> GetRegionById(int id);
        Task<District> GetDistrictById(int id);
        Task<Village> GetVillageById(int id);


        Task<Region> InsertRegionById(Region region);
        Task<District> InsertDistrictById(District district);
        Task<Village> InsertVillageById(Village village);

        Task SaveChangesAsync();
    }
}
