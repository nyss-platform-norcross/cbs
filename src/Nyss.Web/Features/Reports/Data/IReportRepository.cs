using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nyss.Web.Features.Reports.Data
{
    public interface IReportRepository
    {
        Task<Nyss.Data.Models.Report> InsertAsync(Nyss.Data.Models.Report report);
        IEnumerable<Nyss.Data.Models.Report> All();
        Task<PaginationResult<Nyss.Data.Models.Report>> GetReportsAsync(PaginationOptions options);
        Task SaveChangesAsync();
    }
}