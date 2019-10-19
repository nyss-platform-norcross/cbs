using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nyss.Data.Models;
using Nyss.Web.Features.FakeData;
using Nyss.Web.Features.Reports.Data;
using System.Threading.Tasks;

namespace Nyss.Web.Features.Reports
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(
            FakeDataService fakeDataService,
            IReportRepository reportRepository)
        {
            fakeDataService.EnsureFakeDataAsync().Wait();
            _reportRepository = reportRepository;
        }

        public IEnumerable<ReportViewModel> All()
        {
            yield break;
        }

        public Task<PaginationResult<Report>> GetReportsAsync(PaginationOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
