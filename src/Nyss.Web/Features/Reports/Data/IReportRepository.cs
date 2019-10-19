using System.Threading.Tasks;
using Nyss.Web.Features.Report.Data;

namespace Nyss.Web.Features.Reports.Data
{
    public interface IReportRepository
    {
        Task<Nyss.Data.Models.Report> InsertAsync(Nyss.Data.Models.Report report);
        Task<PaginationResult<Nyss.Data.Models.Report>> GetReportsAsync(PaginationOptions options);
        Task SaveChangesAsync();
    }
}