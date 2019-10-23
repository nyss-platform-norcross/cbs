using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nyss.Data.Models;

namespace Nyss.Web.Features.FakeData
{
    public interface IFakeDataService
    {
        Task EnsureFakeDataAsync();
        Task<IEnumerable<DataCollector>> GenerateDataCollectorsAsync(int count, Random r);
        Task<IEnumerable<Report>> GenerateReportsAsync(int count, DateTime from, DateTime to, Random r);
    }
}