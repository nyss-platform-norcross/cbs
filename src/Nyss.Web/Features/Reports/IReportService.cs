using System.Collections.Generic;
using System.Threading.Tasks;
using Nyss.Web.Features.Report.Data;

namespace Nyss.Web.Features.Reports
{
    public interface IReportService
    {
        IEnumerable<ReportViewModel> All();

        Task<PaginationResult<Nyss.Data.Models.Report>> GetReportsAsync(PaginationOptions options);
    }
}
