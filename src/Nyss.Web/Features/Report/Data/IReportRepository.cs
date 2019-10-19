using System.Threading.Tasks;

namespace Nyss.Web.Features.Report.Data
{
    public interface IReportRepository
    {
        Task<Nyss.Data.Models.Report> InsertAsync(Nyss.Data.Models.Report report);
        Task<PaginationResult<Nyss.Data.Models.Report>> GetReportsAsync(PaginationOptions options);
        Task SaveChangesAsync();
    }
}