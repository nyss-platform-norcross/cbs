using System.Collections.Generic;
using System.IO;

namespace Nyss.Web.Features.Reports.Export
{
    public interface ICanExportCaseReports
    {
        string Format { get; }
        string MIMEType { get; }
        string FileExtension { get; }
        void WriteReportsTo(IEnumerable<ReportViewModel> reports, Stream stream);
    }
}
