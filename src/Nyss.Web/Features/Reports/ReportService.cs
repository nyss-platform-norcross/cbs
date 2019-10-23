using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nyss.Data.Models;
using Nyss.Web.Features.FakeData;
using Nyss.Web.Features.Reports.Data;

namespace Nyss.Web.Features.Reports
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(
            IFakeDataService fakeDataService,
            IReportRepository reportRepository)
        {
            fakeDataService.EnsureFakeDataAsync().Wait();
            _reportRepository = reportRepository;
        }

        public IEnumerable<ReportViewModel> All()
        {
            return _reportRepository.All().Select(_ => _.ToViewModel());
        }

        public async Task<PaginationResult<Report>> GetReportsAsync(PaginationOptions options)
        {
            return await _reportRepository.GetReportsAsync(options);
        }
    }
}
