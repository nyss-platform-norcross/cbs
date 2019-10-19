using System.Collections.Generic;
using System.Threading.Tasks;
using Nyss.Data.Models;
using Nyss.Web.Features.Reports.Data;

namespace Nyss.Web.Features.DataCollectors.Data
{
    public interface IDataCollectorRepository
    {
        Task<DataCollector> InsertAsync(DataCollector dataCollector);
        Task<DataCollector> GetDataCollectorByPhoneNumberAsync(string phoneNumber);
        
        Task<IEnumerable<DataCollector>> GetAllAsync();

        Task<PaginationResult<DataCollector>> GetDataCollectorsAsync(PaginationOptions options);
        Task SaveChangesAsync();
    }
}