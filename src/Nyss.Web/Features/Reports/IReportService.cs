using System.Collections.Generic;
using System.Threading.Tasks;
using Nyss.Web.Features.SlowReports;
using Nyss.Web.Features.SlowReports.Logic;

namespace Nyss.Web.Features.Reports
{
    public interface IReportService
    {
        IEnumerable<ReportViewModel> All();

        Task<PaginationResult<ReportViewModel>> GetReportsAsync(PaginationOptions options);
    }
}
