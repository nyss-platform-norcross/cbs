using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nyss.Data.Models;
using Nyss.Web.Features.Reports.Data;

namespace Nyss.Web.Features.DataCollectors.Data
{
    public class InMemoryDataCollectorRepository: IDataCollectorRepository
    {
        private static readonly List<DataCollector> DataCollectors = new List<DataCollector>();

        public Task<DataCollector> InsertAsync(DataCollector dataCollector)
        {
            if (dataCollector == null) return null;

            dataCollector.Id = DataCollectors.Count + 1;
            DataCollectors.Add(dataCollector);

            return Task.FromResult(dataCollector);
        }

        public Task<DataCollector> GetDataCollectorByPhoneNumberAsync(string phoneNumber)
        {
            return Task.FromResult(DataCollectors.FirstOrDefault(_ => _.PhoneNumber == phoneNumber));
        }

        public Task<IEnumerable<DataCollector>> GetAllAsync()
        {
            return Task.FromResult(DataCollectors.AsEnumerable());
        }

        public Task<PaginationResult<DataCollector>> GetDataCollectorsAsync(PaginationOptions options)
        {
            var query = DataCollectors.AsQueryable();

            var totalCount = query.Count();

            switch (options.Order?.ToLower())
            {
                case "displayname":
                default:
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.DisplayName)
                        : query.OrderByDescending(x => x.DisplayName);
                    break;
            }
                    
            foreach (var searchKeyValuePair in options.SearchDictionary.Where(x => !string.IsNullOrEmpty(x.Value)))
            {
                var search = searchKeyValuePair.Key.ToLower();
                var value = searchKeyValuePair.Value;
                if (search == "region")
                {
                    query = query.Where(x => x.Village.District.Region.Name.Contains(value));
                }
                else if (search == "district")
                {
                    query = query.Where(x => x.Village.District.Name.Contains(value));
                }
                else if (search == "village")
                {
                    query = query.Where(x => x.Village.District.Region.Name.Contains(value));
                }
                else if (search == "supervisor")
                {
                    query = query.Where(x => x.Supervisor.Name.Contains(value));
                }
            }

            var filteredCount = query.Count();

            query = query.Skip(options.Start);

            if (options.Count != -1)
            {
                query = query.Take(options.Count);
            }

            var result = new PaginationResult<DataCollector>
            {
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                Data = query.ToList()
            };

            return Task.FromResult(result);
        }

        public async Task SaveChangesAsync()
        {
            await Task.Delay(0);
        }
    }
}
